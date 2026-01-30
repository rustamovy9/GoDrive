using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.RentalCompany;

public class Update : AbstractValidator<RentalCompanyUpdateInfo>
{
    public Update()
    {
        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name!)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Company name must not be empty and must not exceed 200 characters.");
        });

        When(x => x.ContactInfo != null, () =>
        {
            RuleFor(x => x.ContactInfo!)
                .MaximumLength(500)
                .WithMessage("ContactInfo must not exceed 500 characters.");
        });
    }
}