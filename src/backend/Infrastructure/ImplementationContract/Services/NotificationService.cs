using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class NotificationService(INotificationRepository repository,IRealtimeNotifier realtimeNotifier) 
    : INotificationService
{
    
    
    public async Task<Result<PagedResponse<IEnumerable<NotificationReadInfo>>>> 
        GetByUserIdAsync(int userId, BaseFilter filter)
    {
        var result = repository.Find(x =>
            x.UserId == userId &&
            (string.IsNullOrEmpty(filter.Search) ||
             EF.Functions.ILike(x.Title, $"%{filter.Search}%") ||
             EF.Functions.ILike(x.Message, $"%{filter.Search}%")));

        if (!result.IsSuccess)
            return Result<PagedResponse<IEnumerable<NotificationReadInfo>>>
                .Failure(result.Error);

        var query = result.Value!.AsNoTracking();

        int count = await query.CountAsync();

        var data = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x =>x.ToRead())
            .ToListAsync();

        var response = PagedResponse<IEnumerable<NotificationReadInfo>>
            .Create(filter.PageNumber, filter.PageSize, count, data);

        return Result<PagedResponse<IEnumerable<NotificationReadInfo>>>
            .Success(response);
    }

    public async Task<BaseResult> CreateAsync(NotificationCreateInfo createInfo)
    {
        var notification = createInfo.ToEntity();
        var result = await repository.AddAsync(notification);

        await realtimeNotifier.SendNotificationAsync(createInfo.UserId,createInfo.Title,createInfo.Message);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> MarkAsReadAsync(int notificationId)
    {
        var res = await repository.GetByIdAsync(notificationId);

        if (!res.IsSuccess || res.Value is null)
            return BaseResult.Failure(Error.NotFound("Notification not found"));

        var notification = res.Value;

        if (notification.IsRead)
            return BaseResult.Success(); // уже прочитано — норм

        notification.IsRead = true;
        notification.UpdatedAt = DateTimeOffset.UtcNow;
        notification.Version++;

        var update = await repository.UpdateAsync(notification);

        return update.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(update.Error);
    }

    public async Task<BaseResult> MarkAllAsReadAsync(int userId)
    {
        var result = repository.Find(x =>
            x.UserId == userId &&
            !x.IsRead);

        if (!result.IsSuccess)
            return BaseResult.Failure(result.Error);

        var notifications = await result.Value!.ToListAsync();

        if (!notifications.Any())
            return BaseResult.Success();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.UpdatedAt = DateTimeOffset.UtcNow;
            notification.Version++;
        }

        foreach (var notification in notifications)
        {
            var update = await repository.UpdateAsync(notification);
            if (!update.IsSuccess)
                return BaseResult.Failure(update.Error);
        }

        return BaseResult.Success();
    }

    public async Task<BaseResult> DeleteAsync(int notificationId)
    {
        var res = await repository.GetByIdAsync(notificationId);

        if (!res.IsSuccess || res.Value is null)
            return BaseResult.Failure(Error.NotFound());

        var delete = await repository.DeleteAsync(notificationId);

        return delete.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(delete.Error);
    }
}
