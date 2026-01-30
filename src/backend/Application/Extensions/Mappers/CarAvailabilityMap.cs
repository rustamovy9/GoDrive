using Application.DTO_s;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class CarAvailabilityMapper
{
    public static CarAvailabilityReadInfo ToRead(this CarAvailability availability)
    {
        return new CarAvailabilityReadInfo(
            Id: availability.Id,
            CarId: availability.CarId,
            AvailableFrom: availability.AvailableFrom,
            AvailableTo: availability.AvailableTo,
            IsAvailable: availability.IsAvailable);
    }

    public static CarAvailability ToEntity(this CarAvailabilityCreateInfo createInfo)
    {
        return new CarAvailability
        {
            CarId = createInfo.CarId,
            AvailableFrom = createInfo.AvailableFrom,
            AvailableTo = createInfo.AvailableTo,
            IsAvailable = true
        };
    }
}