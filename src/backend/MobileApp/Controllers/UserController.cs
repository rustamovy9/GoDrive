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

    private bool IsAdmin => User.IsInRole(DefaultRoles.Admin);

    // -------------------- GET ALL --------------------
    [HttpGet]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> GetAll([FromQuery] UserFilter filter)
        => (await service.GetAllAsync(filter)).ToActionResult();

    // -------------------- GET BY ID --------------------
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!IsAdmin && id != CurrentUserId)
            return Forbid();

        return (await service.GetByIdAsync(id)).ToActionResult();
    }

    // -------------------- UPDATE --------------------
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateInfo entity)
    {
        if (!IsAdmin && id != CurrentUserId)
            return Forbid();

        return (await service.UpdateAsync(id, entity)).ToActionResult();
    }

    // -------------------- DELETE --------------------
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsAdmin && id != CurrentUserId)
            return Forbid();

        return (await service.DeleteAsync(id)).ToActionResult();
    }

    // -------------------- ASSIGN ROLE --------------------
    [HttpPost("{id:int}/assign-role")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> AssignRole(int id, [FromQuery] string roleName)
        => (await service.AssignRoleAsync(id, roleName)).ToActionResult();

    // -------------------- REMOVE ROLE --------------------
    [HttpDelete("{id:int}/remove-role")]
    [Authorize(Roles = DefaultRoles.Admin)]
    public async Task<IActionResult> RemoveRole(int id, [FromQuery] string roleName)
        => (await service.RemoveRoleAsync(id, roleName)).ToActionResult();
}
