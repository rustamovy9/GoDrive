using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.CarAvailability;

public class Update : AbstractValidator<CarAvailabilityUpdateInfo>
{
    public Update(ITextLocalizer localizer)
    {
        When(x => x.AvailableFrom.HasValue, () =>
        {
            RuleFor(x => x.AvailableFrom!.Value)
                .GreaterThanOrEqualTo(DateTimeOffset.UtcNow)
                .WithMessage(localizer.Get(TextKeys.Validation.AvailableFromNotPast));
        });

        When(x => x.AvailableTo.HasValue, () =>
        {
            RuleFor(x => x.AvailableTo!.Value)
                .GreaterThan(DateTimeOffset.UtcNow)
                .WithMessage(localizer.Get(TextKeys.Validation.AvailableToInFuture));
        });

        RuleFor(x => x)
            .Must(x =>
                !x.AvailableFrom.HasValue ||
                !x.AvailableTo.HasValue ||
                x.AvailableFrom.Value < x.AvailableTo.Value)
            .WithMessage(localizer.Get(TextKeys.Validation.AvailableFromBeforeAvailableTo));

        RuleFor(x => x)
            .Must(x =>
                !x.AvailableFrom.HasValue ||
                !x.AvailableTo.HasValue ||
                (x.AvailableTo.Value - x.AvailableFrom.Value).TotalMinutes >= 30)
            .WithMessage(localizer.Get(TextKeys.Validation.AvailabilityDurationMin30Minutes));
    }
}
