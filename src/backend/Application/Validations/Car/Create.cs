using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.Car;

public class Create : AbstractValidator<CarCreateInfo>
{
    public Create(ITextLocalizer localizer)
    {
        RuleFor(x => x.Brand)
            .NotEmpty()
            .WithMessage(localizer.Get(TextKeys.Validation.BrandRequiredMax100))
            .MaximumLength(100)
            .WithMessage(localizer.Get(TextKeys.Validation.BrandRequiredMax100));

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage(localizer.Get(TextKeys.Validation.ModelRequiredMax100))
            .MaximumLength(100)
            .WithMessage(localizer.Get(TextKeys.Validation.ModelRequiredMax100));

        RuleFor(x => x.Year)
            .InclusiveBetween(1950, DateTime.UtcNow.Year + 1)
            .WithMessage(localizer.Get(TextKeys.Validation.YearValidProductionYear));

        RuleFor(x => x.RegistrationNumber)
            .NotEmpty()
            .WithMessage(localizer.Get(TextKeys.Validation.RegistrationNumberRequiredMax50))
            .MaximumLength(50)
            .WithMessage(localizer.Get(TextKeys.Validation.RegistrationNumberRequiredMax50));

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage(localizer.Get(TextKeys.Validation.CategoryIdGreaterThanZero));

        RuleFor(x => x.LocationId)
            .GreaterThan(0)
            .WithMessage(localizer.Get(TextKeys.Validation.LocationIdGreaterThanZero));
    }
}
