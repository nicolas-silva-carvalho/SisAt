using Microsoft.AspNetCore.Mvc;
namespace SisAt.Controllers;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HandleErrorCode(int statusCode)
    {
        return View("Error");

    }
}
