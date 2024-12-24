using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;

namespace Application.Contracts.Services;

public interface IUserService
{
    Task<Result<PagedResponse<IEnumerable<UserReadInfo>>>> GetAllAsync(UserFilter filter);
    Task<Result<UserReadInfo>> GetByIdAsync(int id);
    Task<BaseResult> UpdateAsync(int id,UserUpdateInfo updateInfo);
    Task<BaseResult> DeleteAsync(int id);
}