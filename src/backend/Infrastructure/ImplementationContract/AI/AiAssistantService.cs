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
    IUserRepository userRepository,
    IChatRepository chatRepository) : IAiAssistantService
{


    private readonly string _apiKey =
        Environment.GetEnvironmentVariable("GROQ_API_KEY")
        ?? throw new Exception("GROQ API key not found");

    public async Task<AiAssistantResponse> ChatAsync(
        int userId,
        string userName,
        string role,
        string message)
    {
        await chatRepository.SaveMessage(userId, "user", message);

        var intent = await DetectIntent(userName, role, message);

        AiAssistantResponse response;

        switch (intent.Intent)
        {
            case "recommend_cars_with_filters":
                response = await RecommendCarsWithFilters(message);
                break;

            case "recommend_cars":
                response = await RecommendCars(message);
                break;

            case "owner_analytics":
                response = await OwnerAnalytics(userId, message);
                break;

            case "admin_stats":
                response = await AdminStats(message);
                break;

            default:
                response = new AiAssistantResponse
                {
                    Reply = intent.Reply
                };
                break;
        }

        await chatRepository.SaveMessage(userId, "assistant", response.Reply);

        return response;
    }

    /* =========================
       INTENT DETECTION
    ========================= */

    private async Task<AiIntentResponse> DetectIntent(
        string userName,
        string role,
        string message)
    {
        var prompt = $@"
You are AI assistant for car rental platform GoDrive.

IMPORTANT:
Always answer in the SAME language as the user.

IMPORTANT RULE:
You DO NOT know cars in the database.
NEVER invent car models.
If the user asks about cars, say you will search suitable cars.

User name: {userName}
User role: {role}

Possible intents:

recommend_cars
recommend_cars_with_filters
owner_analytics
admin_stats
general_question

Respond ONLY JSON:

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
            temperature = 0.5,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var json = await SendAiRequest(body);

        using var doc = JsonDocument.Parse(json);

        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        content = content?
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

        try
        {
            using var intentDoc = JsonDocument.Parse(content!);

            var intent = intentDoc.RootElement
                .GetProperty("intent")
                .GetString();

            var reply = intentDoc.RootElement
                .GetProperty("reply")
                .GetString();

            return new AiIntentResponse
            {
                Intent = intent ?? "general_question",
                Reply = reply ?? ""
            };
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
       RECOMMEND CARS
    ========================= */

    private async Task<AiAssistantResponse> RecommendCars(string message)
    {
        var cars = await carRepository.GetAvailableCarsAsync();

        if (!cars.IsSuccess || !cars.Value!.Any())
        {
            return new AiAssistantResponse
            {
                Reply = "К сожалению сейчас нет доступных машин."
            };
        }

        var result = cars.Value!.Take(3).ToList();

        var carsText = string.Join("\n",
            result.Select(c => $"{c.Brand} {c.Model}"));

        var reply = await GenerateAiReply(message, carsText);

        return new AiAssistantResponse
        {
            Reply = reply,
            RecommendedCarIds = result.Select(c => c.Id).ToList()
        };
    }

    /* =========================
       FILTERED SEARCH
    ========================= */

    private async Task<AiAssistantResponse> RecommendCarsWithFilters(
        string message)
    {
        var cars = await carRepository.GetAvailableCarsAsync();

        if (!cars.IsSuccess || !cars.Value!.Any())
        {
            return new AiAssistantResponse
            {
                Reply = "Сейчас нет доступных машин."
            };
        }

        var query = cars.Value!.AsQueryable();

        var price = Regex
            .Matches(message, @"\d+")
            .Select(x => int.Parse(x.Value))
            .FirstOrDefault();

        if (price > 0)
        {
            query = query.Where(c =>
                c.CarPrices
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => p.PricePerDay)
                    .FirstOrDefault() <= price);
        }

        var result = query.Take(3).ToList();

        var carsText = string.Join("\n",
            result.Select(c => $"{c.Brand} {c.Model}"));

        var reply = await GenerateAiReply(message, carsText);

        return new AiAssistantResponse
        {
            Reply = reply,
            RecommendedCarIds = result.Select(c => c.Id).ToList()
        };
    }

    /* =========================
       OWNER ANALYTICS
    ========================= */

    private async Task<AiAssistantResponse> OwnerAnalytics(
        int ownerId,
        string message)
    {
        var earnings = await bookingRepository.GetOwnerMonthlyEarnings(ownerId);
        var rentals = await bookingRepository.GetActiveRentals(ownerId);

        var data = $"""
Active rentals: {rentals}
Monthly earnings: {earnings}
""";

        var reply = await GenerateAiReply(message, data);

        return new AiAssistantResponse
        {
            Reply = reply
        };
    }

    /* =========================
       ADMIN STATS
    ========================= */

    private async Task<AiAssistantResponse> AdminStats(string message)
    {
        var users = await userRepository.CountUsers();
        var owners = await userRepository.CountOwners();
        var cars = await carRepository.CountCars();

        var data = $"""
Users: {users}
Owners: {owners}
Cars: {cars}
""";

        var reply = await GenerateAiReply(message, data);

        return new AiAssistantResponse
        {
            Reply = reply
        };
    }

    /* =========================
       GENERATE FINAL AI REPLY
    ========================= */

    private async Task<string> GenerateAiReply(string message, string data)
    {
        var prompt = $@"
User message:
{message}

System data:
{data}

Respond naturally using this data.
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

        var json = await SendAiRequest(body);

        return ExtractAiMessage(json);
    }

    /* =========================
       AI HTTP REQUEST
    ========================= */

    private async Task<string> SendAiRequest(object body)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.groq.com/openai/v1/chat/completions");

        request.Headers.Add("Authorization", $"Bearer {_apiKey}");

        request.Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");

        var response = await httpClient.SendAsync(request);

        var result = await response.Content.ReadAsStringAsync();

        Console.WriteLine(result);

        return result;
    }

    /* =========================
       EXTRACT AI MESSAGE
    ========================= */

    private string ExtractAiMessage(string json)
    {
        using var doc = JsonDocument.Parse(json);

        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (string.IsNullOrWhiteSpace(content))
            return "";

        content = content
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

        return content;
    }
}