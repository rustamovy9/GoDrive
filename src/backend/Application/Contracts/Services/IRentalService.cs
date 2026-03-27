using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;

namespace Application.Contracts.Services;

public interface IRentalCompanyService
{
    Task<Result<PagedResponse<IEnumerable<RentalCompanyReadInfo>>>> GetAllAsync(RentalCompanyFilter filter,int currentId,bool isAdmin);
    Task<Result<RentalCompanyReadInfo>> GetByIdAsync(int id,int currentId,bool isAdmin);
    Task<BaseResult> CreateAsync(RentalCompanyCreateInfo createInfo,int ownerId);
    Task<BaseResult> UpdateAsync(int id, RentalCompanyUpdateInfo updateInfo,int currentId,bool isAdmin);
    Task<BaseResult> DeleteAsync(int id,int currentId,bool isAdmin);
}
