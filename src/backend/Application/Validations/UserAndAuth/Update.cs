using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validations.UserAndAuth;

public class Update : AbstractValidator<UserUpdateInfo>
{
    public Update(ITextLocalizer localizer)
    {
        When(x => x.FirstName != null, () =>
        {
            RuleFor(x => x.FirstName!)
                .NotEmpty().WithMessage(localizer.Get(TextKeys.Validation.FirstNameRequired))
                .MaximumLength(50).WithMessage(localizer.Get(TextKeys.Validation.FirstNameMax50));
        });

        When(x => x.LastName != null, () =>
        {
            RuleFor(x => x.LastName!)
                .NotEmpty().WithMessage(localizer.Get(TextKeys.Validation.LastNameRequired))
                .MaximumLength(50).WithMessage(localizer.Get(TextKeys.Validation.LastNameMax50));
        });

        When(x => x.PhoneNumber != null, () =>
        {
            RuleFor(x => x.PhoneNumber!)
                .Must(x => FluentValidationHelpers.IsPhoneNumberValid(x!))
                .WithMessage(localizer.Get(TextKeys.Validation.PhoneNumberInvalid));
        });

        When(x => x.Address != null, () =>
        {
            RuleFor(x => x.Address!)
                .MaximumLength(200)
                .WithMessage(localizer.Get(TextKeys.Validation.AddressMax200));
        });

        When(x => x.Avatar != null, () =>
        {
            RuleFor(x => x.Avatar!)
                .Must(IsImageFile)
                .WithMessage(localizer.Get(TextKeys.Validation.AvatarInvalidImage));
        });
    }

    private static bool IsImageFile(IFormFile file)
    {
        if (file == null || file.Length == 0) return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        return allowedExtensions.Contains(fileExtension);
    }
}
