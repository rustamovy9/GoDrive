using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.CarPrice;

public class Update : AbstractValidator<CarPriceUpdateInfo>
{
    public Update(ITextLocalizer localizer)
    {
        RuleFor(x => x.PricePerDay)
            .GreaterThan(0)
            .WithMessage(localizer.Get(TextKeys.Validation.PricePerDayGreaterThanZero));
    }
}
