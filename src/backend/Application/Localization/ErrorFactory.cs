using Application.Contracts.Localization;
using Domain.Common;

namespace Application.Localization;

public static class ErrorFactory
{
    public static Error NotFound(ITextLocalizer localizer)
        => Error.NotFound(localizer.Get(TextKeys.Errors.NotFoundDefault));

    public static Error BadRequest(ITextLocalizer localizer)
        => Error.BadRequest(localizer.Get(TextKeys.Errors.BadRequestDefault));

    public static Error Forbidden(ITextLocalizer localizer)
        => Error.Forbidden(localizer.Get(TextKeys.Errors.ForbiddenDefault));

    public static Error AlreadyExist(ITextLocalizer localizer)
        => Error.AlreadyExist(localizer.Get(TextKeys.Errors.AlreadyExistDefault));

    public static Error Conflict(ITextLocalizer localizer)
        => Error.Conflict(localizer.Get(TextKeys.Errors.ConflictDefault));

    public static Error InternalServerError(ITextLocalizer localizer)
        => Error.InternalServerError(localizer.Get(TextKeys.Errors.InternalServerErrorDefault));
}
