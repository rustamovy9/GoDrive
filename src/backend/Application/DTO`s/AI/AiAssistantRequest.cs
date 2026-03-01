namespace Application.DTO_s.AI;

public class AiAssistantRequest
{
    public int UserId { get; set; }
    public string Message { get; set; } = string.Empty;
}