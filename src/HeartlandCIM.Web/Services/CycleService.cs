using HeartlandCIM.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace HeartlandCIM.Web.Services;

public class CycleService : ICycleService
{
    private readonly ApplicationDbContext _db;

    public CycleService(ApplicationDbContext db) => _db = db;

    public async Task<string?> GetCurrentCycleAsync()
    {
        return await _db.AreaStatuses
            .Where(a => a.Current_Cycle != null && a.Current_Cycle != "")
            .Select(a => a.Current_Cycle)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsCycleConfirmedAsync()
    {
        return await _db.AreaStatuses
            .AnyAsync(a => a.Current_Cycle != null && a.Current_Cycle != "");
    }

    public async Task<DateTime> GetCutOffDateAsync()
    {
        var cycle = await GetCurrentCycleAsync();
        if (!string.IsNullOrWhiteSpace(cycle))
            return GetCutOffDateForCycle(cycle);

        // fall back to the suggested cycle for today
        return GetCutOffDateForCycle(SuggestCycle(DateTime.Today));
    }

    public string SuggestCycle(DateTime date)
    {
        var season = date.Month <= 6 ? "Spring" : "Fall";
        return $"{season} {date.Year}";
    }

    public DateTime GetCutOffDateForCycle(string cycle)
    {
        // cycle format: "<Season> <Year>"
        var parts = cycle.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var season = parts.Length > 0 ? parts[0] : "Spring";
        var year = DateTime.Today.Year;
        if (parts.Length > 1 && int.TryParse(parts[1], out var y)) year = y;

        return season.Equals("Fall", StringComparison.OrdinalIgnoreCase)
            ? new DateTime(year, 10, 31)
            : new DateTime(year, 4, 30);
    }
}
