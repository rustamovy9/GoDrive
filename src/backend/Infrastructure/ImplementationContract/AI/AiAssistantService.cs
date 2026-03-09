using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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
        Environment.GetEnvironmentVariable("GROQ_API_KEY")
        ?? throw new Exception("GROQ API key not found");

    public async Task<AiAssistantResponse> ChatAsync(
        int userId,
        string userName,
        string role,
        string message)
    {
        var intent = await DetectIntent(userName, role, message);

        return intent.Intent switch
        {
            "recommend_cars" =>
                await RecommendCars(userName, intent.Reply),

            "recommend_cars_with_filters" =>
                await RecommendCarsWithFilters(message, userName, intent.Reply),

            "owner_analytics" =>
                await OwnerAnalytics(userId),

            "admin_stats" =>
                await AdminStats(),

            _ => new AiAssistantResponse
            {
                Reply = intent.Reply
            }
        };
    }

    /* =========================
       AI INTENT DETECTION
    ========================= */

    private async Task<AiIntentResponse> DetectIntent(
        string userName,
        string role,
        string message)
    {
        var prompt = $@"
You are GoDrive AI assistant for a car rental platform.

User name: {userName}
User role: {role}

IMPORTANT:
Respond in the SAME language as the user.

Intent rules:

If user mentions:
car, rent, машина, аренда, авто, price, $, budget
→ intent = recommend_cars_with_filters

If owner asks about earnings or rentals
→ intent = owner_analytics

If admin asks about platform statistics
→ intent = admin_stats

Otherwise
→ intent = general_question

Return ONLY JSON:

{{
""intent"": """",
""reply"": """"
}}

User message:
{message}
";

        var body = new
        {
            model = "llama-3.3-70b-versatile",
            temperature = 0.7,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.groq.com/openai/v1/chat/completions");

        request.Headers.Add("Authorization", $"Bearer {_apiKey}");

        request.Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return new AiIntentResponse
            {
                Intent = "general_question",
                Reply = "AI временно недоступен."
            };
        }

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("choices", out var choices))
        {
            return new AiIntentResponse
            {
                Intent = "general_question",
                Reply = "AI error."
            };
        }

        var text = choices[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        Console.WriteLine("AI TEXT:");
        Console.WriteLine(text);
        
        text = text?
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

        try
        {
            var result = JsonSerializer.Deserialize<AiIntentResponse>(text!);

            if (result == null)
                throw new Exception();

            if (string.IsNullOrWhiteSpace(result.Reply))
                result.Reply = text!;

            return result;
        }
        catch
        {
            return new AiIntentResponse
            {
                Intent = "general_question",
                Reply = text ?? "Извините, я не смог ответить."
            };
        }
    }

    /* =========================
       CAR RECOMMENDATION
    ========================= */

    private async Task<AiAssistantResponse> RecommendCars(
        string userName,
        string aiReply)
    {
        var cars = await _carRepository.GetAvailableCarsAsync();

        if (cars.Value == null || !cars.Value.Any())
        {
            return new AiAssistantResponse
            {
                Reply = "Сейчас нет доступных машин."
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
            Reply = string.IsNullOrWhiteSpace(aiReply)
                ? $"Вот несколько машин для вас, {userName}."
                : aiReply,

            RecommendedCarIds = bestCars.Select(c => c.Id).ToList()
        };
    }

    /* =========================
       OWNER ANALYTICS
    ========================= */

    private async Task<AiAssistantResponse> OwnerAnalytics(int ownerId)
    {
        var earnings = await _bookingRepository.GetOwnerMonthlyEarnings(ownerId);
        var rentals = await _bookingRepository.GetActiveRentals(ownerId);

        return new AiAssistantResponse
        {
            Reply = $"""
Аналитика владельца:

Активные аренды: {rentals}
Доход за месяц: ${earnings}
"""
        };
    }

    /* =========================
       ADMIN STATS
    ========================= */

    private async Task<AiAssistantResponse> AdminStats()
    {
        var users = await _userRepository.CountUsers();
        var owners = await _userRepository.CountOwners();
        var cars = await _carRepository.CountCars();

        return new AiAssistantResponse
        {
            Reply = $"""
Статистика платформы:

Пользователи: {users}
Владельцы: {owners}
Машины: {cars}
"""
        };
    }

    /* =========================
       SMART CAR SEARCH
    ========================= */

    private async Task<AiAssistantResponse> RecommendCarsWithFilters(
        string message,
        string userName,
        string aiReply)
    {
        var cars = await _carRepository.GetAvailableCarsAsync();

        if (cars.Value == null || !cars.Value.Any())
        {
            return new AiAssistantResponse
            {
                Reply = "Нет доступных машин."
            };
        }

        var query = cars.Value.AsQueryable();

        var numbers = Regex
            .Matches(message, @"\d+")
            .Select(x => int.Parse(x.Value));

        var price = numbers.FirstOrDefault();

        if (price > 0)
        {
            query = query.Where(c =>
                c.CarPrices.Any(p => p.PricePerDay <= price));
        }

        if (message.ToLower().Contains("семейн"))
            query = query.Where(c => c.Seats >= 5);

        if (message.ToLower().Contains("dushanbe") ||
            message.ToLower().Contains("душанбе"))
        {
            query = query.Where(c =>
                c.Location.City.ToLower() == "dushanbe");
        }

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
            Reply = string.IsNullOrWhiteSpace(aiReply)
                ? $"Вот подходящие машины, {userName}."
                : aiReply,

            RecommendedCarIds = result.Select(x => x.Id).ToList()
        };
    }
}