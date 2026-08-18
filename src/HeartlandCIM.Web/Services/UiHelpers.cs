namespace HeartlandCIM.Web.Services;

/// <summary>Presentation helpers shared across views.</summary>
public static class UiHelpers
{
    public static string StatusCssClass(string status) => status switch
    {
        "Verified" => "status-verified",
        "Installed" => "status-installed",
        "Removed" => "status-removed",
        _ => "status-not-started"
    };

    /// <summary>
    /// Colour rule for AccessRequirement:
    /// Scissor Lift / Crawl under = red bold, Ladder = italic, Ground Level = plain.
    /// </summary>
    public static string AccessCssClass(string? access)
    {
        if (string.IsNullOrEmpty(access)) return "access-plain";
        var a = access.ToLowerInvariant();
        if (a.Contains("scissor") || a.Contains("crawl")) return "access-danger";
        if (a.Contains("ladder")) return "access-ladder";
        return "access-plain";
    }
}
