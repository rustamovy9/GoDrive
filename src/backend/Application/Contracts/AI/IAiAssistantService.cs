using Application.DTO_s.AI;

namespace Application.Contracts.AI;

public interface IAiAssistantService
{
    Task<AiAssistantResponse> ChatAsync(
        AiAssistantRequest request,
        List<CarAiContext> cars);
}