using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Infrastructure.ImplementationContract.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class CarPriceService(ICarPriceRepository repository,ICarRepository carRepository) : ICarPriceService
{
    public async Task<Result<CarPriceReadInfo>> GetByCarIdAsync(int carId)
    {
        var res = repository.Find(x => x.CarId == carId);

        if (!res.IsSuccess)
            return Result<CarPriceReadInfo>.Failure(res.Error);

        var entity = await res.Value!.FirstOrDefaultAsync();

        if (entity is null)
            return Result<CarPriceReadInfo>.Failure(Error.NotFound("Price not found"));

        return Result<CarPriceReadInfo>.Success(entity.ToRead());
    }

    public async Task<BaseResult> CreateAsync(CarPriceCreateInfo createInfo,int currentUserId,bool isAdmin)
    {
        var carRes = await carRepository.GetByIdAsync(createInfo.CarId);

        if (!carRes.IsSuccess || carRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Car not found"));

        // Проверяем что цена ещё не создана
        var exists = await repository
            .Find(x => x.CarId == createInfo.CarId)
            .Value!
            .AnyAsync();

        if (exists)
            return BaseResult.Failure(Error.Conflict("Price already exists for this car"));
        var car = carRes.Value;

        var entity = createInfo.ToEntity();
        
        if (!isAdmin && car.OwnerId != currentUserId)
            return BaseResult.Failure(Error.Forbidden());

        var result = await repository.AddAsync(entity);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    
    public async Task<BaseResult> UpdateAsync(
        int id,
        CarPriceUpdateInfo updateInfo,
        int currentUserId,
        bool isAdmin)
    {
        var res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return BaseResult.Failure(
                Error.NotFound("Car price not found"));

        var entity = res.Value;

        var carRes = await carRepository.GetByIdAsync(entity.CarId);

        if (!carRes.IsSuccess || carRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Car not found"));

        var car = carRes.Value;

        // 🔐 Access check
        if (!isAdmin && car.OwnerId != currentUserId)
            return BaseResult.Failure(Error.Forbidden());

        entity = entity.ToEntity(updateInfo);

        var result = await repository.UpdateAsync(entity);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }


    public async Task<BaseResult> DeleteAsync(
        int id,
        int currentUserId,
        bool isAdmin)
    {
        var res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return BaseResult.Failure(Error.NotFound("Car price not found"));

        var entity = res.Value;

        var carRes = await carRepository.GetByIdAsync(entity.CarId);

        if (!carRes.IsSuccess || carRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Car not found"));

        var car = carRes.Value;

        // 🔐 Access check
        if (!isAdmin && car.OwnerId != currentUserId)
            return BaseResult.Failure(Error.Forbidden());

        var result = await repository.DeleteAsync(id);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
}