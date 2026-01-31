using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;

namespace Application.Contracts.Services;

public interface IRentalCompanyService
{
    Task<Result<PagedResponse<IEnumerable<RentalCompanyReadInfo>>>> GetAllAsync(RentalCompanyFilter filter);
    Task<Result<RentalCompanyReadInfo>> GetByIdAsync(int id);
    Task<BaseResult> CreateAsync(RentalCompanyCreateInfo createInfo,int ownerId);
    Task<BaseResult> UpdateAsync(int id, RentalCompanyUpdateInfo updateInfo);
    Task<BaseResult> DeleteAsync(int id);
}
