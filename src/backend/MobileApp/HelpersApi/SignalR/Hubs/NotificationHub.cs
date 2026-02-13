using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MobileApp.Hubs;

[Authorize]
public class NotificationHub : Hub
{

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(CustomClaimTypes.Id)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,
                $"user-{userId}");
        }

        await base.OnConnectedAsync();
    }
    
}