using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MobileApp.Controllers;

[Route("/api/auth/")]
public sealed class AuthController(IAuthService service) : BaseController
{
    private int GetUserIdFromClaims() =>
        int.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Id)?.Value 
                  ?? throw new UnauthorizedAccessException("User ID not found"));

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await service.LoginAsync(request);
        if (!result.IsSuccess || result.Value == null)
            return BadRequest(result.Error.Message ?? "Login failed");

        return Ok(new { Token = result.Value.Item1, IsAuthenticated = result.Value.Item2 });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await service.RegisterAsync(request);
        return result.IsSuccess ? Ok("User registered successfully.") : BadRequest(result.Error.Message ?? "User not registered.");
    }

    [Authorize(Roles = DefaultRoles.Admin + "," + DefaultRoles.User)]
    [HttpDelete]
    public async Task<IActionResult> DeleteSelfAsync()
    {
        int userId = GetUserIdFromClaims();
        var result = await service.DeleteAccountAsync(userId);
        return result.IsSuccess ? Ok("Account deleted successfully.") : BadRequest(result.Error.Message ?? "Account not deleted.");
    }

    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpDelete("/delete-user/{id:int}")]
    
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        var result = await service.DeleteAccountAsync(id);
        return result.IsSuccess ? Ok("User deleted successfully.") : BadRequest(result.Error.Message ?? "User not deleted.");
    }

    [Authorize(Roles = DefaultRoles.Admin + "," + DefaultRoles.User)]
    [Route("/change-password")]
    [HttpPut]
    public async Task<IActionResult> ChangeOwnPassword([FromBody] ChangePasswordRequest request)
    {
        int userId = GetUserIdFromClaims();
        var result = await service.ChangePasswordAsync(userId, request);
        return result.IsSuccess ? Ok("Password updated successfully.") : BadRequest(result.Error.Message ?? "Password not updated.");
    }

    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpPut("/change-password/{id:int}")]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequest request, [FromRoute] int id)
    {
        var result = await service.ChangePasswordAsync(id, request);
        return result.IsSuccess ? Ok("Password updated successfully.") : BadRequest(result.Error.Message ?? "Password not updated.");
    }
}