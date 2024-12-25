using Application.DTO_s;
using Domain.Entities;
using Domain.Enums;

namespace Application.Extensions.Mappers;

public static class BookingMapper
{
    public static BookingReadInfo ToRead(this Booking booking)
    {
        return new BookingReadInfo(
            booking.UserId,
            booking.CarId,
            booking.StartDateTime,
            booking.EndDateTime,
            booking.PickupLocation,
            booking.DropOffLocation,
            booking.Status,
            booking.Id
        );
    }


    public static Booking ToEntity(this BookingCreateInfo createInfo)
    {
        return new Booking
        {
            UserId = createInfo.UserId,
            CarId = createInfo.CarId,
            StartDateTime = createInfo.StartDateTime,
            EndDateTime = createInfo.EndDateTime,
            PickupLocation = createInfo.PickupLocation,
            DropOffLocation = createInfo.DropOffLocation,
            Status = Status.InProgress
        };
    }

    public static Booking ToEntity(this Booking entity, BookingUpdateInfo updateInfo)
    {
        entity.UserId = updateInfo.UserId;
        entity.CarId = updateInfo.CarId;
        entity.StartDateTime = updateInfo.StartDateTime;
        entity.EndDateTime = updateInfo.EndDateTime;
        entity.PickupLocation = updateInfo.PickupLocation;
        entity.DropOffLocation = updateInfo.DropOffLocation;
        entity.Status = Status.Pending ;
        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        return entity;
    }
}