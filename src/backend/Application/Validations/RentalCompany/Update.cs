using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.RentalCompany;

public class Update : AbstractValidator<RentalCompanyUpdateInfo>
{
    public Update(ITextLocalizer localizer)
    {
        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name!)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage(localizer.Get(TextKeys.Validation.CompanyNameRequiredMax200));
        });

        When(x => x.ContactInfo != null, () =>
        {
            RuleFor(x => x.ContactInfo!)
                .MaximumLength(500)
                .WithMessage(localizer.Get(TextKeys.Validation.ContactInfoMax500));
        });
    }
}
