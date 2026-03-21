using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace MobileApp.Controllers;

[Route("api/localization")]
public sealed class LocalizationController : BaseController
{
    private static readonly HashSet<string> SupportedCultures = new(StringComparer.OrdinalIgnoreCase)
    {
        "ru",
        "tg",
        "en"
    };

    [AllowAnonymous]
    [HttpPost("set")]
    public IActionResult SetLanguage([FromQuery] string culture)
    {
        if (string.IsNullOrWhiteSpace(culture) || !SupportedCultures.Contains(culture))
            return BadRequest(new { Error = "Unsupported culture. Use: ru, tg, en." });

        var requestCulture = new RequestCulture(culture);
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(requestCulture),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                HttpOnly = false,
                SameSite = SameSiteMode.Lax
            });

        return Ok(new { Culture = culture });
    }

    [AllowAnonymous]
    [HttpGet("current")]
    public IActionResult Current()
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        return Ok(new { Culture = culture });
    }
}
