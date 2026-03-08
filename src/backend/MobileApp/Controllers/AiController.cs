using Application.Contracts.AI;
using Application.DTO_s.AI;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/ai")]
public sealed class AiController(IAiAssistantService aiAssistantService) : BaseController
{
    int UserId =>
        int.Parse(User.FindFirst(CustomClaimTypes.Id)?.Value
                  ?? throw new UnauthorizedAccessException("UserId not found"));

    string FirstName =>
        User.FindFirst(CustomClaimTypes.UserName)?.Value ?? "User";

    string Role =>
        User.FindFirst(CustomClaimTypes.Role)?.Value ?? "user";

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] AiAssistantRequest request)
    {
        var result = await aiAssistantService.ChatAsync(
            UserId,
            FirstName,
            Role,
            request.Message);

        return Ok(result);
    }
}