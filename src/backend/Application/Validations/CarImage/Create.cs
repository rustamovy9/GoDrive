using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validations.CarImage;

public class Create : AbstractValidator<CarImageCreateInfo>
{
    public Create(ITextLocalizer localizer)
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage(localizer.Get(TextKeys.Validation.CarIdGreaterThanZero));

        RuleFor(x => x.File)
            .NotNull().WithMessage(localizer.Get(TextKeys.Validation.CarImageRequired))
            .Must(BeValidImage)
            .WithMessage(localizer.Get(TextKeys.Validation.FileValidImageUnder5Mb));
    }

    private static bool BeValidImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        // 5MB лимит
        const long maxSize = 5 * 1024 * 1024;
        if (file.Length > maxSize)
            return false;

        var allowedTypes = new[]
        {
            "image/jpeg",
            "image/pdf",
            "image/png",
            "image/jpg",
            "image/webp"
        };

        return allowedTypes.Contains(file.ContentType.ToLower());
    }
}
