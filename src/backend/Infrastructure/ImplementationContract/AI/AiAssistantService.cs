using System.Text;
using Application.Contracts.AI;
using Application.DTO_s.AI;
using System.Text.Json;

namespace Infrastructure.ImplementationContract.AI;

public class AiAssistantService(HttpClient httpClient) : IAiAssistantService
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly string _apiKey = Environment.GetEnvironmentVariable("GOOGLE_AI_API_KEY")
                                      ?? throw new Exception("Google AI key not found");

    public async Task<AiAssistantResponse> ChatAsync(int userId, string userName, AiAssistantRequest request,
        List<CarAiContext> cars)
    {
        var systemPrompt = $@"
                            You are GoDrive AI Assistant.

                            User name: {userName}

                            You help clients:
                            - Choose rental cars
                            - Answer questions about bookings
                            - Explain prices
                            - Suggest best options

                            Rules:
                            - Return ONLY valid JSON.
                            - Do NOT invent cars.
                            - Use only provided CarId values.
                            - Maximum 3 recommendations.
                            - If user just asks a question, respond normally.
                            - Always address user by their first name.
                            - Speak in user's language.

                            Return format:

                            {{
                              ""reply"": ""message"",
                              ""recommendedCarIds"": [int]
                            }}";


        var carsContext = string.Join("\n",
            cars.Select(c =>
                $"CarId:{c.CarId}, {c.Brand} {c.Model}, Price:{c.PricePerDay}, Rating:{c.Rating}, Year:{c.Year}, City:{c.City}"
            ));

        var fullPrompt = $@"
                            {systemPrompt}

                            User message:
                            {request.Message}
                            Available cars:
                            {carsContext}";

        var body = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = fullPrompt }
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
        
        Console.WriteLine("==== GEMINI RAW RESPONSE ====");
        Console.WriteLine(json);
        Console.WriteLine("=============================");
        
        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("candidates", out var candidates))
        {
            return new AiAssistantResponse
            {
                Reply = "AI service error. Please try again later.",
                RecommendedCarIds = new List<int>()
            };
        }

        var text = candidates[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();
        
        try
        {
            return JsonSerializer.Deserialize<AiAssistantResponse>(text!)!;
        }
        catch (Exception e)
        {
            return new AiAssistantResponse
            {
                Reply = text ?? "Sorry, something went wrong.",
                RecommendedCarIds = new List<int>()
            };
        }
    }
}