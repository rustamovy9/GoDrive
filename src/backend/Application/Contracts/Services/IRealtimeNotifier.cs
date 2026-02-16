using Application.DTO_s;

namespace Application.Contracts.Services;

public interface IRealtimeNotifier
{
    Task SendNotificationAsync(int userId, NotificationReadInfo notification);

    Task SendUnreadCountAsync(int userId, int count);

    Task SendBookingStatusAsync(int userId, int bookingId, string status);
}