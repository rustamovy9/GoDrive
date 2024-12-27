using Domain.Enums;

namespace Application.DTO_s;

public interface IBaseBookingInfo
{
    public int UserId { get; init; }
    public int CarId { get; init; }
    public DateTimeOffset StartDateTime { get; init; }
    public DateTimeOffset EndDateTime { get; init; }
    public string? PickupLocation { get; init; }
    public string? DropOffLocation { get; init; }
}

public readonly record struct BookingReadInfo(
    int UserId,
    int CarId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime,
    string PickupLocation,
    string DropOffLocation,
    Status Status,
    int Id) : IBaseBookingInfo;

public readonly record struct BookingUpdateInfo(
    int UserId,
    int CarId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime,
    string PickupLocation,
    string DropOffLocation) : IBaseBookingInfo;

public readonly record struct BookingCreateInfo(
    int UserId,
    int CarId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime,
    string PickupLocation,
    string DropOffLocation) : IBaseBookingInfo;