using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.User;

public class UserUpdateInfoValidator : AbstractValidator<UserUpdateInfo>
{
    public UserUpdateInfoValidator()
    {
        RuleFor(user => user.UserName)
            .NotEmpty().WithMessage("UserName is required.")
            .MaximumLength(50).WithMessage("UserName must not exceed 50 characters.");

        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(50).WithMessage("FirstName must not exceed 50 characters.");

        RuleFor(user => user.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(50).WithMessage("LastName must not exceed 50 characters.");

        RuleFor(user => user.Age)
            .InclusiveBetween(0, 120).WithMessage("Age must be between 0 and 120.");

        RuleFor(user => user.DateOfBirth)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow).WithMessage("DateOfBirth cannot be in the future.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(user => user.PhoneNumber)
            .Matches(@"^\+?\d{10,15}$")
            .When(user => !string.IsNullOrEmpty(user.PhoneNumber))
            .WithMessage("PhoneNumber must be a valid international number.");

        RuleFor(user => user.Address)
            .MaximumLength(200).WithMessage("Address must not exceed 200 characters.")
            .When(user => !string.IsNullOrEmpty(user.Address));

        RuleFor(user => user.DriverLicense)
            .MaximumLength(20).WithMessage("DriverLicense must not exceed 20 characters.")
            .When(user => !string.IsNullOrEmpty(user.DriverLicense));

        RuleFor(user => user.File)
            .Must(file => file != null && file.Length > 0)
            .WithMessage("File must not be empty.")
            .When(user => user.File != null);

        RuleFor(user => user.File)
            .Must(file => file.ContentType.StartsWith("image/"))
            .WithMessage("File must be an image.")
            .When(user => user.File != null);
    }
}