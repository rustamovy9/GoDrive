using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;

namespace Application.Contracts.Services;

public interface ICarService
{
    Task<Result<PagedResponse<IEnumerable<CarReadInfo>>>> GetAllAsync(CarFilter filter);
    Task<Result<CarReadInfo>> GetByIdAsync(int id);
    Task<BaseResult> CreateAsync(CarCreateInfo createInfo);
    Task<BaseResult> UpdateAsync(int id,CarUpdateInfo updateInfo);
    Task<BaseResult> DeleteAsync(int id);
}