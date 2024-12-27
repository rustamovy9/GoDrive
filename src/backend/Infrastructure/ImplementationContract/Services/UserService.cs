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

public class UserService(IUserRepository repository, IFileService fileService) : IUserService
{
    public async Task<Result<PagedResponse<IEnumerable<UserReadInfo>>>> GetAllAsync(UserFilter filter)
    {
        return await Task.Run(() =>
        {
            Expression<Func<User, bool>> filterExpression = user =>
                (string.IsNullOrEmpty(filter.UserName) ||
                 user.UserName.ToLower().Contains(filter.UserName.ToLower())) &&
                (string.IsNullOrEmpty(filter.FirstName) ||
                 user.FirstName.ToLower().Contains(filter.FirstName.ToLower())) &&
                (string.IsNullOrEmpty(filter.LastName) ||
                 user.LastName.ToLower().Contains(filter.LastName.ToLower())) &&
                (filter.MinDateOfBirth == null || user.DateOfBirth >= filter.MinDateOfBirth) &&
                (filter.MaxDateOfBirth == null || user.DateOfBirth <= filter.MaxDateOfBirth) &&
                (string.IsNullOrEmpty(filter.Email) || user.Email.ToLower().Contains(filter.Email.ToLower())) &&
                (string.IsNullOrEmpty(filter.PhoneNumber) ||
                 user.PhoneNumber!.ToLower().Contains(filter.PhoneNumber.ToLower())) &&
                (string.IsNullOrEmpty(filter.Address) || user.Address!.ToLower().Contains(filter.Address.ToLower()));

            Result<IQueryable<User>> request = repository
                .Find(filterExpression);

            if (!request.IsSuccess)
                return Result<PagedResponse<IEnumerable<UserReadInfo>>>.Failure(request.Error);

            List<UserReadInfo> query = request.Value!.Select(x => x.ToRead()).ToList();

            int count = query.Count;

            IEnumerable<UserReadInfo> user =
                query.Page(filter.PageNumber, filter.PageSize);

            PagedResponse<IEnumerable<UserReadInfo>> res =
                PagedResponse<IEnumerable<UserReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, user);

            return Result<PagedResponse<IEnumerable<UserReadInfo>>>.Success(res);
        });
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

        bool conflict = (await repository.GetAllAsync()).Value!.Any(user =>
            user.UserName.ToLower().Contains(updateInfo.UserName.ToLower())
            || user.PhoneNumber!.ToLower().Contains(updateInfo.PhoneNumber!)
            || user.Email.ToLower().Contains(updateInfo.Email));
        if (conflict) return BaseResult.Failure(Error.Conflict("phone or email or username already exists."));

        if (updateInfo.DateOfBirth > DateTime.UtcNow || updateInfo.DateOfBirth < DateTime.UtcNow.AddYears(-150))
            return BaseResult.Failure(Error.BadRequest("Invalid date of birth provided."));


        Result<int> result = await repository.UpdateAsync(await res.Value!.ToEntity(updateInfo, fileService));

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        Result<User?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());

        fileService.DeleteFile(res.Value!.AvatarPath, MediaFolders.Images);
        Result<int> result = await repository.DeleteAsync(id);
        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
}