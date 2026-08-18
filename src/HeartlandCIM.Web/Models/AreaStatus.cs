using System.ComponentModel.DataAnnotations;

namespace HeartlandCIM.Web.Models;

/// <summary>
/// One row per physical area. Converted from the SharePoint "AreaStatus" list.
/// </summary>
public class AreaStatus
{
    public int Id { get; set; }

    /// <summary>Area name.</summary>
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    /// <summary>"Open" or "Closed".</summary>
    [MaxLength(50)]
    public string Status { get; set; } = "Closed";

    /// <summary>e.g. "Spring 2026" or "Fall 2026". Null when no cycle confirmed.</summary>
    [MaxLength(100)]
    public string? Current_Cycle { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Modified { get; set; } = DateTime.UtcNow;
}
