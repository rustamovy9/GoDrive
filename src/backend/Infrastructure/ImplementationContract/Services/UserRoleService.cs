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
using Domain.Enums;
using Infrastructure.Extensions;

namespace Infrastructure.ImplementationContract.Services;

public class UserRoleService(IUserRoleRepository repository) : IUserRoleService
{
    public async Task<Result<PagedResponse<IEnumerable<UserRoleReadInfo>>>> GetAllAsync(UserRoleFilter filter)
    {
        return await Task.Run(() =>
        {
            Expression<Func<UserRole, bool>> filterExpression = spec =>
                (filter.RoleId == null || spec.RoleId == filter.RoleId) &&
                (filter.UserId == null || spec.UserId == filter.UserId);

            Result<IQueryable<UserRole>> request = repository
                .Find(filterExpression);

            if (!request.IsSuccess)
                return Result<PagedResponse<IEnumerable<UserRoleReadInfo>>>.Failure(request.Error);

            List<UserRoleReadInfo> query = request.Value!.Select(x => x.ToRead()).ToList();

            int count = query.Count;

            IEnumerable<UserRoleReadInfo> spec =
                query.Page(filter.PageNumber, filter.PageSize);

            PagedResponse<IEnumerable<UserRoleReadInfo>> res =
                PagedResponse<IEnumerable<UserRoleReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, spec);

            return Result<PagedResponse<IEnumerable<UserRoleReadInfo>>>.Success(res);
        });
    }

    public async Task<Result<UserRoleReadInfo?>> GetByIdAsync(int id)
    {
        Result<UserRole?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return Result<UserRoleReadInfo?>.Failure(res.Error);

        return Result<UserRoleReadInfo?>.Success(res.Value!.ToRead());
    }

    public async Task<BaseResult> CreateAsync(UserRoleCreateInfo createInfo)
    {
        Result<int> res = await repository.AddAsync(createInfo.ToEntity());

        return res.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(res.Error);
    }

    public async Task<BaseResult> UpdateAsync(int id, UserRoleUpdateInfo updateInfo)
    {
        Result<UserRole?> res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());
        
        Result<int> result = await repository.UpdateAsync(res.Value!.ToEntity(updateInfo));

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        Result<UserRole?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());
        
        
        Result<int> result = await repository.DeleteAsync(id);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
}