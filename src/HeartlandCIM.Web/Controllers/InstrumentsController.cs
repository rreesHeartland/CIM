using HeartlandCIM.Web.Services;
using HeartlandCIM.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HeartlandCIM.Web.Controllers;

public class InstrumentsController : Controller
{
    private readonly IAreaService _areas;
    private readonly IInstrumentService _instruments;

    public InstrumentsController(IAreaService areas, IInstrumentService instruments)
    {
        _areas = areas;
        _instruments = instruments;
    }

    public async Task<IActionResult> Records(string? area, int? id, string? search)
    {
        var vm = new RecordsViewModel
        {
            Areas = await _areas.GetAllAreaNamesAsync(),
            SelectedArea = area,
            Search = search
        };

        if (!string.IsNullOrWhiteSpace(area))
            vm.Instruments = await _instruments.GetByAreaAsync(area);
        else
            vm.Instruments = await _instruments.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(search))
            vm.Instruments = vm.Instruments
                .Where(i => i.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (id.HasValue)
            vm.Selected = await _instruments.GetByIdAsync(id.Value);

        return View(vm);
    }
}
