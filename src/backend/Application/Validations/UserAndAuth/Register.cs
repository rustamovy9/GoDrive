using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.UserAndAuth;

public class Register : AbstractValidator<RegisterRequest>
{
    public Register(ITextLocalizer localizer)
    {
        RuleFor(x => x.UserName)
            .Matches("^[a-zA-Z0-9_]+$")
            .WithMessage(localizer.Get(TextKeys.Validation.UserNameValidChars));

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(localizer.Get(TextKeys.Validation.FirstNameRequired))
            .MaximumLength(50).WithMessage(localizer.Get(TextKeys.Validation.FirstNameMax50));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(localizer.Get(TextKeys.Validation.LastNameRequired))
            .MaximumLength(50).WithMessage(localizer.Get(TextKeys.Validation.LastNameMax50));

        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow)
            .WithMessage(localizer.Get(TextKeys.Validation.DateOfBirthNotFuture));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer.Get(TextKeys.Validation.EmailRequired))
            .EmailAddress().WithMessage(localizer.Get(TextKeys.Validation.EmailInvalid));

        RuleFor(x => x.PhoneNumber)
            .Must(x => FluentValidationHelpers.IsPhoneNumberValid(x!))
            .WithMessage(localizer.Get(TextKeys.Validation.PhoneNumberInvalid));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer.Get(TextKeys.Validation.PasswordRequired))
            .MinimumLength(6).WithMessage(localizer.Get(TextKeys.Validation.PasswordMin6))
            .MaximumLength(50).WithMessage(localizer.Get(TextKeys.Validation.PasswordMax50))
            .NotEqual(x => x.UserName)
            .WithMessage(localizer.Get(TextKeys.Validation.PasswordNotSameAsUserName));

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(localizer.Get(TextKeys.Validation.ConfirmRequired))
            .Equal(x => x.Password).WithMessage(localizer.Get(TextKeys.Validation.PasswordsDoNotMatch));
    }
}
