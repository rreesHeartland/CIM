using HeartlandCIM.Web.Services;
using HeartlandCIM.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HeartlandCIM.Web.Controllers;

public class ImagesController : Controller
{
    private readonly IAreaService _areas;
    private readonly IInstrumentService _instruments;
    private readonly IWebHostEnvironment _env;

    public ImagesController(IAreaService areas, IInstrumentService instruments, IWebHostEnvironment env)
    {
        _areas = areas;
        _instruments = instruments;
        _env = env;
    }

    public async Task<IActionResult> Browser(string? area)
    {
        var vm = new ImageBrowserViewModel
        {
            Areas = await _areas.GetAllAreaNamesAsync(),
            SelectedArea = area
        };
        if (!string.IsNullOrWhiteSpace(area))
            vm.Instruments = await _instruments.GetByAreaAsync(area);
        return View(vm);
    }

    public async Task<IActionResult> Updater(int id)
    {
        var instrument = await _instruments.GetByIdAsync(id);
        if (instrument == null) return NotFound();
        return View(instrument);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(20_000_000)]
    public async Task<IActionResult> UploadPhoto(int id, string imageType, IFormFile? photo)
    {
        var tech = HttpContext.Session.GetTechnician() ?? "Unknown";
        var instrument = await _instruments.GetByIdAsync(id);
        if (instrument == null) return NotFound();

        if (photo == null || photo.Length == 0)
        {
            TempData["Message"] = "No file was selected.";
            return RedirectToAction(nameof(Updater), new { id });
        }

        // Store under wwwroot/uploads/{instrumentId}/
        var folder = Path.Combine(_env.WebRootPath, "uploads", id.ToString());
        Directory.CreateDirectory(folder);

        var ext = Path.GetExtension(photo.FileName);
        var isWide = string.Equals(imageType, "Wide", StringComparison.OrdinalIgnoreCase);
        var fileName = $"{(isWide ? "wide" : "detail")}_{DateTime.UtcNow:yyyyMMddHHmmss}{ext}";
        var fullPath = Path.Combine(folder, fileName);

        await using (var stream = System.IO.File.Create(fullPath))
        {
            await photo.CopyToAsync(stream);
        }

        var relativePath = $"/uploads/{id}/{fileName}";
        await _instruments.UpdatePhotoAsync(id, isWide ? "Wide" : "Detail", relativePath, tech);

        TempData["Message"] = "Photo updated successfully.";
        return RedirectToAction(nameof(Updater), new { id });
    }
}
