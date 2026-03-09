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
            "recommend_cars_with_filters" =>
                await RecommendCarsWithFilters(message, userName),

            "recommend_cars" =>
                await RecommendCars(userName),

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

IMPORTANT RULES:

Respond in the SAME language as the user.

You DO NOT know the cars in the database.
NEVER invent car models.

If user asks about cars → say you will find suitable cars.

Intent rules:

car, машина, аренда, авто, price, $, budget
→ recommend_cars_with_filters

owner earnings
→ owner_analytics

admin statistics
→ admin_stats

Otherwise
→ general_question

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
            temperature = 0.6,
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

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        content = content
            ?.Replace("```json", "")
            .Replace("```", "")
            .Trim();

        try
        {
            return JsonSerializer.Deserialize<AiIntentResponse>(content!)!;
        }
        catch
        {
            return new AiIntentResponse
            {
                Intent = "general_question",
                Reply = content ?? "AI error"
            };
        }
    }

    /* =========================
       SIMPLE CAR RECOMMENDATION
    ========================= */

    private async Task<AiAssistantResponse> RecommendCars(string userName)
    {
        var cars = await _carRepository.GetAvailableCarsAsync();

        if (!cars.Value.Any())
        {
            return new AiAssistantResponse
            {
                Reply = "К сожалению сейчас нет доступных машин."
            };
        }

        var bestCars = cars.Value.Take(3).Select(c => c.Id).ToList();

        return new AiAssistantResponse
        {
            Reply = $"Я нашёл несколько машин для вас, {userName}.",
            RecommendedCarIds = bestCars
        };
    }

    /* =========================
       SMART FILTER SEARCH
    ========================= */

    private async Task<AiAssistantResponse> RecommendCarsWithFilters(
        string message,
        string userName)
    {
        var cars = await _carRepository.GetAvailableCarsAsync();

        if (!cars.Value.Any())
        {
            return new AiAssistantResponse
            {
                Reply = "Сейчас нет доступных машин."
            };
        }

        var query = cars.Value.AsQueryable();

        /* -------- price detection -------- */

        var price = Regex
            .Matches(message, @"\d+")
            .Select(x => int.Parse(x.Value))
            .FirstOrDefault();

        if (price > 0)
        {
            query = query.Where(x =>
                x.CarPrices.Any(p => p.PricePerDay <= price));
        }

        /* -------- seats -------- */

        if (message.ToLower().Contains("семейн"))
            query = query.Where(x => x.Seats >= 5);

        /* -------- city -------- */

        if (message.ToLower().Contains("душанбе"))
            query = query.Where(x => x.Location.City.ToLower() == "dushanbe");

        var carsResult = query.Take(3).ToList();

        if (!carsResult.Any())
        {
            return new AiAssistantResponse
            {
                Reply = "Я не нашёл машин с такими параметрами."
            };
        }

        var ids = carsResult.Select(x => x.Id).ToList();

        return new AiAssistantResponse
        {
            Reply = $"Я нашёл {carsResult.Count} машины для вас.",
            RecommendedCarIds = ids
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
            Reply = $"Активные аренды: {rentals}. Доход за месяц: ${earnings}"
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
            Reply = $"Пользователи: {users}, владельцы: {owners}, машины: {cars}"
        };
    }
}