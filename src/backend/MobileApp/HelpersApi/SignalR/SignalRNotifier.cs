using Application.Contracts.Services;
using Microsoft.AspNetCore.SignalR;
using MobileApp.Hubs;

namespace MobileApp.HelpersApi.SignalR;

public class SignalRNotifier (IHubContext<NotificationHub> hubContext):IRealtimeNotifier
{
    public async Task SendNotificationAsync(int userId, string title, string message)
    {
        await hubContext.Clients
            .Group($"user-{userId}")
            .SendAsync("ReceiveNotification", new
            {
                Title = title,
                Message = message,
                CreatedAt = DateTimeOffset.UtcNow
            });
    }
}