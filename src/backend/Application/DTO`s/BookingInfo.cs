namespace Application.DTO_s;

public interface IBaseBookingInfo
{
    public int UserId { get; init; }
    public int CarId { get; init; }
    public DateTime StartDateTime { get; init; }
    public DateTime EndDateTime { get; init; }
    public string? PickupLocation { get; init; }
    public string? DropOffLocation { get; init; }
}

public readonly record struct BookingReadInfo(
    int UserId,
    int CarId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string PickupLocation,
    string DropOffLocation,
    string Status,
    int Id) : IBaseBookingInfo;

public readonly record struct BookingUpdateInfo(
    int UserId,
    int CarId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string PickupLocation,
    string DropOffLocation) : IBaseBookingInfo;

public readonly record struct BookingCreateInfo(
    int UserId,
    int CarId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string PickupLocation,
    string DropOffLocation) : IBaseBookingInfo;