using System.Diagnostics;
using HeartlandCIM.Web.Models;
using HeartlandCIM.Web.Services;
using HeartlandCIM.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HeartlandCIM.Web.Controllers;

public class HomeController : Controller
{
    private readonly ICycleService _cycle;

    public HomeController(ICycleService cycle) => _cycle = cycle;

    public async Task<IActionResult> Index()
    {
        // Require the technician name on first use.
        if (!HttpContext.Session.HasTechnician())
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Home") });

        var vm = new HomeViewModel
        {
            CurrentCycle = await _cycle.GetCurrentCycleAsync(),
            CycleConfirmed = await _cycle.IsCycleConfirmedAsync()
        };
        return View(vm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
