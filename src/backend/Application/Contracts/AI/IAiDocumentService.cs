using Application.DTO_s;
using Application.Extensions.ResultPattern;

namespace Application.Contracts.AI;

public interface IAiDocumentService
{
    Task<Result<AiDocumentResult>> VerifyAsync(string filePath);
}