using HeartlandCIM.Web.Data;
using HeartlandCIM.Web.Models;

namespace HeartlandCIM.Web.Services;

public class LogService : ILogService
{
    private readonly ApplicationDbContext _db;

    public LogService(ApplicationDbContext db) => _db = db;

    public async Task<InstrumentLog> CreateAsync(
        string itemId,
        string? technicianName,
        string actionTaken,
        string? archivedPicPath = null,
        string? imageType = null)
    {
        var log = new InstrumentLog
        {
            ItemID = itemId,
            Technician_Name = technicianName,
            Action_Taken = actionTaken,
            Archived_PicPath = archivedPicPath,
            Image_Type = imageType,
            LogTimeStamp = DateTime.UtcNow,
            Created = DateTime.UtcNow
        };
        _db.InstrumentLogs.Add(log);
        await _db.SaveChangesAsync();
        return log;
    }
}
