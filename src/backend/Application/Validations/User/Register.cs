using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.User;

public class RegisterInfoValidator : AbstractValidator<RegisterRequest>
{
    public RegisterInfoValidator()
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

        RuleFor(user => user.DateOfBirth)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow).WithMessage("DateOfBirth cannot be in the future.");

        RuleFor(user => user.EmailAddress)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(user => user.PhoneNumber)
            .Matches(@"^\+?\d{10,15}$")
            .When(user => !string.IsNullOrEmpty(user.PhoneNumber))
            .WithMessage("PhoneNumber must be a valid international number.");
        
    }
}