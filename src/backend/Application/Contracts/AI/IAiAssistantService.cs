using Application.DTO_s.AI;

namespace Application.Contracts.AI;

public interface IAiAssistantService
{
    Task<AiAssistantResponse> ChatAsync(
        int userId,
        string userName,
        AiAssistantRequest request,
        List<CarAiContext> cars);
}