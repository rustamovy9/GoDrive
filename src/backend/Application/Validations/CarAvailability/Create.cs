using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.CarAvailability;

public class Create : AbstractValidator<CarAvailabilityCreateInfo>
{
    public Create()
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage("CarId must be greater than 0.");

        RuleFor(x => x.AvailableFrom)
            .GreaterThanOrEqualTo(DateTimeOffset.UtcNow)
            .WithMessage("AvailableFrom must not be in the past.");

        RuleFor(x => x)
            .Must(x => x.AvailableFrom < x.AvailableTo)
            .WithMessage("AvailableFrom must be earlier than AvailableTo.");

        RuleFor(x => x)
            .Must(x => (x.AvailableTo - x.AvailableFrom).TotalMinutes >= 30)
            .WithMessage("Availability duration must be at least 30 minutes.");
    }
}