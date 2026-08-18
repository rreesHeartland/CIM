using HeartlandCIM.Web.Services;
using HeartlandCIM.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HeartlandCIM.Web.Controllers;

public class CalibrationsController : Controller
{
    private readonly ICycleService _cycle;
    private readonly IAreaService _areas;
    private readonly IInstrumentService _instruments;

    public CalibrationsController(ICycleService cycle, IAreaService areas, IInstrumentService instruments)
    {
        _cycle = cycle;
        _areas = areas;
        _instruments = instruments;
    }

    private bool EnsureTech(out IActionResult? redirect)
    {
        if (!HttpContext.Session.HasTechnician())
        {
            redirect = RedirectToAction("Login", "Account",
                new { returnUrl = Request.Path + Request.QueryString });
            return false;
        }
        redirect = null;
        return true;
    }

    public async Task<IActionResult> SelectArea()
    {
        if (!EnsureTech(out var r)) return r!;

        var vm = new SelectAreaViewModel
        {
            CycleConfirmed = await _cycle.IsCycleConfirmedAsync(),
            CurrentCycle = await _cycle.GetCurrentCycleAsync(),
            Areas = await _areas.GetAreaProgressAsync()
        };
        return View(vm);
    }

    public async Task<IActionResult> SelectInstrument(string area)
    {
        if (!EnsureTech(out var r)) return r!;
        if (string.IsNullOrWhiteSpace(area)) return RedirectToAction(nameof(SelectArea));

        var cutOff = await _cycle.GetCutOffDateAsync();
        var vm = new SelectInstrumentViewModel
        {
            Area = area,
            CurrentCycle = await _cycle.GetCurrentCycleAsync(),
            CutOffDate = cutOff,
            DueInstruments = await _instruments.GetDueInAreaAsync(area, cutOff),
            VerifiedInstruments = await _instruments.GetVerifiedInAreaAsync(area)
        };
        return View(vm);
    }

    public async Task<IActionResult> InstrumentDetails(int id)
    {
        if (!EnsureTech(out var r)) return r!;

        var instrument = await _instruments.GetByIdAsync(id);
        if (instrument == null) return NotFound();
        return View(instrument);
    }

    /// <summary>AJAX endpoint that performs a workflow action and returns JSON.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DoAction(int id, string action, bool alsoVerify = false)
    {
        var tech = HttpContext.Session.GetTechnician();
        if (string.IsNullOrWhiteSpace(tech))
            return Json(new { success = false, message = "Session expired. Please sign in again." });

        var instrument = await _instruments.GetByIdAsync(id);
        if (instrument == null)
            return Json(new { success = false, message = "Instrument not found." });

        string actionPerformed;
        switch (action?.ToLowerInvariant())
        {
            case "remove":
                await _instruments.RemoveAsync(id, tech);
                actionPerformed = "Removed";
                break;
            case "install":
                await _instruments.InstallAsync(id, tech, alsoVerify);
                actionPerformed = alsoVerify ? "Installed & Verified" : "Installed";
                break;
            case "verify":
                await _instruments.VerifyAsync(id, tech);
                actionPerformed = "Verified";
                break;
            default:
                return Json(new { success = false, message = "Unknown action." });
        }

        var resultUrl = Url.Action(nameof(Result), new
        {
            title = instrument.Title,
            area = instrument.Area,
            performed = actionPerformed
        });
        return Json(new { success = true, redirectUrl = resultUrl });
    }

    public IActionResult Result(string title, string area, string performed)
    {
        var vm = new ResultViewModel
        {
            Title = title ?? string.Empty,
            Area = area ?? string.Empty,
            ActionPerformed = performed ?? string.Empty
        };
        return View(vm);
    }
}
