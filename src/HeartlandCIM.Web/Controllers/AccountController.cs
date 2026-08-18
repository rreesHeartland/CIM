using HeartlandCIM.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeartlandCIM.Web.Controllers;

/// <summary>
/// Very lightweight technician sign-in. No Windows Auth - the technician simply
/// types their name once and it is stored in session for logging attribution.
/// </summary>
public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string technicianName, string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(technicianName))
        {
            ModelState.AddModelError(nameof(technicianName), "Please enter your name to continue.");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        HttpContext.Session.SetTechnician(technicianName.Trim());

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
