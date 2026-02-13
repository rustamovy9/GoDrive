using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Domain.Entities;

namespace Application.Contracts.Services;

public interface INotificationService
{
    Task<Result<PagedResponse<IEnumerable<NotificationReadInfo>>>> GetByUserIdAsync(int userId, BaseFilter filter);
    Task<BaseResult> CreateAsync(NotificationCreateInfo createInfo);    
    Task<BaseResult> MarkAsReadAsync(int notificationId);
    Task<BaseResult> MarkAllAsReadAsync(int userId);
    Task<BaseResult> DeleteAsync(int notificationId);
}