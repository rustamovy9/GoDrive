using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.RentalCompany;

public class Create : AbstractValidator<RentalCompanyCreateInfo>
{
    public Create(ITextLocalizer localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage(localizer.Get(TextKeys.Validation.CompanyNameRequiredMax200));

        RuleFor(x => x.ContactInfo)
            .MaximumLength(500)
            .WithMessage(localizer.Get(TextKeys.Validation.ContactInfoMax500));
    }
}
