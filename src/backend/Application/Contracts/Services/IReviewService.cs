using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;

namespace Application.Contracts.Services;

public interface IReviewService
{
    Task<Result<PagedResponse<IEnumerable<ReviewReadInfo>>>> GetAllAsync(ReviewFilter filter);

    Task<Result<ReviewReadInfo>> GetByIdAsync(int id);

    Task<BaseResult> CreateAsync(int userId,ReviewCreateInfo createInfo);

    Task<BaseResult> UpdateAsync(int id, ReviewUpdateInfo updateInfo);

    Task<BaseResult> DeleteAsync(int id);
}