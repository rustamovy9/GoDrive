using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;
using Domain.Common;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class CarService(ICarRepository repository, INotificationService notificationService) : ICarService
{
    public async Task<Result<PagedResponse<IEnumerable<CarReadInfo>>>> GetAllAsync(CarFilter filter,string role ,int userId)
    {
        var result = repository.Find(car =>
            (string.IsNullOrEmpty(filter.Search) ||
             EF.Functions.ILike(car.Brand, $"%{filter.Search}%") ||
             EF.Functions.ILike(car.Model, $"%{filter.Search}%") ||
             EF.Functions.ILike(car.RegistrationNumber, $"%{filter.Search}%")) &&
            (string.IsNullOrEmpty(filter.Brand) ||
             EF.Functions.ILike(car.Brand, $"%{filter.Brand}%")) &&
            (string.IsNullOrEmpty(filter.Model) ||
             EF.Functions.ILike(car.Model, $"%{filter.Model}%")) &&
            (filter.YearFrom == null || car.Year >= filter.YearFrom) &&
            (filter.YearTo == null || car.Year <= filter.YearTo) &&
            (filter.CategoryId == null || car.CategoryId == filter.CategoryId) &&
            (filter.LocationId == null || car.LocationId == filter.LocationId) &&
            (filter.OwnerId == null || car.OwnerId == filter.OwnerId) &&
            (filter.Status == null || car.CarStatus == filter.Status) &&
            (string.IsNullOrEmpty(filter.RegistrationNumber) ||
             EF.Functions.ILike(car.RegistrationNumber, $"%{filter.RegistrationNumber}%"))
        );

        if (!result.IsSuccess)
            return Result<PagedResponse<IEnumerable<CarReadInfo>>>.Failure(result.Error);

        var query = result.Value!.AsNoTracking();
        
        if (role == DefaultRoles.User)
        {
            query = query.Where(c => c.CarStatus == CarStatus.Available);
        }
        else if (role == DefaultRoles.Owner)
        {
            query = query.Where(c => c.OwnerId == userId);
        }

        if (filter.MinPrice != null)
        {
            query = query.Where(c =>
                c.CarPrices
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => p.PricePerDay)
                    .FirstOrDefault() >= filter.MinPrice);
        }

        if (filter.MaxPrice != null)
        {
            query = query.Where(c =>
                c.CarPrices
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => p.PricePerDay)
                    .FirstOrDefault() <= filter.MaxPrice);
        }


        int count = await query.CountAsync();

        var data = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(c => c.ToRead())
            .ToListAsync();

        var response = PagedResponse<IEnumerable<CarReadInfo>>
            .Create(filter.PageSize, filter.PageNumber, count, data);

        return Result<PagedResponse<IEnumerable<CarReadInfo>>>.Success(response);
    }

    public async Task<Result<CarReadInfo>> GetByIdAsync(int id)
    {
        Result<Car?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return Result<CarReadInfo>.Failure(res.Error);

        return Result<CarReadInfo>.Success(res.Value!.ToRead());
    }

    public async Task<BaseResult> CreateAsync(CarCreateInfo createInfo,int ownerId)
    {
        Result<IQueryable<Car>> conflict = repository.Find(x => x.RegistrationNumber == createInfo.RegistrationNumber);

        if (conflict.IsSuccess && await conflict.Value!.AnyAsync())
            return BaseResult.Failure(Error.Conflict("Registration number already exists."));

        Car car = createInfo.ToEntity();

        car.CarStatus = CarStatus.Blocked;
        car.OwnerId = ownerId;

        Result<int> res = await repository.AddAsync(car);

        if (!res.IsSuccess)
            return BaseResult.Failure(res.Error);

        await notificationService.CreateAsync(
            new NotificationCreateInfo(
                car.OwnerId,
                "Car created",
                "Your car has been created and is waiting for document verification."));

        return BaseResult.Success();
    }

    public async Task<BaseResult> UpdateAsync(int id, CarUpdateInfo updateInfo,int currentUserId,bool isAdmin)
    {
        Result<Car?> res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null) return BaseResult.Failure(Error.NotFound("Car not found"));
        
        var car = res.Value;

        if (!isAdmin && car.OwnerId != currentUserId)
            return BaseResult.Failure(Error.Forbidden());

        if (car.CarStatus == CarStatus.Blocked)
            return BaseResult.Failure(
                Error.BadRequest("Archived car cannot be updated"));

        Result<int> result = await repository.UpdateAsync(res.Value!.ToEntity(updateInfo));

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id,int currentUserId,bool isAdmin)
    {
        Result<Car?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound("Car not found"));
        
        var car = res.Value;

        if (!isAdmin && car.OwnerId != currentUserId)
            return BaseResult.Failure(Error.Forbidden());

        if (!isAdmin && car.CarStatus != CarStatus.Blocked)
            return BaseResult.Failure(
                Error.BadRequest("Only blocked car can be deleted"));

        Result<int> result = await repository.DeleteAsync(id);

        if (!result.IsSuccess)
            return BaseResult.Failure(result.Error);

        await notificationService.CreateAsync(
            new NotificationCreateInfo(
                res.Value!.OwnerId,
                "Car deleted",
                "Your car has been deleted from the system."));

        return BaseResult.Success();
    }

    public async Task<BaseResult> UpdateStatusAsync(int id, CarUpdateStatusInfo status,bool isAdmin)
    {
        if (!isAdmin)
            return BaseResult.Failure(Error.Forbidden());
        
        Result<Car?> res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return BaseResult.Failure(Error.NotFound("Car not found"));

        Car car = res.Value;
        
        if (!isAdmin)
            return BaseResult.Failure(Error.Forbidden());

        car.CarStatus = status.Status;
        car.UpdatedAt = DateTimeOffset.UtcNow;
        car.Version++;

        Result<int> update = await repository.UpdateAsync(car);

        if (!update.IsSuccess)
            return BaseResult.Failure(update.Error);

        await notificationService.CreateAsync(
            new NotificationCreateInfo(
                car.OwnerId,
                "Car status updated",
                $"Your car status changed to {status}."));

        return BaseResult.Success();
    }
}