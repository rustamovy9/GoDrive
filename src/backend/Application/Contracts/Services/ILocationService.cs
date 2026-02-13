using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Domain.Common;

namespace Application.Contracts.Services;

public interface ILocationService
{
    Task<Result<PagedResponse<IEnumerable<LocationReadInfo>>>> GetAllAsync(BaseFilter filter);

    Task<Result<LocationReadInfo>> GetByIdAsync(int id);

    Task<BaseResult> CreateAsync(LocationCreateInfo createInfo);

    Task<BaseResult> UpdateAsync(int id, LocationUpdateInfo updateInfo);

    Task<BaseResult> DeleteAsync(int id);
}