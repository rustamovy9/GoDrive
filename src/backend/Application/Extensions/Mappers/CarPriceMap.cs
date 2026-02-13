using Application.DTO_s;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class CarPriceMap
{
    public static CarPriceReadInfo ToRead(this CarPrice price)
        => new(
            price.Id,
            price.CarId,
            price.PricePerDay,
            price.Currency);

    public static CarPrice ToEntity(this CarPriceCreateInfo createInfo)
        => new()
        {
            CarId = createInfo.CarId,
            PricePerDay = createInfo.PricePerDay,
            Currency = createInfo.Currency
        };

    public static CarPrice ToEntity(this CarPrice entity, CarPriceUpdateInfo updateInfo)
    {
        if (updateInfo.PricePerDay.HasValue)
            entity.PricePerDay = updateInfo.PricePerDay.Value;

        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        return entity;
    }
}