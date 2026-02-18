using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Domain.Constants;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class CarImageService(ICarImageRepository repository, ICarRepository carRepository, IFileService fileService)
    : ICarImageService
{
    public async Task<Result<IEnumerable<CarImageReadInfo>>>
        GetByCarIdAsync(int carId, int currentUserId, bool isAdmin)
    {
        // 1️⃣ Получаем машину
        var carRes = await carRepository.GetByIdAsync(carId);

        if (!carRes.IsSuccess || carRes.Value is null)
            return Result<IEnumerable<CarImageReadInfo>>
                .Failure(Error.NotFound("Car not found"));

        var car = carRes.Value;

        // 2️⃣ Проверка доступа
        bool isOwner = car.OwnerId == currentUserId;
        bool isPublic = car.CarStatus == CarStatus.Available;

        if (!isAdmin && !isOwner && !isPublic)
            return Result<IEnumerable<CarImageReadInfo>>
                .Failure(Error.Forbidden());

        // 3️⃣ Получаем изображения
        var imagesRes = repository.Find(x => x.CarId == carId);

        if (!imagesRes.IsSuccess)
            return Result<IEnumerable<CarImageReadInfo>>
                .Failure(imagesRes.Error);

        var data = await imagesRes.Value!
            .OrderByDescending(x => x.IsMain)
            .Select(x => x.ToRead())
            .ToListAsync();

        return Result<IEnumerable<CarImageReadInfo>>
            .Success(data);
    }


    public async Task<BaseResult> CreateAsync(CarImageCreateInfo createInfo, int currentUserId)
    {
        var carRes = await carRepository.GetByIdAsync(createInfo.CarId);

        if (!carRes.IsSuccess || carRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Car not found"));

        if (carRes.Value.CarStatus == CarStatus.Blocked)
            return BaseResult.Failure(Error.BadRequest("Cannot upload image for blocked car"));

        if (carRes.Value.OwnerId != currentUserId)
            return BaseResult.Failure(Error.Forbidden());

        var imagesQuery = repository.Find(x => x.CarId == createInfo.CarId);

        if (!imagesQuery.IsSuccess)
            return BaseResult.Failure(imagesQuery.Error);

        var images = await imagesQuery.Value!.ToListAsync();

        if (images.Count >= 10)
            return BaseResult.Failure(Error.BadRequest("Maximum 10 images allowed"));

        var image = await createInfo.ToEntity(fileService);

        // Если это первое фото → делаем main
        if (!images.Any())
        {
            image.IsMain = true;
        }
        else if (createInfo.IsMain)
        {
            // снимаем старое главное
            foreach (var img in images.Where(x => x.IsMain))
            {
                img.IsMain = false;
            }

            await repository.UpdateRangeAsync(images);

            image.IsMain = true;
        }

        var result = await repository.AddAsync(image);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> SetMainAsync(int imageId, int currentUserId)
    {
        var imageRes = await repository.GetByIdAsync(imageId);

        if (!imageRes.IsSuccess || imageRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Image not found"));

        var carRes = await carRepository.GetByIdAsync(imageRes.Value.CarId);

        if (!carRes.IsSuccess || carRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Car not found"));

        if (carRes.Value.OwnerId != currentUserId)
            return BaseResult.Failure(Error.Forbidden());

        var images = await repository
            .Find(x => x.CarId == imageRes.Value.CarId)
            .Value!
            .ToListAsync();

        foreach (var img in images)
        {
            img.IsMain = img.Id == imageId;
        }

        await repository.UpdateRangeAsync(images);

        return BaseResult.Success();
    }

    public async Task<BaseResult> DeleteAsync(int id, int currentUserId, bool isAdmin)
    {
        var imageRes = await repository.GetByIdAsync(id);

        if (!imageRes.IsSuccess || imageRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Image not found"));

        var carRes = await carRepository.GetByIdAsync(imageRes.Value.CarId);

        if (!carRes.IsSuccess || carRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Car not found"));

        if (!isAdmin && carRes.Value.OwnerId != currentUserId)
            return BaseResult.Failure(Error.Forbidden());

        bool wasMain = imageRes.Value.IsMain;

        fileService.DeleteFile(imageRes.Value.ImagePath, MediaFolders.Images);

        var deleteResult = await repository.DeleteAsync(id);

        if (!deleteResult.IsSuccess)
            return BaseResult.Failure(deleteResult.Error);

        // если удалили главное фото → назначим новое
        if (wasMain)
        {
            var otherRes = repository.Find(x => x.CarId == imageRes.Value.CarId);

            if (otherRes.IsSuccess)
            {
                var other = await otherRes.Value!.FirstOrDefaultAsync();

                if (other != null)
                {
                    other.IsMain = true;
                    await repository.UpdateAsync(other);
                }
            }
        }

        return BaseResult.Success();
    }

    public async Task<Result<(byte[] FileBytes, string FileName)>> DownloadAsync(
        int id,
        int currentUserId,
        bool isAdmin)
    {
        var carImageRes = await repository.GetByIdAsync(id);

        if (!carImageRes.IsSuccess || carImageRes.Value is null)
            return Result<(byte[], string)>.Failure(Error.NotFound("Image not found"));

        var carImage = carImageRes.Value;

        var carRes = await carRepository.GetByIdAsync(carImage.CarId);

        if (!carRes.IsSuccess || carRes.Value is null)
            return Result<(byte[], string)>.Failure(Error.NotFound("Car not found"));

        var car = carRes.Value;

        bool isOwner = car.OwnerId == currentUserId;
        bool isPublic = car.CarStatus == CarStatus.Available;

        if (!isAdmin && !isOwner && !isPublic)
            return Result<(byte[], string)>.Failure(Error.Forbidden());

        if (!fileService.FileExists(carImage.ImagePath))
            return Result<(byte[], string)>.Failure(Error.NotFound("File not found"));

        var file = await fileService.GetFileAsync(carImage.ImagePath,MediaFolders.Images);

        return Result<(byte[], string)>.Success(file);
    }
}