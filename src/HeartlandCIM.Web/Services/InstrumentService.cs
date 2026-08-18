using HeartlandCIM.Web.Data;
using HeartlandCIM.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace HeartlandCIM.Web.Services;

public class InstrumentService : IInstrumentService
{
    private readonly ApplicationDbContext _db;
    private readonly ILogService _log;

    public InstrumentService(ApplicationDbContext db, ILogService log)
    {
        _db = db;
        _log = log;
    }

    public async Task<CalibrationInstrument?> GetByIdAsync(int id) =>
        await _db.CalibrationInstruments.FirstOrDefaultAsync(i => i.Id == id);

    public async Task<List<CalibrationInstrument>> GetAllAsync() =>
        await _db.CalibrationInstruments.OrderBy(i => i.Title).ToListAsync();

    public async Task<List<CalibrationInstrument>> GetByAreaAsync(string area) =>
        await _db.CalibrationInstruments
            .Where(i => i.Area == area)
            .OrderBy(i => i.Title)
            .ToListAsync();

    public async Task<List<CalibrationInstrument>> GetDueInAreaAsync(string area, DateTime cutOff)
    {
        return await _db.CalibrationInstruments
            .Where(i => i.Area == area
                        && i.AreaStatus == "Open"
                        && i.Feedback_Verified_Time == null
                        && i.Next_Cal_Date != null
                        && i.Next_Cal_Date <= cutOff)
            .OrderBy(i => i.Title)
            .ToListAsync();
    }

    public async Task<List<CalibrationInstrument>> GetVerifiedInAreaAsync(string area) =>
        await _db.CalibrationInstruments
            .Where(i => i.Area == area && i.Feedback_Verified_Time != null)
            .OrderBy(i => i.Title)
            .ToListAsync();

    public async Task RemoveAsync(int id, string technician)
    {
        var i = await GetByIdAsync(id) ?? throw new InvalidOperationException("Instrument not found.");
        i.Removed_Time = DateTime.UtcNow;
        i.Removed_Tech = technician;
        i.Modified = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        await _log.CreateAsync(i.Title, technician, "Instrument Removed");
    }

    public async Task InstallAsync(int id, string technician, bool alsoVerify)
    {
        var i = await GetByIdAsync(id) ?? throw new InvalidOperationException("Instrument not found.");
        i.Install_Time = DateTime.UtcNow;
        i.Install_Tech = technician;
        if (alsoVerify)
        {
            i.Feedback_Verified_Time = DateTime.UtcNow;
            i.Feedback_Verified_Tech = technician;
        }
        i.Modified = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        await _log.CreateAsync(i.Title, technician,
            alsoVerify ? "Instrument Installed & Verified" : "Instrument Installed");
    }

    public async Task VerifyAsync(int id, string technician)
    {
        var i = await GetByIdAsync(id) ?? throw new InvalidOperationException("Instrument not found.");
        i.Feedback_Verified_Time = DateTime.UtcNow;
        i.Feedback_Verified_Tech = technician;
        i.Modified = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        await _log.CreateAsync(i.Title, technician, "Feedback Verified");
    }

    public async Task<int> UpdateNextCalDatesAsync()
    {
        var all = await _db.CalibrationInstruments
            .Where(i => i.Last_Cal_Date != null)
            .ToListAsync();
        foreach (var i in all)
        {
            var months = string.Equals(i.PointType, "CCP", StringComparison.OrdinalIgnoreCase) ? 6 : 12;
            i.Next_Cal_Date = i.Last_Cal_Date!.Value.AddMonths(months).Date;
            i.Modified = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync();
        return all.Count;
    }

    public async Task<int> ResetCalibrationCycleAsync()
    {
        var all = await _db.CalibrationInstruments.ToListAsync();
        foreach (var i in all)
        {
            i.Removed_Time = null;
            i.Removed_Tech = null;
            i.Install_Time = null;
            i.Install_Tech = null;
            i.Feedback_Verified_Time = null;
            i.Feedback_Verified_Tech = null;
            i.Modified = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync();
        return all.Count;
    }

    public async Task UpdatePhotoAsync(int id, string imageType, string newRelativePath, string technician)
    {
        var i = await GetByIdAsync(id) ?? throw new InvalidOperationException("Instrument not found.");

        var isWide = string.Equals(imageType, "Wide", StringComparison.OrdinalIgnoreCase)
                     || string.Equals(imageType, "Wide_Pic", StringComparison.OrdinalIgnoreCase);

        var oldPath = isWide ? i.Wide_PicPath : i.Detail_PicPath;

        if (isWide) i.Wide_PicPath = newRelativePath;
        else i.Detail_PicPath = newRelativePath;
        i.Modified = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        await _log.CreateAsync(
            i.Title, technician,
            isWide ? "Updated Wide_Pic" : "Updated Detail_Pic",
            archivedPicPath: oldPath,
            imageType: isWide ? "Wide" : "Detail");
    }

    public Task SaveAsync() => _db.SaveChangesAsync();
}
