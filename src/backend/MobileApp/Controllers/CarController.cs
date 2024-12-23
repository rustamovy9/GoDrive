using Application.Contracts.Services;
using Application.DTO_s;
using Application.Filters;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;


[ApiController]
[Route("api/cars")]
public class CarController (ICarService service) : BaseController
{
    [HttpGet] public async Task<IActionResult> Get([FromQuery] CarFilter filter)
        => (await service.GetAllAsync(filter)).ToActionResult();

    [HttpGet("{id:guid}")] public async Task<IActionResult> Get([FromRoute] int id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    [HttpPost] public async Task<IActionResult> Create([FromForm] CarCreateInfo entity)
        => (await service.CreateAsync(entity)).ToActionResult();

    [HttpPut("{id:guid}")] public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CarUpdateInfo entity)
        => (await service.UpdateAsync(id, entity)).ToActionResult();

    [HttpDelete("{id:guid}")] public async Task<IActionResult> Delete([FromRoute] int id)
        => (await service.DeleteAsync(id)).ToActionResult();
}