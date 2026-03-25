using Domain.Enums;

namespace Application.DTO_s;
public sealed record BookingReadInfo(
    int Id,
    int CarId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime,
    decimal TotalPrice,
    BookingStatus BookingStatus,
    PaymentStatus PaymentStatus,
    bool IsContactShared,
    string PickupCity,
    string DropOffCity);

public sealed record BookingUpdateInfo(
    int? CarId,
    DateTimeOffset? StartDateTime,
    DateTimeOffset? EndDateTime,
    int? PickupLocationId,
    int? DropOffLocationId,
    string? Comment);

public sealed record BookingUpdateStatusInfo(
    BookingStatus Status,
    string? Reason);

public sealed record BookingCreateInfo(
    int CarId,
    DateTimeOffset StartDateTime,
    DateTimeOffset EndDateTime,
    int PickupLocationId,
    int DropOffLocationId,
    string? Comment);