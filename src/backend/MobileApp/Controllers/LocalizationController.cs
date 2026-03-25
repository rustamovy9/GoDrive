using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace MobileApp.Controllers;

[Route("api/localization")]
public sealed class LocalizationController : BaseController
{
    private static readonly Dictionary<string, string> CultureMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ru"] = "ru",
        ["en"] = "en",
        ["tg"] = "tg",
        ["tj"] = "tg" // alias for Tajik
    };

    [AllowAnonymous]
    [HttpPost("set")]
    public IActionResult SetLanguage([FromQuery] string culture)
    {
        if (string.IsNullOrWhiteSpace(culture) || !CultureMap.TryGetValue(culture, out var mapped))
            return BadRequest(new { Error = "Unsupported culture. Use: ru, en, tj (tg also accepted)." });

        var requestCulture = new RequestCulture(mapped);
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

        return Ok(new { Culture = culture, MappedTo = mapped });
    }

    [AllowAnonymous]
    [HttpGet("current")]
    public IActionResult Current()
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        var publicCulture = culture.Equals("tg", StringComparison.OrdinalIgnoreCase) ? "tj" : culture;
        return Ok(new { Culture = publicCulture });
    }
}
