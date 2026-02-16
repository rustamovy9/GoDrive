using Application.Contracts.Services;
using Application.DTO_s;
using Microsoft.AspNetCore.SignalR;
using MobileApp.Hubs;

namespace MobileApp.HelpersApi.SignalR;

public class SignalRNotifier (IHubContext<NotificationHub> hubContext):IRealtimeNotifier
{
    public async Task SendNotificationAsync(int userId,NotificationReadInfo readInfo)
    {
        await hubContext.Clients
            .Group($"user-{userId}")
            .SendAsync("ReceiveNotification",readInfo);
    }

    public async Task SendUnreadCountAsync(int userId, int count)
    {
        await hubContext.Clients
            .Group($"user-{userId}")
            .SendAsync("ReceiveUnreadCount", count);
    }

    public async Task SendBookingStatusAsync(int userId, int bookingId, string status)
    {
        await hubContext.Clients
            .Group($"user-{userId}")
            .SendAsync("ReceiveBookingStatus", new
            {
                BookingId = bookingId,
                Status = status
            });
    }
}