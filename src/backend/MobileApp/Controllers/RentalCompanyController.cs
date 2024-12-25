using Application.Contracts.Services;
using Application.DTO_s;
using Application.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/rental-companies")]
[Authorize]
public class RentalCompanyController(IRentalCompanyService service) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] RentalCompanyFilter filter)
        => (await service.GetAllAsync(filter)).ToActionResult();

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RentalCompanyCreateInfo entity)
        => (await service.CreateAsync(entity)).ToActionResult();

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RentalCompanyUpdateInfo entity)
        => (await service.UpdateAsync(id, entity)).ToActionResult();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
        => (await service.DeleteAsync(id)).ToActionResult();
}