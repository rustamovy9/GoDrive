using Application.DTO_s;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validations.UserAndAuth;

public class UserUpdateInfoValidator : AbstractValidator<UserUpdateInfo>
{
    public UserUpdateInfoValidator()
    {
        When(x => x.FirstName != null, () =>
        {
            RuleFor(x => x.FirstName!)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("FirstName must not be empty and must not exceed 50 characters.");
        });

        When(x => x.LastName != null, () =>
        {
            RuleFor(x => x.LastName!)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("LastName must not be empty and must not exceed 50 characters.");
        });

        When(x => x.PhoneNumber != null, () =>
        {
            RuleFor(x => x.PhoneNumber!)
                .Matches(@"^\+?\d{10,15}$")
                .WithMessage("PhoneNumber must be a valid international number.");
        });

        When(x => x.Address != null, () =>
        {
            RuleFor(x => x.Address!)
                .MaximumLength(200)
                .WithMessage("Address must not exceed 200 characters.");
        });

        When(x => x.DriverLicense != null, () =>
        {
            RuleFor(x => x.DriverLicense!)
                .MaximumLength(50)
                .WithMessage("DriverLicense must not exceed 50 characters.");
        });

        When(x => x.AvatarPath != null, () =>
        {
            RuleFor(x => x.AvatarPath!)
                .Must(BeValidImage)
                .WithMessage("Avatar must be a valid image file.");
        });
    }

    private static bool BeValidImage(IFormFile file)
    {
        if (file.Length <= 0)
            return false;

        return file.ContentType.StartsWith("image/");
    }
}