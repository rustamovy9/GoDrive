using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;

namespace Application.Contracts.Services;

public interface IUserService
{
    Task<Result<PagedResponse<IEnumerable<UserReadInfo>>>> GetAllAsync(UserFilter filter);
    Task<Result<UserReadInfo>> GetByIdAsync(int id,int currentUserId, bool isAdmin);
    Task<Result<UserReadInfo>> GetByIdAsync(int id);
    Task<BaseResult> UpdateAsync(int userId,UserUpdateInfo updateInfo,int currentUserId, bool isAdmin);
    Task<BaseResult> DeleteAsync(int userId,int currentUserId,bool isAdmin);
    Task<BaseResult> AssignRoleAsync(int userId, string roleName);
    Task<BaseResult> RemoveRoleAsync(int userId, string roleName);

}