using Application.Contracts.Services;
using Application.DTO_s;
using Application.Filters;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize]
public sealed class BookingController(IBookingService service) : BaseController
{
    private int CurrentUserId =>
        int.Parse(User.FindFirst(CustomClaimTypes.Id)?.Value
                  ?? throw new UnauthorizedAccessException("UserId not found"));

    private bool IsAdmin =>
        User.IsInRole(DefaultRoles.Admin);

    // 🔍 GET ALL
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BookingFilter filter)
        => (await service.GetAllAsync(filter, CurrentUserId, IsAdmin))
            .ToActionResult();

    // 🔍 GET BY ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
        => (await service.GetByIdAsync(id, CurrentUserId, IsAdmin))
            .ToActionResult();

    // ➕ CREATE
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingCreateInfo info)
        => (await service.CreateAsync(info, CurrentUserId))
            .ToActionResult();

    // ✏ UPDATE (только Pending и только владелец booking)
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] BookingUpdateInfo info)
        => (await service.UpdateAsync(id, info, CurrentUserId))
            .ToActionResult();

    // 🔄 UPDATE STATUS (Pending → Confirmed/Rejected/Cancelled и т.д.)
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] BookingUpdateStatusInfo info)
        => (await service.UpdateStatusAsync(
                id,
                info,
                CurrentUserId,
                IsAdmin))
            .ToActionResult();

    // ❌ DELETE (только Pending)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => (await service.DeleteAsync(id, CurrentUserId, IsAdmin))
            .ToActionResult();
}
