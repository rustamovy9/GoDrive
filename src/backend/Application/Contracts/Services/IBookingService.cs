

using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;

namespace Application.Contracts.Services;

public interface IBookingService
{
    Task<Result<PagedResponse<IEnumerable<BookingReadInfo>>>> GetAllAsync(BookingFilter filter);
    Task<Result<BookingReadInfo>> GetByIdAsync(int id);
    Task<BaseResult> CreateAsync(BookingCreateInfo createInfo,int userId);
    Task<BaseResult> UpdateAsync(int id,BookingUpdateInfo updateInfo);
    Task<BaseResult> DeleteAsync(int id);
}   