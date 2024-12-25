using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Constants;
using Domain.Entities;


namespace Application.Extensions.Mappers;

public static class CarMapper
{
    public static CarReadInfo ToRead(this Car car)
    {
        return new CarReadInfo(
            car.Brand,
            car.Model,
            car.Year,
            car.Category,
            car.RegistrationNumber,
            car.Location,
            car.CarStatus,
            car.ImageCar,
            car.Id
        );
    }


    public static async Task<Car> ToEntity(this CarCreateInfo createInfo,IFileService fileService)
    {
        string? imagePath = FileData.Default;
        if (createInfo.File is not null)
            imagePath = await fileService.CreateFile(createInfo.File, MediaFolders.Images);
        return new Car
        {
            Brand = createInfo.Brand,
            Model = createInfo.Model,
            Year = createInfo.Year,
            Category = createInfo.Category,
            RegistrationNumber = createInfo.RegistrationNumber,
            Location = createInfo.Location,
            CarStatus = createInfo.CarStatus,
            ImageCar = imagePath
        };
    }

    public static async Task<Car> ToEntity(this Car entity, CarUpdateInfo updateInfo,IFileService fileService)
    {
        if (updateInfo.File is not null)
        { 
            fileService.DeleteFile(entity.ImageCar, MediaFolders.Images);
            
            entity.ImageCar = await fileService.CreateFile(updateInfo.File, MediaFolders.Images);
        }
        entity.Brand = updateInfo.Brand;
        entity.Model = updateInfo.Model;
        entity.Year = updateInfo.Year;
        entity.Category = updateInfo.Category;
        entity.RegistrationNumber = updateInfo.RegistrationNumber;
        entity.Location = updateInfo.Location;
        entity.CarStatus = updateInfo.CarStatus ;
        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        return entity;
    }
}