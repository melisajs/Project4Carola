using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Controllers;

public class LanguageController : Controller
{
    public IActionResult Set(string culture, string? returnUrl = null)
    {
        culture = culture == "en-US" ? "en-US" : "tr-TR";
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true });

        return LocalRedirect(Url.IsLocalUrl(returnUrl) ? returnUrl! : "/");
    }
}
