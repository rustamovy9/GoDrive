using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Common;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationController(INotificationService service) : ControllerBase
{
    private int UserId =>
        int.Parse(User.FindFirst(CustomClaimTypes.Id)?.Value
                  ?? throw new UnauthorizedAccessException("UserId not found"));


    // 📥 Получить свои уведомления
    [HttpGet]
    public async Task<IActionResult> GetMyNotifications(
        [FromQuery] BaseFilter filter)
        => (await service.GetByUserIdAsync(UserId, filter))
            .ToActionResult();


    // ✅ Отметить одно уведомление как прочитанное
    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
        => (await service.MarkAsReadAsync(id))
            .ToActionResult();


    // ✅ Отметить все уведомления как прочитанные
    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
        => (await service.MarkAllAsReadAsync(UserId))
            .ToActionResult();


    // ❌ Удалить уведомление
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => (await service.DeleteAsync(id))
            .ToActionResult();
}