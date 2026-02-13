using System.Linq.Expressions;
using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class RentalCompanyService(IRentalCompanyRepository repository, ILocationRepository locationRepository)
    : IRentalCompanyService
{
    public async Task<Result<PagedResponse<IEnumerable<RentalCompanyReadInfo>>>> GetAllAsync(
        RentalCompanyFilter filter)
    {
        Expression<Func<RentalCompany, bool>> filterExpression = spec =>
            (string.IsNullOrEmpty(filter.Search) ||
             EF.Functions.ILike(spec.Name, $"%{filter.Search}%") &&
             (string.IsNullOrEmpty(filter.Name) ||
              EF.Functions.ILike(spec.Name, $"%{filter.Name}%")) &&
             (string.IsNullOrEmpty(filter.City) ||
              EF.Functions.ILike(spec.Location.City, $"%{filter.City}%")) &&
             (string.IsNullOrEmpty(filter.Country) ||
              EF.Functions.ILike(spec.Location.Country, $"%{filter.Country}%")) &&
             (string.IsNullOrEmpty(filter.ContactInfo) ||
              EF.Functions.ILike(spec.ContactInfo!, $"%{filter.ContactInfo}%")) &&
             (filter.OwnerId == null || spec.OwnerId == filter.OwnerId) &&
             (filter.LocationId == null || spec.LocationId == filter.LocationId) &&
             (filter.CreatedFrom == null || spec.CreatedAt >= filter.CreatedFrom) &&
             (filter.CreatedTo == null || spec.CreatedAt <= filter.CreatedTo) &&
             (filter.HasCars == null ||
              (filter.HasCars == true
                  ? spec.Cars.Any()
                  : !spec.Cars.Any()))
            );

        Result<IQueryable<RentalCompany>> request = repository.Find(filterExpression);

        if (!request.IsSuccess)
            return Result<PagedResponse<IEnumerable<RentalCompanyReadInfo>>>.Failure(request.Error);

        var query = request.Value!.Include(x => x.Location)
            .AsNoTracking();

        query = query.OrderByDescending(x => x.CreatedAt);

        int count = await query.CountAsync();

        var data = await query.Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => x.ToRead()).ToListAsync();

        PagedResponse<IEnumerable<RentalCompanyReadInfo>> res =
            PagedResponse<IEnumerable<RentalCompanyReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, data);

        return Result<PagedResponse<IEnumerable<RentalCompanyReadInfo>>>.Success(res);
    }

    public async Task<Result<RentalCompanyReadInfo>> GetByIdAsync(int id)
    {
        Result<RentalCompany?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess || res.Value is null) return Result<RentalCompanyReadInfo>.Failure(Error.NotFound("Rental company not found"));

        return Result<RentalCompanyReadInfo>.Success(res.Value.ToRead());
    }

    public async Task<BaseResult> CreateAsync(RentalCompanyCreateInfo createInfo, int ownerId)
    {
        var locationExists = await locationRepository
            .Find(x => x.Id == createInfo.LocationId)
            .Value!
            .AnyAsync();

        if (!locationExists)
            return BaseResult.Failure(Error.NotFound("Location not found"));

        var exists = repository.Find(x => x.OwnerId == ownerId);
        
        if(exists.IsSuccess && await exists.Value!.AnyAsync())
            return BaseResult.Failure(Error.Conflict("Owner already has a rental company"));
        var entity = createInfo.ToEntity(ownerId);

        var res = await repository.AddAsync(entity);

        return res.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(res.Error);
    }

    public async Task<BaseResult> UpdateAsync(int id, RentalCompanyUpdateInfo updateInfo)
    {
        Result<RentalCompany?> res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null) return BaseResult.Failure(Error.NotFound("Rental Company not found "));

        var company = res.Value;

        if (updateInfo.LocationId.HasValue)
        {
            var locationExists = await locationRepository
                .Find(x => x.Id == updateInfo.LocationId)
                .Value!
                .AnyAsync();

            if (!locationExists)
                return BaseResult.Failure(Error.NotFound("Location not found"));
        }

        company = company.ToEntity(updateInfo);
        
        Result<int> result = await repository.UpdateAsync(company);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        Result<RentalCompany?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess || res.Value is null) return BaseResult.Failure(Error.NotFound("Rental company not found"));

        var company = res.Value;

        if (company.Cars.Any())
            return BaseResult.Failure(Error.BadRequest("Cannot delete company with existing cars"));

        
        Result<int> result = await repository.DeleteAsync(id);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
}