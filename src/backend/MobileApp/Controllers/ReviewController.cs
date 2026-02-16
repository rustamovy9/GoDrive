using Application.Contracts.Services;
using Application.DTO_s;
using Application.Filters;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileApp.HelpersApi.Extensions.ResultPattern;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/reviews")]
[Authorize]
public class ReviewController(IReviewService service) : BaseController
{
    private int CurrentUserId =>
        int.Parse(User.FindFirst(CustomClaimTypes.Id)?.Value
                  ?? throw new UnauthorizedAccessException("UserId not found"));

    // -------------------- GET ALL --------------------

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] ReviewFilter filter)
        => (await service.GetAllAsync(filter)).ToActionResult();

    // -------------------- GET BY ID --------------------

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id)
        => (await service.GetByIdAsync(id)).ToActionResult();

    // -------------------- CREATE --------------------

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReviewCreateInfo entity)
        => (await service.CreateAsync(CurrentUserId, entity)).ToActionResult();

    // -------------------- UPDATE --------------------

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ReviewUpdateInfo entity)
        => (await service.UpdateAsync(id, entity)).ToActionResult();

    // -------------------- DELETE --------------------

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => (await service.DeleteAsync(id)).ToActionResult();
}