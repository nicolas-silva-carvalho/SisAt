using Microsoft.AspNetCore.Mvc;

namespace SisAt.Controllers;

public class HomeController : Controller
{
    public IActionResult Index(string returnUrl = null)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "AgendamentoHorarios");
        }

        Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, private";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }
}
