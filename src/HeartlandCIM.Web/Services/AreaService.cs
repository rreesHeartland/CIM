using HeartlandCIM.Web.Data;
using HeartlandCIM.Web.Models;
using HeartlandCIM.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HeartlandCIM.Web.Services;

public class AreaService : IAreaService
{
    private readonly ApplicationDbContext _db;

    public AreaService(ApplicationDbContext db) => _db = db;

    public async Task<List<AreaStatus>> GetAllAreasAsync() =>
        await _db.AreaStatuses.OrderBy(a => a.Title).ToListAsync();

    public async Task<List<string>> GetOpenAreaNamesAsync() =>
        await _db.AreaStatuses
            .Where(a => a.Status == "Open")
            .OrderBy(a => a.Title)
            .Select(a => a.Title)
            .ToListAsync();

    public async Task<List<string>> GetAllAreaNamesAsync() =>
        await _db.CalibrationInstruments
            .Select(i => i.Area)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();

    public async Task<List<AreaProgressViewModel>> GetAreaProgressAsync()
    {
        var openAreas = await GetOpenAreaNamesAsync();

        // Pull the relevant instruments once, then compute status in memory
        // (Status is a computed property and cannot be translated to SQL).
        var instruments = await _db.CalibrationInstruments
            .Where(i => openAreas.Contains(i.Area))
            .ToListAsync();

        return openAreas.Select(area =>
        {
            var list = instruments.Where(i => i.Area == area).ToList();
            return new AreaProgressViewModel
            {
                Area = area,
                Total = list.Count,
                NotStarted = list.Count(i => i.Status == "Not Started"),
                Removed = list.Count(i => i.Status == "Removed"),
                Installed = list.Count(i => i.Status == "Installed"),
                Verified = list.Count(i => i.Status == "Verified"),
            };
        }).ToList();
    }

    public async Task SetAreaStatusAsync(int areaId, string status)
    {
        var area = await _db.AreaStatuses.FindAsync(areaId);
        if (area == null) return;

        area.Status = status;
        area.Modified = DateTime.UtcNow;

        // Mirror the status onto the instruments in that area (PowerApp behaviour).
        var instruments = await _db.CalibrationInstruments
            .Where(i => i.Area == area.Title)
            .ToListAsync();
        foreach (var i in instruments)
        {
            i.AreaStatus = status;
            i.Modified = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
    }

    public async Task SetCycleAsync(string cycle)
    {
        var areas = await _db.AreaStatuses.ToListAsync();
        foreach (var a in areas)
        {
            a.Current_Cycle = cycle;
            a.Modified = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync();
    }

    public async Task<bool> ResetCycleLabelAsync()
    {
        var anyOpen = await _db.AreaStatuses.AnyAsync(a => a.Status == "Open");
        if (anyOpen) return false;

        var areas = await _db.AreaStatuses.ToListAsync();
        foreach (var a in areas)
        {
            a.Current_Cycle = null;
            a.Modified = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync();
        return true;
    }
}
