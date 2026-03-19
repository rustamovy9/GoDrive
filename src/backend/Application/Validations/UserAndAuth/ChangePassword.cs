using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.UserAndAuth;

public class ChangePassword : AbstractValidator<ChangePasswordRequest>
{
    public ChangePassword(ITextLocalizer localizer)
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage(localizer.Get(TextKeys.Validation.OldPasswordRequired))
            .MinimumLength(6).WithMessage(localizer.Get(TextKeys.Validation.OldPasswordMin6))
            .MaximumLength(50).WithMessage(localizer.Get(TextKeys.Validation.OldPasswordMax50));

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(localizer.Get(TextKeys.Validation.NewPasswordRequired))
            .MinimumLength(6).WithMessage(localizer.Get(TextKeys.Validation.NewPasswordMin6))
            .MaximumLength(50).WithMessage(localizer.Get(TextKeys.Validation.NewPasswordMax50))
            .NotEqual(x => x.OldPassword)
            .WithMessage(localizer.Get(TextKeys.Validation.NewPasswordDifferentFromOld));

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(localizer.Get(TextKeys.Validation.ConfirmRequired))
            .Equal(x => x.NewPassword).WithMessage(localizer.Get(TextKeys.Validation.PasswordsDoNotMatch));
    }
}
