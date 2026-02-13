using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Common;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoryController(ICategoryService service) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] BaseFilter filter)
        => (await service.GetAll(filter)).ToActionResult();
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateInfo createInfo)
        => (await service.CreateAsync(createInfo)).ToActionResult(); 
    
    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id,[FromBody] CategoryUpdateInfo updateInfo)
        => (await service.UpdateAsync(id,updateInfo)).ToActionResult();

    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => (await service.DeleteAsync(id)).ToActionResult();

}