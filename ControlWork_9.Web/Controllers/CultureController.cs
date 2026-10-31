using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ControWork_9.Controllers;

public class CultureController : Controller
{
    [HttpGet]
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public IActionResult Set(string? culture, string returnUrl = "/")
    {
        if (!string.IsNullOrEmpty(culture))
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true, Path = "/" }
            );   
        }
        if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
        {
            return RedirectToAction("Index", "Main");
        }
        return LocalRedirect(returnUrl);
    }
}
