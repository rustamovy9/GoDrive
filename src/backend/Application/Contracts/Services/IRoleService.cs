using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;

namespace Application.Contracts.Services;


public interface IRoleService
{
    Task<Result<PagedResponse<IEnumerable<RoleReadInfo>>>> GetAllAsync(RoleFilter filter);
    Task<Result<RoleReadInfo?>> GetByIdAsync(int roleId);
    Task<BaseResult> CreateAsync(RoleCreateInfo roleCreateInfo);
    Task<BaseResult> UpdateAsync(int roleId, RoleUpdateInfo roleUpdateInfo);
    Task<BaseResult> DeleteAsync(int roleId);
}
