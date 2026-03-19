using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.CarAvailability;

public class Create : AbstractValidator<CarAvailabilityCreateInfo>
{
    public Create(ITextLocalizer localizer)
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage(localizer.Get(TextKeys.Validation.CarIdGreaterThanZero));

        RuleFor(x => x.AvailableFrom)
            .GreaterThanOrEqualTo(DateTimeOffset.UtcNow)
            .WithMessage(localizer.Get(TextKeys.Validation.AvailableFromNotPast));

        RuleFor(x => x)
            .Must(x => x.AvailableFrom < x.AvailableTo)
            .WithMessage(localizer.Get(TextKeys.Validation.AvailableFromBeforeAvailableTo));

        RuleFor(x => x)
            .Must(x => (x.AvailableTo - x.AvailableFrom).TotalMinutes >= 30)
            .WithMessage(localizer.Get(TextKeys.Validation.AvailabilityDurationMin30Minutes));
    }
}
