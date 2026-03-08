namespace Application.DTO_s.AI;

public class AiAssistantRequest
{
    public string Message { get; set; } = string.Empty;
}

public class AiAssistantResponse
{
    public string Reply { get; set; } = string.Empty;
    public List<int> RecommendedCarIds { get; set; } = new();
}

public class AiIntentResponse
{
    public string Intent { get; set; } = "";
    public string Reply { get; set; } = "";
}

public class CarAiContext
{
    public int CarId { get; set; }
    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public decimal PricePerDay { get; set; }
    public double Rating { get; set; }
    public int Year { get; set; }
    public string City { get; set; } = "";
}