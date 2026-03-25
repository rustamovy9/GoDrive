using Application.DTO_s;
using Domain.Entities;
using Domain.Enums;

namespace Application.Extensions.Mappers;

public static class BookingMapper
{
    public static BookingReadInfo ToRead(this Booking booking)
    {
        return new BookingReadInfo(
            booking.Id,
            booking.CarId,
            booking.StartDateTime,
            booking.EndDateTime,
            booking.TotalPrice,
            booking.BookingStatus,
            booking.PaymentStatus,
            booking.IsContactShared,
            booking.PickupLocation.City,
            booking.DropOffLocation.City
        );
    }


    public static Booking ToEntity(this BookingCreateInfo createInfo,int userId)
    {
        return new Booking
        {
            UserId = userId,
            CarId = createInfo.CarId,
            StartDateTime = createInfo.StartDateTime,
            EndDateTime = createInfo.EndDateTime,
            PickupLocationId = createInfo.PickupLocationId,
            DropOffLocationId = createInfo.DropOffLocationId,
            
            BookingStatus = BookingStatus.Pending,
            PaymentStatus = PaymentStatus.PendingAgreement,
            IsContactShared = false
        };
    }

    public static Booking ToEntity(this Booking entity, BookingUpdateInfo updateInfo)
    {
        if (updateInfo.StartDateTime.HasValue)
            entity.StartDateTime = updateInfo.StartDateTime.Value;

        if (updateInfo.EndDateTime.HasValue)
            entity.EndDateTime = updateInfo.EndDateTime.Value;

        if (updateInfo.PickupLocationId.HasValue)
            entity.PickupLocationId = updateInfo.PickupLocationId.Value;

        if (updateInfo.DropOffLocationId.HasValue)
            entity.DropOffLocationId = updateInfo.DropOffLocationId.Value;

        if (updateInfo.Comment is not null)
            entity.Comment = updateInfo.Comment;

        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        return entity;
    }
}