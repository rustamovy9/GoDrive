using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.Booking;

public class Update : AbstractValidator<BookingUpdateInfo>
{
    public Update()
    {
        // UserId должен быть положительным числом
        RuleFor(update => update.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than 0.");

        // CarId должен быть положительным числом
        RuleFor(update => update.CarId)
            .GreaterThan(0).WithMessage("CarId must be greater than 0.");

        // StartDateTime должен быть раньше EndDateTime
        RuleFor(update => update)
            .Must(update => update.StartDateTime < update.EndDateTime)
            .WithMessage("StartDateTime must be earlier than EndDateTime.");

        // PickupLocation не может быть пустым и длина не более 100 символов
        RuleFor(update => update.PickupLocation)
            .NotEmpty().WithMessage("PickupLocation is required.")
            .MaximumLength(100).WithMessage("PickupLocation must not exceed 100 characters.");

        // DropOffLocation не может быть пустым и длина не более 100 символов
        RuleFor(update => update.DropOffLocation)
            .NotEmpty().WithMessage("DropOffLocation is required.")
            .MaximumLength(100).WithMessage("DropOffLocation must not exceed 100 characters.");

        // Проверка на будущее время для StartDateTime
        RuleFor(update => update.StartDateTime)
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("StartDateTime must not be in the past.");

        // EndDateTime должен быть не более чем через год от текущей даты
        RuleFor(update => update.EndDateTime)
            .LessThanOrEqualTo(DateTime.Now.AddYears(1)).WithMessage("EndDateTime must be within a year from now.");
    }
}