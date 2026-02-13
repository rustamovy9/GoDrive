using System.Windows.Markup;
using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/car-documents")]
[Authorize]
public sealed class CarDocumentController(ICarDocumentService service) : BaseController
{
    private int UserId =>
        int.Parse(User.FindFirst(CustomClaimTypes.Id)?.Value
                  ?? throw new UnauthorizedAccessException("UserId not found"));

    private bool IsAdmin =>
        User.IsInRole(DefaultRoles.Admin);


    [HttpGet("car/car{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int carId)
    {
        var res = await service.GetByCarIdAsync(carId);
        return res.ToActionResult();
    }

    [Authorize(Roles = DefaultRoles.Owner)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CarDocumentCreateInfo createInfo)
    {
        var res = await service.CreateAsync(createInfo);
        return res.ToActionResult();
    }

    [Authorize(Roles = DefaultRoles.Admin)]
    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CarDocumentUpdateInfo updateInfo)
    {
        var res = await service.UpdateStatusAsync(id,UserId,updateInfo);
        return res.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var res = await service.DeleteAsync(id,UserId,IsAdmin);
        return res.ToActionResult();
    }

    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var res = await service.DownloadAsync(id, UserId, IsAdmin);

        if (!res.IsSuccess)
            return BadRequest(res.Error.Message);

        return File(res.Value.FileBytes, "application/octet-stream", res.Value.FileName);
    }
    
}