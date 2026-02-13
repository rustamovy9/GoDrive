using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/car-prices")]
[Authorize]
public sealed class CarPriceController(ICarPriceService service) : BaseController
{
    private int CurrentUserId =>
        int.Parse(User.FindFirst(CustomClaimTypes.Id)?.Value
                  ?? throw new UnauthorizedAccessException("UserId not found"));

    private bool IsAdmin =>
        User.IsInRole(DefaultRoles.Admin);


    // 🔎 Get price by CarId
    [HttpGet("by-car/{carId:int}")]
    public async Task<IActionResult> Get(int carId)
        => (await service.GetByCarIdAsync(carId)).ToActionResult();


    // ➕ Create price (Owner или Admin)
    [Authorize(Roles = DefaultRoles.Owner + "," + DefaultRoles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CarPriceCreateInfo createInfo)
        => (await service.CreateAsync(
                createInfo,
                CurrentUserId,
                IsAdmin))
            .ToActionResult();


    // ✏ Update price
    [Authorize(Roles = DefaultRoles.Owner + "," + DefaultRoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] CarPriceUpdateInfo updateInfo)
        => (await service.UpdateAsync(
                id,
                updateInfo,
                CurrentUserId,
                IsAdmin))
            .ToActionResult();


    // ❌ Delete price
    [Authorize(Roles = DefaultRoles.Owner + "," + DefaultRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => (await service.DeleteAsync(
                id,
                CurrentUserId,
                IsAdmin))
            .ToActionResult();
}