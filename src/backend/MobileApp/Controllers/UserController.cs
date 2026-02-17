using Application.Contracts.Services;
using Application.DTO_s;
using Application.Filters;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController(IUserService service) : BaseController
{
    private int CurrentUserId =>
        int.Parse(User.FindFirst(CustomClaimTypes.Id)?.Value
            ?? throw new UnauthorizedAccessException("UserId not found"));

    // -------------------- ADMIN --------------------
    [HttpGet]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> GetAll([FromQuery] UserFilter filter)
        => (await service.GetAllAsync(filter)).ToActionResult();
    
    [HttpGet("{id:int}")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> Get(int id)
    {
        return (await service.GetByIdAsync(id)).ToActionResult();
    }
    
    [HttpPut("{id:int}")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateInfo entity)
    {
        return (await service.UpdateAsync(id, entity)).ToActionResult();
    } 
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        return (await service.DeleteAsync(id)).ToActionResult();
    }

    // -------------------- USER --------------------
    [HttpGet("/me")]
    public async Task<IActionResult> GetMyProfile()
    {
       
        return (await service.GetByIdAsync(CurrentUserId)).ToActionResult();
    }
    
    [HttpPut("/me")]
    public async Task<IActionResult> UpdateMyProfile([FromForm] UserUpdateInfo entity)
    {
        return (await service.UpdateAsync(CurrentUserId, entity)).ToActionResult();
    }
    
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMyAccount()
        => (await service.DeleteAsync(CurrentUserId)).ToActionResult();

    // -------------------- ROLE MANAGEMENT --------------------
    [HttpPost("{id:int}/assign-role")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> AssignRole(int id, [FromQuery] string roleName)
        => (await service.AssignRoleAsync(id, roleName)).ToActionResult();
    
    [HttpDelete("{id:int}/remove-role")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> RemoveRole(int id, [FromQuery] string roleName)
        => (await service.RemoveRoleAsync(id, roleName)).ToActionResult();
}
