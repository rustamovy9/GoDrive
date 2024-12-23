using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.Booking;

public class Create : AbstractValidator<BookingReadInfo>
{
    public Create()
    {
        // UserId должен быть положительным числом
        RuleFor(booking => booking.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than 0.");

        // CarId должен быть положительным числом
        RuleFor(booking => booking.CarId)
            .GreaterThan(0).WithMessage("CarId must be greater than 0.");

        // StartDateTime должен быть раньше EndDateTime
        RuleFor(booking => booking)
            .Must(booking => booking.StartDateTime < booking.EndDateTime)
            .WithMessage("StartDateTime must be earlier than EndDateTime.");

        // PickupLocation не может быть пустым
        RuleFor(booking => booking.PickupLocation)
            .NotEmpty().WithMessage("PickupLocation is required.");

        // DropOffLocation не может быть пустым
        RuleFor(booking => booking.DropOffLocation)
            .NotEmpty().WithMessage("DropOffLocation is required.");

        // Проверка, что время бронирования не в прошлом
        RuleFor(booking => booking.StartDateTime)
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("StartDateTime must not be in the past.");
    }
}