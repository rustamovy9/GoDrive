namespace Application.DTO_s;


public sealed record NotificationReadInfo(
    int Id,
    string Title,
    string Message,
    bool IsRead,
    DateTimeOffset CreatedAt);

public sealed record NotificationCreateInfo(
    int UserId,
    string Title,
    string Message);
