using Application.Contracts.Services;
using Application.DTO_s;
using Application.Filters;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/rental-companies")]
[Authorize]
public sealed class RentalCompanyController(IRentalCompanyService service) : BaseController
{
    private int CurrentUserId =>
        int.Parse(User.FindFirst(CustomClaimTypes.Id)?.Value
                  ?? throw new UnauthorizedAccessException("UserId not found"));

    private bool IsAdmin => User.IsInRole(DefaultRoles.Admin);

    // -------------------- GET ALL --------------------

    [HttpGet]
    [Authorize(Roles = DefaultRoles.Admin + "," + DefaultRoles.Owner)]
    public async Task<IActionResult> Get([FromQuery] RentalCompanyFilter filter)
        => (await service.GetAllAsync(filter,CurrentUserId,IsAdmin)).ToActionResult();

    // -------------------- GET BY ID --------------------

    [HttpGet("{id:int}")]
    [Authorize(Roles = DefaultRoles.Admin + "," + DefaultRoles.Owner)]
    public async Task<IActionResult> Get(int id)
        => (await service.GetByIdAsync(id,CurrentUserId,IsAdmin)).ToActionResult();

    // -------------------- CREATE --------------------

    [HttpPost]
    [Authorize(Roles = DefaultRoles.Owner)]
    public async Task<IActionResult> Create([FromBody] RentalCompanyCreateInfo entity)
        => (await service.CreateAsync(entity, CurrentUserId)).ToActionResult();

    // -------------------- UPDATE --------------------

    [HttpPut("{id:int}")]
    [Authorize(Roles = DefaultRoles.Owner + "," + DefaultRoles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] RentalCompanyUpdateInfo entity)
        => (await service.UpdateAsync(id, entity,CurrentUserId,IsAdmin)).ToActionResult();

    // -------------------- DELETE --------------------

    [HttpDelete("{id:int}")]
    [Authorize(Roles = DefaultRoles.Owner + "," + DefaultRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
        => (await service.DeleteAsync(id,CurrentUserId,IsAdmin)).ToActionResult();
}