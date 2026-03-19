using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.CarPrice;

public class Create : AbstractValidator<CarPriceCreateInfo>
{
    public Create(ITextLocalizer localizer)
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage(localizer.Get(TextKeys.Validation.CarIdGreaterThanZero));

        RuleFor(x => x.PricePerDay)
            .GreaterThan(0)
            .WithMessage(localizer.Get(TextKeys.Validation.PricePerDayGreaterThanZero));
    }
}
