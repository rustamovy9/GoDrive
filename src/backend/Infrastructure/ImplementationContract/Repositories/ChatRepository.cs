using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly DataContext _context;

    public ChatRepository(DataContext context)
    {
        _context = context;
    }

    public async Task SaveMessage(int userId, string role, string content)
    {
        var msg = new ChatMessage
        {
            UserId = userId,
            Role = role,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        _context.ChatMessages.Add(msg);

        await _context.SaveChangesAsync();
    }

    public async Task<List<ChatMessage>> GetLastMessages(int userId, int count)
    {
        return await _context.ChatMessages
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(count)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
    }
}