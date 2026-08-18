using Microsoft.AspNetCore.Http;

namespace HeartlandCIM.Web.Services;

/// <summary>Helpers for reading/writing the technician name held in session.</summary>
public static class SessionExtensions
{
    public const string TechKey = "TechnicianName";

    public static string? GetTechnician(this ISession session) =>
        session.GetString(TechKey);

    public static void SetTechnician(this ISession session, string name) =>
        session.SetString(TechKey, name);

    public static bool HasTechnician(this ISession session) =>
        !string.IsNullOrWhiteSpace(session.GetString(TechKey));
}
