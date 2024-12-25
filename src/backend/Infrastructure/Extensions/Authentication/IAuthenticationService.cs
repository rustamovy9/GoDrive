using Domain.Entities;

namespace Infrastructure.Extensions.Authentication;

public interface IAuthenticationService
{
    Task<string> GenerateTokenAsync(User user);
}