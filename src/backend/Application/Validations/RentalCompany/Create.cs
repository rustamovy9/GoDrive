using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.RentalCompany;

public class Create : AbstractValidator<RentalCompanyCreateInfo>
{
    public Create()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Company name is required and must not exceed 200 characters.");

        RuleFor(x => x.ContactInfo)
            .MaximumLength(500)
            .WithMessage("ContactInfo must not exceed 500 characters.");
    }
}