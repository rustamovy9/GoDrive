using System.Linq.Expressions;
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
using Infrastructure.Extensions;

namespace Infrastructure.ImplementationContract.Services;

public class CarService (ICarRepository repository,IFileService fileService) : ICarService
{
     public async Task<Result<PagedResponse<IEnumerable<CarReadInfo>>>> GetAllAsync(CarFilter filter)
    {
                Expression<Func<Car, bool>> filterExpression = car =>
                    (string.IsNullOrEmpty(filter.Brand) || car.Brand.ToLower().Contains(filter.Brand.ToLower())) &&
                    (string.IsNullOrEmpty(filter.Model) || car.Model.ToLower().Contains(filter.Model.ToLower())) &&
                    (filter.YearFrom == null || car.Year >= filter.YearFrom) &&
                    (filter.YearTo == null || car.Year <= filter.YearTo) &&
                    (filter.CategoryId == null || car.CategoryId ==  filter.CategoryId ) &&
                    (filter.LocationId == null || car.LocationId ==  filter.LocationId) &&
                    (string.IsNullOrEmpty(filter.RegistrationNumber) || car.RegistrationNumber.ToLower().Contains(filter.RegistrationNumber.ToLower()));

            Result<IQueryable<Car>> request = repository
                .Find(filterExpression);

            if (!request.IsSuccess)
                return Result<PagedResponse<IEnumerable<CarReadInfo>>>.Failure(request.Error);

            List<CarReadInfo> query = request.Value!.Select(x => x.ToRead()).ToList();

            int count = query.Count;

            IEnumerable<CarReadInfo> car =
                query.Page(filter.PageNumber, filter.PageSize);

            PagedResponse<IEnumerable<CarReadInfo>> res =
                PagedResponse<IEnumerable<CarReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, car);

            return Result<PagedResponse<IEnumerable<CarReadInfo>>>.Success(res);
    }

    public async Task<Result<CarReadInfo>> GetByIdAsync(int id)
    {
        Result<Car?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return Result<CarReadInfo>.Failure(res.Error);

        return Result<CarReadInfo>.Success(res.Value!.ToRead());
    }

    public async Task<BaseResult> CreateAsync(CarCreateInfo createInfo)
    {
        bool conflict = (await repository.GetAllAsync()).Value!.Any(car => car.RegistrationNumber == createInfo.RegistrationNumber);

        if (conflict) return BaseResult.Failure(Error.Conflict("Registration number already exists."));

        Result<int> res = await repository.AddAsync(createInfo.ToEntity());

        return res.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(res.Error);
    }

    public async Task<BaseResult> UpdateAsync(int id, CarUpdateInfo updateInfo)
    {
        Result<Car?> res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());
        
        Result<int> result = await repository.UpdateAsync(res.Value!.ToEntity(updateInfo));

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        Result<Car?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());
        
        Result<int> result = await repository.DeleteAsync(id);
        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
}