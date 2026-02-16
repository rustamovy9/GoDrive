using Application.DTO_s;
using Domain.Entities;
using Domain.Enums;


namespace Application.Extensions.Mappers;

public static class CarMapper
{
    public static CarReadInfo ToRead(this Car car)
    {
        return new CarReadInfo(
            car.Id,
            car.Brand,
            car.Model,
            car.Year,
            car.RegistrationNumber,
            car.CarStatus,
            car.CategoryId,
            car.LocationId,
            car.RentalCompanyId,
            car.CarImages
                .Select(ci => ci.ImagePath)
                .ToList(),  
            car.CreatedAt
        );
    }


    public static  Car ToEntity(this CarCreateInfo createInfo)
    {
        return new Car
        {
            Brand = createInfo.Brand,
            Model = createInfo.Model,
            Year = createInfo.Year,
            RegistrationNumber = createInfo.RegistrationNumber,
            
            CategoryId = createInfo.CategoryId,
            LocationId = createInfo.LocationId,
            RentalCompanyId = createInfo.RentalCompanyId, 
        };
    }

    public static Car ToEntity(this Car entity, CarUpdateInfo updateInfo)
    {
        if (updateInfo.Brand is not null)
            entity.Brand = updateInfo.Brand;

        if (updateInfo.Model is not null)
            entity.Model = updateInfo.Model;

        if (updateInfo.Year.HasValue)
            entity.Year = updateInfo.Year.Value;

        if (updateInfo.CategoryId.HasValue)
            entity.CategoryId = updateInfo.CategoryId.Value;

        if (updateInfo.LocationId.HasValue)
            entity.LocationId = updateInfo.LocationId.Value;

        if (updateInfo.RentalCompanyId.HasValue)
            entity.RentalCompanyId = updateInfo.RentalCompanyId;

        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        return entity;
    }
}