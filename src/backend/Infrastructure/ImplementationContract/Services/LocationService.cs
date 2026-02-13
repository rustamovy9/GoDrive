using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class LocationService(ILocationRepository repository) : ILocationService
{
    public async Task<Result<PagedResponse<IEnumerable<LocationReadInfo>>>> 
        GetAllAsync(BaseFilter filter)
    {
        var result = repository.Find(x =>
            string.IsNullOrEmpty(filter.Search) ||
            EF.Functions.ILike(x.Country, $"%{filter.Search}%") ||
            EF.Functions.ILike(x.City, $"%{filter.Search}%"));

        if (!result.IsSuccess)
            return Result<PagedResponse<IEnumerable<LocationReadInfo>>>
                .Failure(result.Error);

        var query = result.Value!.AsNoTracking();

        int count = await query.CountAsync();

        var data = await query
            .OrderBy(x => x.Country)
            .ThenBy(x => x.City)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => x.ToRead())
            .ToListAsync();

        var response = PagedResponse<IEnumerable<LocationReadInfo>>
            .Create(filter.PageNumber, filter.PageSize, count, data);

        return Result<PagedResponse<IEnumerable<LocationReadInfo>>>
            .Success(response);
    }

    public async Task<Result<LocationReadInfo>> GetByIdAsync(int id)
    {
        var res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return Result<LocationReadInfo>.Failure(Error.NotFound());

        var location = res.Value;

        return Result<LocationReadInfo>.Success(location.ToRead());
    }

    public async Task<BaseResult> CreateAsync(LocationCreateInfo createInfo)
    {
        var conflict = repository.Find(x =>
            x.Country.ToLower() == createInfo.Country.ToLower() &&
            x.City.ToLower() == createInfo.City.ToLower());

        if (conflict.IsSuccess && await conflict.Value!.AnyAsync())
            return BaseResult.Failure(
                Error.Conflict("Location already exists."));

        var location = createInfo.ToEntity();

        var result = await repository.AddAsync(location);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> UpdateAsync(int id, LocationUpdateInfo updateInfo)
    {
        var res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return BaseResult.Failure(Error.NotFound());

        var location = res.Value.ToEntity(updateInfo);

        var result = await repository.UpdateAsync(location);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        var res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return BaseResult.Failure(Error.NotFound());

        var result = await repository.DeleteAsync(id);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
}
