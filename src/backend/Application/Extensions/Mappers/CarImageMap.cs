using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Constants;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class CarImageMap
{
    public static CarImageReadInfo ToRead(this CarImage image)
    {
        return new CarImageReadInfo(
            Id: image.Id,
            CarId: image.CarId,
            ImagePath: image.ImagePath,
            IsMain: image.IsMain);
    }

    public static async Task<CarImage> ToEntity(
        this CarImageCreateInfo createInfo,
        IFileService fileService)
    {
        var path = await fileService.CreateFile(
            createInfo.File,
            MediaFolders.Images);

        return new CarImage
        {
            CarId = createInfo.CarId,
            ImagePath = path,
            IsMain = createInfo.IsMain
        };
    }
}