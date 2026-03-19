using Application.Contracts.Localization;
using Application.Localization;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MobileApp.Controllers;

namespace WebAPI.Controllers;

[Route("/error")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController(ITextLocalizer localizer) : BaseController
{
    [HttpGet, HttpPost, HttpPut, HttpDelete, HttpPatch, HttpOptions, HttpHead]
    public IActionResult HandleError()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return exception switch
        {
            ValidationException => Problem(
                title: localizer.Get(TextKeys.General.ValidationExceptionTitle),
                detail: exception?.Message,
                statusCode: StatusCodes.Status400BadRequest,
                instance: HttpContext.Request.Path,
                type: exception?.HelpLink
            ),
            _ => Problem(
                title: localizer.Get(TextKeys.General.UnexpectedErrorTitle),
                detail: exception?.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                instance: HttpContext.Request.Path,
                type: "https://metanit.com"
            )
        };
    }
}
