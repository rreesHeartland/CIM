using System.ComponentModel.DataAnnotations;

namespace HeartlandCIM.Web.Models;

/// <summary>
/// Represents a single calibration instrument. Converted from the
/// SharePoint "CalibrationInstruments" list used by the original PowerApp.
/// </summary>
public class CalibrationInstrument
{
    public int Id { get; set; }

    /// <summary>Instrument tag name, e.g. "TT.1010".</summary>
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    /// <summary>MegaMation Part Number.</summary>
    [MaxLength(255)]
    public string? MM { get; set; }

    [MaxLength(255)]
    public string Area { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Description { get; set; }

    /// <summary>"CP" or "CCP".</summary>
    [MaxLength(50)]
    public string? PointType { get; set; }

    [MaxLength(255)]
    public string? InstrumentType { get; set; }

    [MaxLength(50)]
    public string? Low { get; set; }

    [MaxLength(50)]
    public string? Mid { get; set; }

    [MaxLength(50)]
    public string? High { get; set; }

    /// <summary>Unit of Measure.</summary>
    [MaxLength(100)]
    public string? U_of_M { get; set; }

    [MaxLength(100)]
    public string? Tolerance { get; set; }

    [MaxLength(255)]
    public string? Manufacturer { get; set; }

    [MaxLength(255)]
    public string? ModelNumber { get; set; }

    [MaxLength(255)]
    public string? SerialNumber { get; set; }

    [MaxLength(100)]
    public string? Cal_Frequency { get; set; }

    public DateTime? Last_Cal_Date { get; set; }

    public DateTime? Next_Cal_Date { get; set; }

    /// <summary>"Open" or "Closed" - mirrors the owning area's status.</summary>
    [MaxLength(50)]
    public string? AreaStatus { get; set; }

    /// <summary>"Ground Level", "Ladder Required", "Scissor Lift", "Crawl under".</summary>
    [MaxLength(100)]
    public string? AccessRequirement { get; set; }

    [MaxLength(255)]
    public string? Tool_Requirement { get; set; }

    /// <summary>Non-null means tanks must be drained before service.</summary>
    [MaxLength(255)]
    public string? DrainTanks { get; set; }

    public DateTime? Removed_Time { get; set; }
    [MaxLength(255)]
    public string? Removed_Tech { get; set; }

    public DateTime? Install_Time { get; set; }
    [MaxLength(255)]
    public string? Install_Tech { get; set; }

    public DateTime? Feedback_Verified_Time { get; set; }
    [MaxLength(255)]
    public string? Feedback_Verified_Tech { get; set; }

    /// <summary>Relative path (under wwwroot) for the close-up image.</summary>
    [MaxLength(500)]
    public string? Detail_PicPath { get; set; }

    /// <summary>Relative path (under wwwroot) for the wide-angle image.</summary>
    [MaxLength(500)]
    public string? Wide_PicPath { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Modified { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Computed calibration workflow status. Never stored as a column - always
    /// derived from the timestamp fields, matching the PowerApp logic.
    /// </summary>
    public string Status
    {
        get
        {
            if (Feedback_Verified_Time != null) return "Verified";
            if (Install_Time != null) return "Installed";
            if (Removed_Time != null) return "Removed";
            return "Not Started";
        }
    }
}
