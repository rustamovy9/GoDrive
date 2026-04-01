using Application.Contracts.Services;
using Application.DTO_s;
using Application.Filters;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/cars")]
[Authorize]
public sealed class CarController(ICarService service) : BaseController
{
    private int UserId =>
        int.TryParse(User.FindFirst(CustomClaimTypes.Id)?.Value, out int id)
            ? id
            : 0;
        
        
    string Role =>
        User?.Identity?.IsAuthenticated == true
            ? User.IsInRole(DefaultRoles.Admin) ? DefaultRoles.Admin :
            User.IsInRole(DefaultRoles.Owner) ? DefaultRoles.Owner :
            DefaultRoles.User
            : DefaultRoles.User;

    private bool IsAdmin => User.IsInRole(DefaultRoles.Admin);


    [Authorize(Roles = DefaultRoles.Admin + "," + DefaultRoles.Owner)]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] CarFilter filter)
        => (await service.GetAllAsync(filter,Role,UserId)).ToActionResult();

    [AllowAnonymous]
    [HttpGet("public")]
    public async Task<IActionResult> GetPublic([FromQuery] CarFilter filter)
        => (await service.GetPublicAsync(filter)).ToActionResult();

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get([FromRoute]int id)
        => (await service.GetByIdAsync(id,UserId,IsAdmin)).ToActionResult();
    
    [HttpPost]
    [Authorize(Roles = DefaultRoles.Owner + "," + DefaultRoles.User)]
    public async Task<IActionResult> Create([FromBody] CarCreateInfo entity)
        => (await service.CreateAsync(entity,UserId)).ToActionResult();

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CarUpdateInfo entity)
        => (await service.UpdateAsync(id, entity,UserId,IsAdmin)).ToActionResult();

    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> Update(int id,
        [FromBody] CarUpdateStatusInfo statusInfo)
        => (await service.UpdateStatusAsync(id, statusInfo, IsAdmin)).ToActionResult(); 

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
        => (await service.DeleteAsync(id,UserId,IsAdmin)).ToActionResult();
}
