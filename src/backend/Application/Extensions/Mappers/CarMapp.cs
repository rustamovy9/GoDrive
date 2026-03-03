using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Constants;
using Domain.Entities;


namespace Application.Extensions.Mappers;

public static class CarMapper
{
    public static CarReadInfo ToRead(this Car car, IFileService fileService)
    {
        return new CarReadInfo(
            car.Id,
            car.Brand,
            car.Model,
            car.Year,
            car.CarStatus,
            car.CategoryId,
            car.LocationId,
            car.RentalCompanyId,
            car.CarImages
                .Select(ci => fileService.GetFileUrl(ci.ImagePath,MediaFolders.Images))
                .ToList(),
            car.CarPrices.OrderByDescending(x => x.CreatedAt).Select(cp => cp.PricePerDay)
                .FirstOrDefault(),
            car.CreatedAt
        );
    }

    public static CarDetailReadInfo ToReadDetail(this Car car, IFileService fileService)
    {


        return new CarDetailReadInfo(
            car.Id,
            car.Brand,
            car.Model,
            car.Year,
            car.RegistrationNumber,
            car.CarStatus,
            car.Category.Name,
            car.Location.Country + "," + car.Location.City,
            car.Owner.UserName,
            car.CarPrices
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => p.PricePerDay)
            .FirstOrDefault(),
            car.CarImages.Select(ci =>fileService.GetFileUrl(ci.ImagePath, MediaFolders.Images)).ToList(),
            car.CarDocuments.Select(x => fileService.GetFileUrl(x.FilePath,MediaFolders.Docs)).ToList(),
            car.CreatedAt
        );
    }


    public static Car ToEntity(this CarCreateInfo createInfo)
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