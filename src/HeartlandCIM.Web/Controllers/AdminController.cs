using HeartlandCIM.Web.Services;
using HeartlandCIM.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HeartlandCIM.Web.Controllers;

public class AdminController : Controller
{
    private readonly IAreaService _areas;
    private readonly IInstrumentService _instruments;
    private readonly ICycleService _cycle;

    public AdminController(IAreaService areas, IInstrumentService instruments, ICycleService cycle)
    {
        _areas = areas;
        _instruments = instruments;
        _cycle = cycle;
    }

    public IActionResult Index() => View();

    public async Task<IActionResult> AreaManagement()
    {
        var all = await _areas.GetAllAreasAsync();
        var now = DateTime.Today;
        var vm = new AreaManagementViewModel
        {
            Areas = all,
            AnyOpen = all.Any(a => a.Status == "Open"),
            CurrentCycle = await _cycle.GetCurrentCycleAsync(),
            CycleOptions = new List<string>
            {
                $"Spring {now.Year}",
                $"Fall {now.Year}"
            }
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetCycle(string cycle)
    {
        if (!string.IsNullOrWhiteSpace(cycle))
            await _areas.SetCycleAsync(cycle);
        TempData["Message"] = $"Calibration cycle set to {cycle}.";
        return RedirectToAction(nameof(AreaManagement));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetCycleLabel()
    {
        var ok = await _areas.ResetCycleLabelAsync();
        TempData["Message"] = ok
            ? "Calibration cycle cleared."
            : "Cannot reset the cycle while one or more areas are Open. Close all areas first.";
        return RedirectToAction(nameof(AreaManagement));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetAreaStatus(int id, string status)
    {
        await _areas.SetAreaStatusAsync(id, status);
        TempData["Message"] = "Area status updated.";
        return RedirectToAction(nameof(AreaManagement));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCalDates()
    {
        var count = await _instruments.UpdateNextCalDatesAsync();
        TempData["Message"] = $"Updated next calibration dates for {count} instrument(s).";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetCycle()
    {
        var count = await _instruments.ResetCalibrationCycleAsync();
        TempData["Message"] = $"Reset calibration progress for {count} instrument(s).";
        return RedirectToAction(nameof(Index));
    }
}
