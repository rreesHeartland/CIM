namespace HeartlandCIM.Web.Services;

/// <summary>
/// Encapsulates all calibration-cycle business logic (Spring/Fall, cutoff dates).
/// </summary>
public interface ICycleService
{
    /// <summary>The currently confirmed cycle across areas, e.g. "Spring 2026", or null if none.</summary>
    Task<string?> GetCurrentCycleAsync();

    /// <summary>True when at least one area has a confirmed (non-empty) Current_Cycle.</summary>
    Task<bool> IsCycleConfirmedAsync();

    /// <summary>
    /// The cutoff date for the current cycle. Spring = Apr 30, Fall = Oct 31 of the
    /// cycle year. Falls back to the current year's cycle if none is confirmed.
    /// </summary>
    Task<DateTime> GetCutOffDateAsync();

    /// <summary>The suggested cycle for a given date ("Spring {yr}" if month &lt;= 6, else "Fall {yr}").</summary>
    string SuggestCycle(DateTime date);

    /// <summary>Cutoff date derived from a cycle label like "Spring 2026".</summary>
    DateTime GetCutOffDateForCycle(string cycle);
}
