namespace Application.DTO_s.AI;

public class AiAssistantResponse
{
    public string Reply { get; set; } = string.Empty;
    public List<int> RecommendedCarIds { get; set; } = new();
}