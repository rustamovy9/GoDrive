using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface IChatRepository
{
    Task SaveMessage(int userId, string role, string content);

    Task<List<ChatMessage>> GetLastMessages(int userId, int count);
}