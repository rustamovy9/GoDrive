using Application.DTO_s;
using Domain.Constants;
using FluentValidation;

namespace Application.Validations.UserAndAuth;

public class RegisterInfoValidator : AbstractValidator<RegisterRequest>
{
    public RegisterInfoValidator()
    {
        RuleFor(user => user.UserName)
            .NotEmpty()
            .MaximumLength(40)
            .Matches("^[a-zA-Z0-9_]+$")
            .WithMessage("UserName can contain only letters, numbers and underscore");


        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(50).WithMessage("FirstName must not exceed 50 characters.");

        RuleFor(user => user.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(50).WithMessage("LastName must not exceed 50 characters.");

        RuleFor(user => user.DateOfBirth)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow).WithMessage("DateOfBirth cannot be in the future.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(user => user.PhoneNumber)
            .Matches(@"^\+?\d{10,15}$")
            .When(user => !string.IsNullOrEmpty(user.PhoneNumber))
            .WithMessage("PhoneNumber must be a valid international number.");
        
        
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("The password must contain at least 6 characters.")
            .MaximumLength(50).WithMessage("Password must not exceed 50 characters.")
            .NotEqual(x=>x.UserName).WithMessage("Password cannot be the same as UserName");
        
        RuleFor(user => user.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm is required")
            .Equal(x=>x.Password).WithMessage("Passwords do not match");
    }
}