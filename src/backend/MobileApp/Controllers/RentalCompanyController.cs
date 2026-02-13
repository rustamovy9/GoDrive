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
    private bool IsOwner => User.IsInRole(DefaultRoles.Owner);

    // -------------------- GET ALL --------------------

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] RentalCompanyFilter filter)
        => (await service.GetAllAsync(filter)).ToActionResult();

    // -------------------- GET BY ID --------------------

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    // -------------------- CREATE --------------------

    [HttpPost]
    [Authorize(Roles = DefaultRoles.Owner)]
    public async Task<IActionResult> Create([FromBody] RentalCompanyCreateInfo entity)
        => (await service.CreateAsync(entity, CurrentUserId)).ToActionResult();

    // -------------------- UPDATE --------------------

    [HttpPut("{id:int}")]
    [Authorize(Roles = DefaultRoles.Owner + "," + DefaultRoles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] RentalCompanyUpdateInfo entity)
        => (await service.UpdateAsync(id, entity)).ToActionResult();

    // -------------------- DELETE --------------------

    [HttpDelete("{id:int}")]
    [Authorize(Roles = DefaultRoles.Owner + "," + DefaultRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
        => (await service.DeleteAsync(id)).ToActionResult();
}