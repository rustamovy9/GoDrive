using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Common;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationController(ILocationService service) : ControllerBase
{
    // 🌍 Получить все локации (доступно всем)
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BaseFilter filter)
        => (await service.GetAllAsync(filter)).ToActionResult();


    // 📍 Получить локацию по id (доступно всем)
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
        => (await service.GetByIdAsync(id)).ToActionResult();


    // ➕ Создать локацию (только Admin)
    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] LocationCreateInfo createInfo)
        => (await service.CreateAsync(createInfo)).ToActionResult();


    // ✏ Обновить локацию (только Admin)
    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] LocationUpdateInfo updateInfo)
        => (await service.UpdateAsync(id, updateInfo)).ToActionResult();


    // ❌ Удалить локацию (только Admin)
    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => (await service.DeleteAsync(id)).ToActionResult();
}