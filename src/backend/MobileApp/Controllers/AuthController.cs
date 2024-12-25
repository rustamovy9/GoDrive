using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MobileApp.Controllers;

[Route("/api/auth/")]
public sealed class AuthController(IAuthService service) : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        Tuple<string, bool> res = await service.LoginAsync(request);
        if (!res.Item2) BadRequest(res.Item1);
        return Ok(res.Item1);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        bool res = await service.RegisterAsync(request);
        return res ? Ok() : BadRequest("User not registered!");
    }

    [Authorize(Roles = DefaultRoles.Admin+","+DefaultRoles.User)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync()
    {
        int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Id)!.Value);
        bool res = await service.DeleteAccountAsync(userId);
        return res ? Ok() : BadRequest("User not deleted!");
    }

    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        int deletedBy = int.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Id)!.Value);
        bool res = await service.DeleteAccountAsync(id);
        return res ? Ok() : BadRequest("User not deleted!");
    }

    [Authorize(Roles = DefaultRoles.Admin+","+DefaultRoles.User)]
    [HttpPut]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Id)!.Value);
        bool changed = await service.ChangePasswordAsync(userId, request);
        return changed ? Ok() : BadRequest("User password not updated!");
    }

    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, [FromRoute] int id)
    {
        int updatedBy = int.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Id)!.Value);
        bool changed = await service.ChangePasswordAsync(id, request);
        return changed ? Ok() : BadRequest("User password not updated!");
    }
}