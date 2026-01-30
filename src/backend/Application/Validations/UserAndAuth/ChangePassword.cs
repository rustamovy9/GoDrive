using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.UserAndAuth;

public class ChangePasswordInfoValidator : AbstractValidator<ChangePasswordRequest>
{

    public ChangePasswordInfoValidator()
    {
        RuleFor(user => user.OldPassword)
            .NotEmpty().WithMessage("Old password required.")
            .MinimumLength(6).WithMessage("Old password must contain at least 6 characters.")
            .MaximumLength(50).WithMessage("Old password must not exceed 50 characters.");

        RuleFor(user=>user.NewPassword)
            .NotEmpty().WithMessage("New password required.")
            .MinimumLength(6).WithMessage("New password must contain at least 6 characters.")
            .MaximumLength(50).WithMessage("New password must not exceed 50 characters.")
            .NotEqual(x=>x.OldPassword).WithMessage("New password must be different from old password");
        
        RuleFor(user => user.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm is required")
            .Equal(x=>x.NewPassword).WithMessage("Passwords do not match");
    }
    
}