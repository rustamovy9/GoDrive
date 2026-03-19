using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.UserAndAuth;

public class Login : AbstractValidator<LoginRequest>
{
    public Login(ITextLocalizer localizer)
    {
        RuleFor(x => x.UserNameOrEmail)
            .NotEmpty().WithMessage(localizer.Get(TextKeys.Validation.LoginUserNameOrEmailRequired));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer.Get(TextKeys.Validation.PasswordRequired))
            .MinimumLength(6).WithMessage(localizer.Get(TextKeys.Validation.PasswordMin6))
            .MaximumLength(50).WithMessage(localizer.Get(TextKeys.Validation.PasswordMax50));
    }
}
