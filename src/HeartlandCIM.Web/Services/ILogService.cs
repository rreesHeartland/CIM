using HeartlandCIM.Web.Models;

namespace HeartlandCIM.Web.Services;

public interface ILogService
{
    Task<InstrumentLog> CreateAsync(
        string itemId,
        string? technicianName,
        string actionTaken,
        string? archivedPicPath = null,
        string? imageType = null);
}
