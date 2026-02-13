namespace Application.Contracts.Services;

public interface IRealtimeNotifier
{
    Task SendNotificationAsync(int userId, string title, string message);
}