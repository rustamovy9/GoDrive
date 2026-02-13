using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Domain.Common;

namespace Application.Contracts.Services;

public interface ICategoryService
{
    Task<BaseResult> CreateAsync(CategoryCreateInfo createInfo);

    Task<BaseResult> UpdateAsync(int id,CategoryUpdateInfo updateInfo);
    Task<BaseResult> DeleteAsync(int id);
    Task<Result<CategoryReadInfo>> GetByIdAsync(int id);
    Task<Result<PagedResponse<IEnumerable<CategoryReadInfo>>>> GetAll(BaseFilter filter);
}