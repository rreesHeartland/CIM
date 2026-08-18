using System.ComponentModel.DataAnnotations;

namespace HeartlandCIM.Web.Models;

/// <summary>
/// Audit log entry. Converted from the SharePoint "InstrumentLog" list.
/// </summary>
public class InstrumentLog
{
    public int Id { get; set; }

    /// <summary>References the instrument Title.</summary>
    [MaxLength(255)]
    public string ItemID { get; set; } = string.Empty;

    public DateTime LogTimeStamp { get; set; } = DateTime.UtcNow;

    [MaxLength(255)]
    public string? Technician_Name { get; set; }

    /// <summary>
    /// e.g. "Instrument Removed", "Instrument Installed",
    /// "Instrument Installed &amp; Verified", "Feedback Verified",
    /// "Updated Detail_Pic", "Updated Wide_Pic".
    /// </summary>
    [MaxLength(500)]
    public string? Action_Taken { get; set; }

    [MaxLength(500)]
    public string? Archived_PicPath { get; set; }

    [MaxLength(50)]
    public string? Image_Type { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;
}
