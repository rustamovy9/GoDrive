using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;

namespace Application.Contracts.Services;

public interface ICategoryService
{
    Task<Result<PagedResponse<IEnumerable<CategoryReadInfo>>>> GetAll();
}