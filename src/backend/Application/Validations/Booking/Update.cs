using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.Booking;

public class Update : AbstractValidator<BookingUpdateInfo>
{
    public Update()
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage("CarId must be greater than 0.");

        RuleFor(x => x.PickupLocationId)
            .GreaterThan(0)
            .WithMessage("PickupLocationId must be greater than 0.");

        RuleFor(x => x.DropOffLocationId)
            .GreaterThan(0)
            .WithMessage("DropOffLocationId must be greater than 0.");

        RuleFor(x => x)
            .Must(x => x.PickupLocationId != x.DropOffLocationId)
            .WithMessage("Pickup and Drop-off locations cannot be the same.");

        When(x => x.StartDateTime.HasValue, () =>
        {
            RuleFor(x => x.StartDateTime!.Value)
                .GreaterThanOrEqualTo(DateTimeOffset.UtcNow)
                .WithMessage("StartDateTime must not be in the past.");
        });

        When(x => x.EndDateTime.HasValue, () =>
        {
            RuleFor(x => x.EndDateTime!.Value)
                .GreaterThan(DateTimeOffset.UtcNow)
                .WithMessage("EndDateTime must be in the future.");
        });

        RuleFor(x => x)
            .Must(x => x.StartDateTime < x.EndDateTime)
            .WithMessage("StartDateTime must be earlier than EndDateTime.");

        RuleFor(x => x)
            .Must(x =>
                !x.StartDateTime.HasValue ||
                !x.EndDateTime.HasValue ||
                (x.EndDateTime.Value - x.StartDateTime.Value).TotalHours >= 1)
            .WithMessage("Booking duration must be at least 1 hour.");

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage("Comment must not exceed 500 characters.");
    }
}