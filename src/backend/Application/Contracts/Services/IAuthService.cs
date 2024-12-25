
using Application.DTO_s;

namespace Application.Contracts.Services;

public interface IAuthService
{
    Task<Tuple<string,bool>> LoginAsync(LoginRequest request);
    Task<bool> RegisterAsync(RegisterRequest request);
    Task<bool> DeleteAccountAsync(int userId);
    Task<bool> ChangePasswordAsync(int userId,ChangePasswordRequest request);
}