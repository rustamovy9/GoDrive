using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Filters;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;


[ApiController]
[Route("api/bookings")]
public class BookingController (IBookingService service) : BaseController
{
    [HttpGet] public async Task<IActionResult> Get([FromQuery] BookingFilter filter)
        => (await service.GetAllAsync(filter)).ToActionResult();

    [HttpGet("{id:guid}")] public async Task<IActionResult> Get([FromRoute] int id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    [HttpPost] public async Task<IActionResult> Create([FromBody] BookingCreateInfo entity)
        => (await service.CreateAsync(entity)).ToActionResult();

    [HttpPut("{id:guid}")] public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BookingUpdateInfo entity)
        => (await service.UpdateAsync(id, entity)).ToActionResult();

    [HttpDelete("{id:guid}")] public async Task<IActionResult> Delete([FromRoute] int id)
        => (await service.DeleteAsync(id)).ToActionResult();
}