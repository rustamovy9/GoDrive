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
[Authorize(Roles = DefaultRoles.Admin)]
public class UserController (IUserService service) : BaseController
{
    [HttpGet] public async Task<IActionResult> Get([FromQuery] UserFilter filter)
        => (await service.GetAllAsync(filter)).ToActionResult();

    [HttpGet("{id:int}")] public async Task<IActionResult> Get([FromRoute] int id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    [HttpPut("{id:int}")] public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UserUpdateInfo entity)
        => (await service.UpdateAsync(id, entity)).ToActionResult();

    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete([FromRoute] int id)
        => (await service.DeleteAsync(id)).ToActionResult();
}