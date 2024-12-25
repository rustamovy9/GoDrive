
using Application.DTO_s;
using Application.Extensions.ResultPattern;

namespace Application.Contracts.Services;

public interface IAuthService
{
    Task<Result<Tuple<string,bool>>> LoginAsync(LoginRequest request);
    Task<BaseResult> RegisterAsync(RegisterRequest request);
    Task<BaseResult> DeleteAccountAsync(int userId);
    Task<BaseResult> ChangePasswordAsync(int userId,ChangePasswordRequest request);
}