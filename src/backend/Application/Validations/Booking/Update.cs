using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.Booking;

public class Update : AbstractValidator<BookingUpdateInfo>
{
    public Update(ITextLocalizer localizer)
    {
        RuleFor(booking => booking.CarId)
            .GreaterThan(0).WithMessage(localizer.Get(TextKeys.Validation.CarIdGreaterThanZero));

        RuleFor(booking => booking.PickupLocationId)
            .GreaterThan(0).WithMessage(localizer.Get(TextKeys.Validation.PickupLocationIdGreaterThanZero));

        RuleFor(booking => booking.DropOffLocationId)
            .GreaterThan(0).WithMessage(localizer.Get(TextKeys.Validation.DropOffLocationIdGreaterThanZero));

        RuleFor(x => x)
            .Must(x => x.PickupLocationId != x.DropOffLocationId)
            .WithMessage(localizer.Get(TextKeys.Validation.PickupAndDropOffDifferent));

        RuleFor(booking => booking.StartDateTime)
            .Must(date => date == null || date.Value >= DateTime.Now)
            .WithMessage(localizer.Get(TextKeys.Validation.StartDateTimeNotPast));

        RuleFor(booking => booking.EndDateTime)
            .Must(date => date == null || date.Value > DateTime.Now)
            .WithMessage(localizer.Get(TextKeys.Validation.EndDateTimeInFuture));

        RuleFor(booking => booking)
            .Must(booking => booking.StartDateTime < booking.EndDateTime)
            .WithMessage(localizer.Get(TextKeys.Validation.StartDateTimeBeforeEndDateTime));

        RuleFor(x => x)
            .Must(x => (x.EndDateTime - x.StartDateTime).Value.TotalHours >= 1)
            .WithMessage(localizer.Get(TextKeys.Validation.BookingDurationMin1Hour));

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage(localizer.Get(TextKeys.Validation.CommentMax500));
    }
}
