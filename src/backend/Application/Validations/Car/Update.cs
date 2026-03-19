using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.Car;

public class Update : AbstractValidator<CarUpdateInfo>
{
    public Update(ITextLocalizer localizer)
    {
        When(x => x.Brand != null, () =>
        {
            RuleFor(x => x.Brand!)
                .NotEmpty()
                .WithMessage(localizer.Get(TextKeys.Validation.BrandRequiredMax100))
                .MaximumLength(100)
                .WithMessage(localizer.Get(TextKeys.Validation.BrandRequiredMax100));
        });

        When(x => x.Model != null, () =>
        {
            RuleFor(x => x.Model!)
                .NotEmpty()
                .WithMessage(localizer.Get(TextKeys.Validation.ModelRequiredMax100))
                .MaximumLength(100)
                .WithMessage(localizer.Get(TextKeys.Validation.ModelRequiredMax100));
        });

        When(x => x.Year.HasValue, () =>
        {
            RuleFor(x => x.Year!.Value)
                .InclusiveBetween(1950, DateTime.UtcNow.Year + 1)
                .WithMessage(localizer.Get(TextKeys.Validation.YearValidProductionYear));
        });

        When(x => x.CategoryId.HasValue, () =>
        {
            RuleFor(x => x.CategoryId!.Value)
                .GreaterThan(0)
                .WithMessage(localizer.Get(TextKeys.Validation.CategoryIdGreaterThanZero));
        });

        When(x => x.LocationId.HasValue, () =>
        {
            RuleFor(x => x.LocationId!.Value)
                .GreaterThan(0)
                .WithMessage(localizer.Get(TextKeys.Validation.LocationIdGreaterThanZero));
        });

        When(x => x.RentalCompanyId.HasValue, () =>
        {
            RuleFor(x => x.RentalCompanyId!.Value)
                .GreaterThan(0)
                .WithMessage(localizer.Get(TextKeys.Validation.RentalCompanyIdGreaterThanZero));
        });
    }
}
