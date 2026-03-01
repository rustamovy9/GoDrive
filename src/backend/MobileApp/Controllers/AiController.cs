using Application.Contracts.AI;
using Application.Contracts.Repositories;
using Application.DTO_s.AI;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace MobileApp.Controllers;

[ApiController]
[Route("api/ai")]
public sealed class AiController(IAiAssistantService aiAssistantService,ICarRepository carRepository) : BaseController
{
    int UserId =>
        int.Parse(User.FindFirst(CustomClaimTypes.Id)?.Value
                  ?? throw new UnauthorizedAccessException("UserId not found"));
    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] AiAssistantRequest aiAssistantRequest)
    {
        aiAssistantRequest.UserId = UserId;
        var cars = await carRepository.GetAvailableCarsAsync();
        
        var context = cars.Value!.Select(c => new CarAiContext
        {
            CarId = c.Id,
            Brand = c.Brand,
            Model = c.Model,
            PricePerDay = c.CarPrices.Select(x=>x.PricePerDay).FirstOrDefault(),
            Rating = c.Reviews.Average(x=>x.Rating),
            Year = c.Year,
            City = c.Location.City
        }).ToList();

        var res = await aiAssistantService.ChatAsync(aiAssistantRequest, context); 
        
        return Ok(res);
    }
    
    
}