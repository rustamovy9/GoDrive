using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Constants;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public sealed class PaymentController(IPaymentService service) : BaseController
{
    private int UserId =>
        int.Parse(User.FindFirst(CustomClaimTypes.Id)?.Value
                  ?? throw new UnauthorizedAccessException("UserId not found"));

    private bool IsAdmin =>
        User.IsInRole(DefaultRoles.Admin);

    private bool IsOwner =>
        User.IsInRole(DefaultRoles.Owner);
    
    // 📥 Получить платежи по booking
    [HttpGet("booking/{bookingId:int}")]
    public async Task<IActionResult> GetByBookingId(int bookingId)
        => (await service.GetByBookingIdAsync(bookingId))
            .ToActionResult();


    // ➕ Создать платеж (клиент)
    [HttpPost]
    [Authorize(Roles = DefaultRoles.User)]
    public async Task<IActionResult> Create(
        [FromBody] PaymentCreateInfo createInfo)
        => (await service.CreateAsync(createInfo))
            .ToActionResult();


    // 🔄 Обновить статус платежа
    // Owner или Admin
    [HttpPut("{id:int}/status")]
    [Authorize(Roles = DefaultRoles.Owner + "," + DefaultRoles.Admin)]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] PaymentStatus status)
        => (await service.UpdateStatusAsync(id, status))
            .ToActionResult();
}