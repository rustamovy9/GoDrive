using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;

namespace Application.Contracts.Services;

public interface IUserRoleService
{
    Task<Result<PagedResponse<IEnumerable<UserRoleReadInfo>>>> GetAllAsync(UserRoleFilter filter);
    Task<Result<UserRoleReadInfo?>> GetByIdAsync(int userRoleId);
    Task<BaseResult> CreateAsync(UserRoleCreateInfo userRoleCreateInfo);
    Task<BaseResult> UpdateAsync(int userRoleId, UserRoleUpdateInfo userRoleUpdateInfo);
    Task<BaseResult> DeleteAsync(int userRoleId);
}