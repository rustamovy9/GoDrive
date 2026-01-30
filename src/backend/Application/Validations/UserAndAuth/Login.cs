using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.UserAndAuth;

public class LoginInfoValidator : AbstractValidator<LoginRequest>
{
    public LoginInfoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Email or UserName is required.")
            .EmailAddress().WithMessage("Invalid email  format.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must contain at least 6 characters.")
            .MaximumLength(50).WithMessage("Password must not exceed 50 characters.");
    }
    
}