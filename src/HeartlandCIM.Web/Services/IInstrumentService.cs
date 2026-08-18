using HeartlandCIM.Web.Models;

namespace HeartlandCIM.Web.Services;

public interface IInstrumentService
{
    Task<CalibrationInstrument?> GetByIdAsync(int id);
    Task<List<CalibrationInstrument>> GetAllAsync();
    Task<List<CalibrationInstrument>> GetByAreaAsync(string area);

    /// <summary>
    /// Instruments due for calibration in an area: AreaStatus = Open,
    /// Feedback_Verified_Time null, Next_Cal_Date &lt;= cutoff.
    /// </summary>
    Task<List<CalibrationInstrument>> GetDueInAreaAsync(string area, DateTime cutOff);

    /// <summary>Verified instruments in an area (Feedback_Verified_Time set).</summary>
    Task<List<CalibrationInstrument>> GetVerifiedInAreaAsync(string area);

    Task RemoveAsync(int id, string technician);
    Task InstallAsync(int id, string technician, bool alsoVerify);
    Task VerifyAsync(int id, string technician);

    /// <summary>Batch: recalculate Next_Cal_Date (CCP +6mo, CP +12mo) from Last_Cal_Date.</summary>
    Task<int> UpdateNextCalDatesAsync();

    /// <summary>Batch: clear all Removed/Install/Verified tech + time fields.</summary>
    Task<int> ResetCalibrationCycleAsync();

    Task UpdatePhotoAsync(int id, string imageType, string newRelativePath, string technician);

    Task SaveAsync();
}
