using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;
using Domain.Common;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class RoleService(IRoleRepository repository) : IRoleService
{
    public async Task<Result<PagedResponse<IEnumerable<RoleReadInfo>>>> GetAllAsync(RoleFilter filter)
    {
        var result = repository.Find(r =>
            string.IsNullOrEmpty(filter.Name) ||
            EF.Functions.ILike(r.Name, $"%{filter.Name}%"));

        if (!result.IsSuccess)
            return Result<PagedResponse<IEnumerable<RoleReadInfo>>>.Failure(result.Error);

        var query = result.Value!;

        int count = await query.CountAsync();

        var data = await query
            .OrderBy(r => r.Name)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(r => r.ToRead())
            .ToListAsync();

        var response = PagedResponse<IEnumerable<RoleReadInfo>>
            .Create(filter.PageNumber, filter.PageSize, count, data);

        return Result<PagedResponse<IEnumerable<RoleReadInfo>>>.Success(response);
    }

    public async Task<Result<RoleReadInfo>> GetByIdAsync(int id)
    {
        var res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return Result<RoleReadInfo>.Failure(Error.NotFound("Role not found"));

        return Result<RoleReadInfo>.Success(res.Value.ToRead());
    }

    public async Task<BaseResult> CreateAsync(RoleCreateInfo createInfo)
    {
        var existing = repository.Find(r => r.Name == createInfo.Name);

        if (existing.IsSuccess && await existing.Value!.AnyAsync())
            return BaseResult.Failure(Error.Conflict("Role already exists"));

        var result = await repository.AddAsync(createInfo.ToEntity());

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> UpdateAsync(int id, RoleUpdateInfo updateInfo)
    {
        var res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return BaseResult.Failure(Error.NotFound("Role not found"));

        var existing = repository.Find(r =>
            r.Name == updateInfo.Name &&
            r.Id != id);

        if (existing.IsSuccess && await existing.Value!.AnyAsync())
            return BaseResult.Failure(Error.Conflict("Role name already exists"));

        var updated = res.Value.ToEntity(updateInfo);

        var result = await repository.UpdateAsync(updated);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        var res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return BaseResult.Failure(Error.NotFound("Role not found"));

        // 🔥 Запрещаем удалять системные роли
        if (res.Value.Name == DefaultRoles.Admin ||
            res.Value.Name == DefaultRoles.User ||
            res.Value.Name == DefaultRoles.Owner)
        {
            return BaseResult.Failure(
                Error.BadRequest("System roles cannot be deleted"));
        }

        var result = await repository.DeleteAsync(id);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
}