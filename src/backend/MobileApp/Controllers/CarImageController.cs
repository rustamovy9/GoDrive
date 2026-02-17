using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/car-images")]
[Authorize]
public sealed class CarImageController(ICarImageService service) : BaseController
{
    private int CurrentUserId =>
        int.Parse(User.FindFirst(CustomClaimTypes.Id)?.Value
                  ?? throw new UnauthorizedAccessException("UserId not found"));

    private bool IsAdmin =>
        User.IsInRole(DefaultRoles.Admin);

    [HttpGet("car/{carId:int}")]
    public async Task<IActionResult> Get(int carId)
        => (await service.GetByCarIdAsync(carId,CurrentUserId,IsAdmin)).ToActionResult();
    
    
    [HttpPost]
    [Authorize(Roles = DefaultRoles.Owner)]
    public async Task<IActionResult> Create([FromForm] CarImageCreateInfo createInfo)
        => (await service.CreateAsync(createInfo, CurrentUserId))
            .ToActionResult();
    
    [Authorize(Roles = DefaultRoles.Owner)]
    [HttpPut("{id:int}/set-main")]
    public async Task<IActionResult> SetMain(int id)
        => (await service.SetMainAsync(id, CurrentUserId))
            .ToActionResult();

    [Authorize(Roles = DefaultRoles.Owner + "," + DefaultRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => (await service.DeleteAsync(id, CurrentUserId, IsAdmin))
            .ToActionResult();
    
    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var res = await service.DownloadAsync(id, CurrentUserId, IsAdmin);

        if (!res.IsSuccess)
            return res.ToActionResult();

        return File(res.Value.FileBytes, "application/octet-stream", res.Value.FileName);
    }
}