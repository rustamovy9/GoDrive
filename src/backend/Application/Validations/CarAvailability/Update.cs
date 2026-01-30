using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.CarAvailability;

public class Update : AbstractValidator<CarAvailabilityUpdateInfo>
{
    public Update()
    {
        When(x => x.AvailableFrom.HasValue, () =>
        {
            RuleFor(x => x.AvailableFrom!.Value)
                .GreaterThanOrEqualTo(DateTimeOffset.UtcNow)
                .WithMessage("AvailableFrom must not be in the past.");
        });

        When(x => x.AvailableTo.HasValue, () =>
        {
            RuleFor(x => x.AvailableTo!.Value)
                .GreaterThan(DateTimeOffset.UtcNow)
                .WithMessage("AvailableTo must be in the future.");
        });

        RuleFor(x => x)
            .Must(x =>
                !x.AvailableFrom.HasValue ||
                !x.AvailableTo.HasValue ||
                x.AvailableFrom.Value < x.AvailableTo.Value)
            .WithMessage("AvailableFrom must be earlier than AvailableTo.");

        RuleFor(x => x)
            .Must(x =>
                !x.AvailableFrom.HasValue ||
                !x.AvailableTo.HasValue ||
                (x.AvailableTo.Value - x.AvailableFrom.Value).TotalMinutes >= 30)
            .WithMessage("Availability duration must be at least 30 minutes.");
    }
}