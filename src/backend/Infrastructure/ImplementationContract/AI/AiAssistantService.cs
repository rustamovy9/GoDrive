using System.Text;
using System.Text.Json;
using Application.Contracts.AI;
using Application.Contracts.Repositories;
using Application.DTO_s.AI;

namespace Infrastructure.ImplementationContract.AI;

public class AiAssistantService(
    HttpClient httpClient,
    ICarRepository carRepository,
    IBookingRepository bookingRepository,
    IUserRepository userRepository) : IAiAssistantService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ICarRepository _carRepository = carRepository;
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly IUserRepository _userRepository = userRepository;

    private readonly string _apiKey =
        Environment.GetEnvironmentVariable("GOOGLE_AI_API_KEY")
        ?? throw new Exception("Google AI key not found");

    public async Task<AiAssistantResponse> ChatAsync(
        int userId,
        string userName,
        string role,
        string message)
    {
        var intent = await DetectIntent(userName, role, message);

        switch (intent.Intent)
        {
            case "recommend_cars":
            return await RecommendCars(userName);

            case "recommend_cars_with_filters":
            return await RecommendCarsWithFilters(message, userName);

            case "owner_analytics":
            return await OwnerAnalytics(userId);

            case "admin_stats":
            return await AdminStats();

            default:
            return new AiAssistantResponse
            {
                Reply = intent.Reply
            };
        }
    }

    /* ========================
       INTENT DETECTION
    ======================== */

    private async Task<AiIntentResponse> DetectIntent(
        string userName,
        string role,
        string message)
    {
        var prompt = $@"
You are GoDrive AI assistant for a car rental platform.

User name: {userName}
User role: {role}

Roles:
user → car recommendations
owner → earnings and rentals analytics
admin → platform statistics

IMPORTANT:
Detect the language of the user and respond in the same language.

Possible intents:

recommend_cars
owner_analytics
admin_stats
general_question

Return JSON:

{{
""intent"": """",
""reply"": """"
}}

User message:
{message}
";

        var body = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}",
            content);

        var json = await response.Content.ReadAsStringAsync();
        
        
        Console.WriteLine("===== GEMINI RAW RESPONSE =====");
        Console.WriteLine(json);
        Console.WriteLine("================================");

        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("candidates", out var candidates))
        {
            return new AiIntentResponse
            {
                Intent = "general_question",
                Reply = "AI service error"
            };
        }

        var text = candidates[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        try
        {
            return JsonSerializer.Deserialize<AiIntentResponse>(text!)!;
        }
        catch
        {
            return new AiIntentResponse
            {
                Intent = "general_question",
                Reply = text ?? "AI error"
            };
        }
    }

    /* ========================
       CAR RECOMMENDATION
    ======================== */

    private async Task<AiAssistantResponse> RecommendCars(string userName)
    {
        var cars = await _carRepository.GetAvailableCarsAsync();

        if (cars.Value == null || !cars.Value.Any())
        {
            return new AiAssistantResponse
            {
                Reply = "No cars available right now."
            };
        }

        var bestCars = cars.Value
            .Select(c => new
            {
                c.Id,
                Rating = c.Reviews
                    .Select(r => r.Rating)
                    .DefaultIfEmpty(0)
                    .Average()
            })
            .OrderByDescending(c => c.Rating)
            .Take(3)
            .ToList();

        return new AiAssistantResponse
        {
            Reply = $"Here are some cars you may like, {userName}.",
            RecommendedCarIds = bestCars.Select(c => c.Id).ToList()
        };
    }

    /* ========================
       OWNER ANALYTICS
    ======================== */

    private async Task<AiAssistantResponse> OwnerAnalytics(int ownerId)
    {
        var earnings = await _bookingRepository.GetOwnerMonthlyEarnings(ownerId);
        var rentals = await _bookingRepository.GetActiveRentals(ownerId);

        return new AiAssistantResponse
        {
            Reply = $@"
Owner analytics:

Active rentals: {rentals}
Monthly earnings: ${earnings}
"
        };
    }

    /* ========================
       ADMIN STATS
    ======================== */

    private async Task<AiAssistantResponse> AdminStats()
    {
        var users = await _userRepository.CountUsers();
        var owners = await _userRepository.CountOwners();
        var cars = await _carRepository.CountCars();

        return new AiAssistantResponse
        {
            Reply = $@"
Platform statistics:

Users: {users}
Owners: {owners}
Cars: {cars}
"
        };
    }
    
    private async Task<AiAssistantResponse> RecommendCarsWithFilters(
        string message,
        string userName)
    {
        var cars = await _carRepository.GetAvailableCarsAsync();

        if (cars.Value == null || !cars.Value.Any())
        {
            return new AiAssistantResponse
            {
                Reply = "No cars available."
            };
        }

        var query = cars.Value.AsQueryable();

        if (message.Contains("50"))
            query = query.Where(c =>
                c.CarPrices.Any(p => p.PricePerDay <= 50));

        if (message.ToLower().Contains("family"))
            query = query.Where(c => c.Seats >= 5);

        if (message.ToLower().Contains("dushanbe"))
            query = query.Where(c => c.Location.City.ToLower() == "dushanbe");

        var result = query
            .Select(c => new
            {
                c.Id,
                Rating = c.Reviews
                    .Select(r => r.Rating)
                    .DefaultIfEmpty(0)
                    .Average()
            })
            .OrderByDescending(x => x.Rating)
            .Take(3)
            .ToList();

        return new AiAssistantResponse
        {
            Reply = $"Here are cars that match your request, {userName}.",
            RecommendedCarIds = result.Select(x => x.Id).ToList()
        };
    }
}