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
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class UserService(IUserRepository repository, IFileService fileService,IRoleRepository roleRepository,IUserRoleRepository userRoleRepository) : IUserService
{
    public async Task<Result<PagedResponse<IEnumerable<UserReadInfo>>>> GetAllAsync(UserFilter filter)
    {
        Expression<Func<User, bool>> filterExpression = user =>
            (string.IsNullOrEmpty(filter.UserName) ||
             EF.Functions.ILike(user.UserName, $"%{filter.UserName}%")) &&
            (string.IsNullOrEmpty(filter.FirstName) ||
             EF.Functions.ILike(user.FirstName, $"%{filter.FirstName}%")) &&
            (string.IsNullOrEmpty(filter.LastName) ||
             EF.Functions.ILike(user.LastName, $"%{filter.LastName}%")) &&
            (filter.MinDateOfBirth == null || user.DateOfBirth >= filter.MinDateOfBirth) &&
            (filter.MaxDateOfBirth == null || user.DateOfBirth <= filter.MaxDateOfBirth) &&
            (string.IsNullOrEmpty(filter.Email) ||
             EF.Functions.ILike(user.Email, $"%{filter.Email}%")) &&
            (string.IsNullOrEmpty(filter.PhoneNumber) ||
             EF.Functions.ILike(user.PhoneNumber!, $"%{filter.PhoneNumber}%")) &&
            (string.IsNullOrEmpty(filter.Address) ||
             EF.Functions.ILike(user.Address!, $"%{filter.Address}%"));

        var result = repository.Find(filterExpression);

        if (!result.IsSuccess)
            return Result<PagedResponse<IEnumerable<UserReadInfo>>>.Failure(result.Error);

        var query = result.Value!.AsNoTracking();

        int count = await query.CountAsync();

        var data = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => x.ToRead())
            .ToListAsync();

        var response = PagedResponse<IEnumerable<UserReadInfo>>
            .Create(filter.PageNumber, filter.PageSize, count, data);

        return Result<PagedResponse<IEnumerable<UserReadInfo>>>.Success(response);
    }

    public async Task<Result<UserReadInfo>> GetByIdAsync(int id)
    {
        Result<User?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return Result<UserReadInfo>.Failure(res.Error);

        return Result<UserReadInfo>.Success(res.Value!.ToRead());
    }


    public async Task<BaseResult> UpdateAsync(int id, UserUpdateInfo updateInfo)
    {
        Result<User?> res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());

        if (!string.IsNullOrWhiteSpace(updateInfo.PhoneNumber))
        {
            var conflict = repository.Find(x =>
                x.Id != id &&
                x.PhoneNumber == updateInfo.PhoneNumber);

            if (conflict.IsSuccess && await conflict.Value!.AnyAsync())
                return BaseResult.Failure(Error.Conflict("Phone number already exists."));
        }


        Result<int> result = await repository.UpdateAsync(await res.Value!.ToEntity(updateInfo, fileService));

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        Result<User?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());

        if (res.Value!.AvatarPath != FileData.Default)
        {
            fileService.DeleteFile(res.Value.AvatarPath, MediaFolders.Images);
        }

        Result<int> result = await repository.DeleteAsync(id);
        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> AssignRoleAsync(int userId, string roleName)
    {
        var userRes = await repository.GetByIdAsync(userId);
        if (!userRes.IsSuccess || userRes.Value is null)
            return BaseResult.Failure(Error.NotFound("User not found"));

        var roleRes = roleRepository.Find(r => r.Name == roleName);
        if (!roleRes.IsSuccess)
            return BaseResult.Failure(roleRes.Error);

        var role = await roleRes.Value!.FirstOrDefaultAsync();
        if (role is null)
            return BaseResult.Failure(Error.NotFound("Role not found"));

        var existing = userRoleRepository.Find(x =>
            x.UserId == userId && x.RoleId == role.Id);

        if (existing.IsSuccess && await existing.Value!.AnyAsync())
            return BaseResult.Failure(Error.Conflict("User already has this role"));

        await userRoleRepository.AddAsync(new UserRole
        {
            UserId = userId,
            RoleId = role.Id
        });

        var user = userRes.Value;
        user.Version++;
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await repository.UpdateAsync(user);

        return BaseResult.Success();
    }

    public async Task<BaseResult> RemoveRoleAsync(int userId, string roleName)
    {
        var roleRes = roleRepository.Find(r => r.Name == roleName);
        if (!roleRes.IsSuccess)
            return BaseResult.Failure(roleRes.Error);

        var role = await roleRes.Value!.FirstOrDefaultAsync();
        if (role is null)
            return BaseResult.Failure(Error.NotFound("Role not found"));

        var userRoleRes = userRoleRepository.Find(x =>
            x.UserId == userId && x.RoleId == role.Id);

        if (!userRoleRes.IsSuccess)
            return BaseResult.Failure(userRoleRes.Error);

        var userRole = await userRoleRes.Value!.FirstOrDefaultAsync();
        if (userRole is null)
            return BaseResult.Failure(Error.NotFound("User does not have this role"));

        await userRoleRepository.DeleteAsync(userRole.Id);

        return BaseResult.Success();
    }
}