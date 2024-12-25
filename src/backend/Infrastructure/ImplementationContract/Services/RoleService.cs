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

public class RoleService(IRoleRepository repository) : IRoleService
{
    public async Task<Result<PagedResponse<IEnumerable<RoleReadInfo>>>> GetAllAsync(RoleFilter filter)
    {
        return await Task.Run(() =>
        {
            Expression<Func<Role, bool>> filterExpression = role =>
                (string.IsNullOrEmpty(filter.Name) || role.Name.ToLower().Contains(filter.Name.ToLower()));

            Result<IQueryable<Role>> request = repository
                .Find(filterExpression);

            if (!request.IsSuccess)
                return Result<PagedResponse<IEnumerable<RoleReadInfo>>>.Failure(request.Error);

            List<RoleReadInfo> query = request.Value!.Select(x => x.ToRead()).ToList();

            int count = query.Count;

            IEnumerable<RoleReadInfo> role =
                query.Page(filter.PageNumber, filter.PageSize);

            PagedResponse<IEnumerable<RoleReadInfo>> res =
                PagedResponse<IEnumerable<RoleReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, role);

            return Result<PagedResponse<IEnumerable<RoleReadInfo>>>.Success(res);
        });
    }

    public async Task<Result<RoleReadInfo?>> GetByIdAsync(int id)
    {
        Result<Role?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return Result<RoleReadInfo?>.Failure(res.Error);

        return Result<RoleReadInfo?>.Success(res.Value!.ToRead());
    }

    public async Task<BaseResult> CreateAsync(RoleCreateInfo createInfo)
    {
        Result<int> res = await repository.AddAsync(createInfo.ToEntity());

        return res.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(res.Error);
    }

    public async Task<BaseResult> UpdateAsync(int id, RoleUpdateInfo updateInfo)
    {
        Result<Role?> res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());
        
        Result<int> result = await repository.UpdateAsync(res.Value!.ToEntity(updateInfo));

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        Result<Role?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());
        
        
        Result<int> result = await repository.DeleteAsync(id);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
}