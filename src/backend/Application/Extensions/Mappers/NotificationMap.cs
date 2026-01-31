using Application.DTO_s;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class NotificationMap
{
    public static NotificationReadInfo ToRead(this Notification notification)
    {
        return new NotificationReadInfo(
            Id: notification.Id,
            Title: notification.Title,
            Message: notification.Message,
            IsRead: notification.IsRead,
            CreatedAt: notification.CreatedAt);
    }
}