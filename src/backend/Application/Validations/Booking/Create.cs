using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.Booking;

public class Create : AbstractValidator<BookingCreateInfo>
{
    public Create()
    {
        RuleFor(booking => booking.CarId)
            .GreaterThan(0).WithMessage("CarId must be greater than 0.");


        RuleFor(booking => booking.PickupLocationId)
            .GreaterThan(0).WithMessage("PickupLocation must be greater than 0.");

        RuleFor(booking => booking.DropOffLocationId)
            .GreaterThan(0).WithMessage("DropOffLocation must be greater than 0.");

        RuleFor(x => x)
            .Must(x => x.PickupLocationId != x.DropOffLocationId)
            .WithMessage("Pickup and Drop-off locations cannot be the same");
        
        
        RuleFor(booking => booking)
            .Must(booking => booking.StartDateTime < booking.EndDateTime)
            .WithMessage("StartDateTime must be earlier than EndDateTime.");
        
        RuleFor(booking => booking.StartDateTime)
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("StartDateTime must not be in the past.");
        
        
        RuleFor(x => x)
            .Must(x => (x.EndDateTime - x.StartDateTime).TotalHours >= 1)
            .WithMessage("Booking duration must be at least 1 hour.");
        
        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage("Comment must not exceed 500 characters.");
    }
}