

using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;
using Domain.Enums;

namespace Application.Contracts.Services;

public interface IBookingService
{
    Task<Result<PagedResponse<IEnumerable<BookingReadInfo>>>> GetAllAsync(BookingFilter filter,int currentUserId,bool isAdmin);
    Task<Result<BookingReadInfo>> GetByIdAsync(int id,int currentUserId,bool isAdmin);
    Task<BaseResult> CreateAsync(BookingCreateInfo createInfo,int userId);
    Task<BaseResult> UpdateAsync(int id,BookingUpdateInfo updateInfo,int currentUserId);
    Task<BaseResult> UpdateStatusAsync(int id, BookingUpdateStatusInfo updateStatusInfo,int currentUserId,bool isAdmin);
    Task<BaseResult> DeleteAsync(int id,int currentUserId,bool isAdmin);
}   