using Application.Extensions.ResultPattern;
using Application.Filters;
using Application.Responces;

namespace Application.Contracts.Services;

public interface ISpecializationService
{
    Task<Result<PagedResponse<IEnumerable<SpecializationReadInfo>>>> GetAllAsync(SpecializationFilter filter);
    Task<Result<SpecializationReadInfo>> GetByIdAsync(Guid id);
    Task<BaseResult> CreateAsync(SpecializationCreateInfo createInfo);
    Task<BaseResult> UpdateAsync(Guid id,SpecializationUpdateInfo updateInfo);
    Task<BaseResult> DeleteAsync(Guid id);
}