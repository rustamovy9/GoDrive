using Application.Contracts.Services;
using Application.DTO_s;
using Application.Filters;
using Domain.Constants;
using Infrastructure.ImplementationContract.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;


[ApiController]
[Route("api/roles")]
[Authorize(Roles = DefaultRoles.Admin)]
public class RoleController (IRoleService service) : BaseController
{
    [HttpGet] public async Task<IActionResult> Get([FromQuery] RoleFilter filter)
        => (await service.GetAllAsync(filter)).ToActionResult();

    [HttpGet("{id:int}")] public async Task<IActionResult> Get([FromRoute] int id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    [HttpPost] public async Task<IActionResult> Create([FromBody] RoleCreateInfo entity)
        => (await service.CreateAsync(entity)).ToActionResult();

    [HttpPut("{id:int}")] public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RoleUpdateInfo entity)
        => (await service.UpdateAsync(id, entity)).ToActionResult();

    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete([FromRoute] int id)
        => (await service.DeleteAsync(id)).ToActionResult();
}