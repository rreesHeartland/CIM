using HeartlandCIM.Web.Models;

namespace HeartlandCIM.Web.ViewModels;

public class AreaProgressViewModel
{
    public string Area { get; set; } = string.Empty;
    public int Total { get; set; }
    public int NotStarted { get; set; }
    public int Removed { get; set; }
    public int Installed { get; set; }
    public int Verified { get; set; }

    public int Pct(int count) => Total == 0 ? 0 : (int)Math.Round(count * 100.0 / Total);
    public int NotStartedPct => Pct(NotStarted);
    public int RemovedPct => Pct(Removed);
    public int InstalledPct => Pct(Installed);
    public int VerifiedPct => Pct(Verified);
}

public class HomeViewModel
{
    public string? CurrentCycle { get; set; }
    public bool CycleConfirmed { get; set; }
}

public class SelectAreaViewModel
{
    public bool CycleConfirmed { get; set; }
    public string? CurrentCycle { get; set; }
    public List<AreaProgressViewModel> Areas { get; set; } = new();
}

public class SelectInstrumentViewModel
{
    public string Area { get; set; } = string.Empty;
    public string? CurrentCycle { get; set; }
    public DateTime CutOffDate { get; set; }
    public List<CalibrationInstrument> DueInstruments { get; set; } = new();
    public List<CalibrationInstrument> VerifiedInstruments { get; set; } = new();
}

public class ResultViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string ActionPerformed { get; set; } = string.Empty;
}

public class AreaManagementViewModel
{
    public List<AreaStatus> Areas { get; set; } = new();
    public List<string> CycleOptions { get; set; } = new();
    public string? CurrentCycle { get; set; }
    public bool AnyOpen { get; set; }
}

public class RecordsViewModel
{
    public List<string> Areas { get; set; } = new();
    public string? SelectedArea { get; set; }
    public string? Search { get; set; }
    public List<CalibrationInstrument> Instruments { get; set; } = new();
    public CalibrationInstrument? Selected { get; set; }
}

public class ImageBrowserViewModel
{
    public List<string> Areas { get; set; } = new();
    public string? SelectedArea { get; set; }
    public List<CalibrationInstrument> Instruments { get; set; } = new();
}
