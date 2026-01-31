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
using Infrastructure.Extensions;

namespace Infrastructure.ImplementationContract.Services;

public class RentalCompanyService(IRentalCompanyRepository repository) : IRentalCompanyService
{
    public async Task<Result<PagedResponse<IEnumerable<RentalCompanyReadInfo>>>> GetAllAsync(
        RentalCompanyFilter filter)
    {
        return await Task.Run(() =>
        {
            Expression<Func<RentalCompany, bool>> filterExpression = spec =>
                (string.IsNullOrEmpty(filter.Name) || spec.Name.ToLower().Contains(filter.Name.ToLower())) &&
                (string.IsNullOrEmpty(filter.ContactInfo) || spec.ContactInfo.ToLower().Contains(filter.ContactInfo.ToLower()));

            Result<IQueryable<RentalCompany>> request = repository.Find(filterExpression);

            if (!request.IsSuccess)
                return Result<PagedResponse<IEnumerable<RentalCompanyReadInfo>>>.Failure(request.Error);

            List<RentalCompanyReadInfo> query = request.Value!.Select(x => x.ToRead()).ToList();

            int count = query.Count;

            IEnumerable<RentalCompanyReadInfo> spec =
                query.Page(filter.PageNumber, filter.PageSize);

            PagedResponse<IEnumerable<RentalCompanyReadInfo>> res =
                PagedResponse<IEnumerable<RentalCompanyReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, spec);

            return Result<PagedResponse<IEnumerable<RentalCompanyReadInfo>>>.Success(res);
        });
    }

    public async Task<Result<RentalCompanyReadInfo>> GetByIdAsync(int id)
    {
        Result<RentalCompany?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return Result<RentalCompanyReadInfo>.Failure(res.Error);

        return Result<RentalCompanyReadInfo>.Success(res.Value!.ToRead());
    }

    public async Task<BaseResult> CreateAsync(RentalCompanyCreateInfo createInfo,int ownerId)
    {
        Result<int> res = await repository.AddAsync(createInfo.ToEntity(ownerId));

        return res.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(res.Error);
    }

    public async Task<BaseResult> UpdateAsync(int id, RentalCompanyUpdateInfo updateInfo)
    {
        Result<RentalCompany?> res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());

        Result<int> result = await repository.UpdateAsync(res.Value!.ToEntity(updateInfo));

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        Result<RentalCompany?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());

        Result<int> result = await repository.DeleteAsync(id);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
}
