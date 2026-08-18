using HeartlandCIM.Web.Models;
using HeartlandCIM.Web.ViewModels;

namespace HeartlandCIM.Web.Services;

public interface IAreaService
{
    Task<List<AreaStatus>> GetAllAreasAsync();

    /// <summary>Distinct area names that currently have AreaStatus = "Open".</summary>
    Task<List<string>> GetOpenAreaNamesAsync();

    /// <summary>Distinct area names from all instruments.</summary>
    Task<List<string>> GetAllAreaNamesAsync();

    /// <summary>Per-area progress (counts + percentages) for the open cycle.</summary>
    Task<List<AreaProgressViewModel>> GetAreaProgressAsync();

    Task SetAreaStatusAsync(int areaId, string status);

    /// <summary>Applies the given cycle label to every area record.</summary>
    Task SetCycleAsync(string cycle);

    /// <summary>Clears Current_Cycle on every area (only allowed when no area is Open).</summary>
    Task<bool> ResetCycleLabelAsync();
}
