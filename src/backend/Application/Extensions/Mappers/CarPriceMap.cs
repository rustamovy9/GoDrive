using Application.DTO_s;
using Domain.Entities;
using Domain.Enums;

namespace Application.Extensions.Mappers;

public static class CarPriceMap
{
    public static CarPriceReadInfo ToRead(this CarPrice price)
    {
        return new CarPriceReadInfo(
            Id: price.Id,
            CarId: price.CarId,
            PricePerDay: price.PricePerDay,
            Currency: price.Currency);
    }
    
    public static CarPrice ToEntity(this CarPriceCreateInfo createInfo)
    {
        return new CarPrice
        {
            CarId = createInfo.CarId,
            PricePerDay = createInfo.PricePerDay,
            Currency = Currency.TJS 
        };
    }
    
    public static CarPrice ToEntity(
        this CarPrice entity,
        CarPriceUpdateInfo updateInfo)
    {
        if (updateInfo.PricePerDay.HasValue)
            entity.PricePerDay = updateInfo.PricePerDay.Value;

        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        return entity;
    }
}