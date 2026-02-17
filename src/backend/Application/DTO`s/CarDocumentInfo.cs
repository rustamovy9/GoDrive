using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTO_s;

public sealed record CarDocumentReadInfo(
    int Id,
    int CarId,
    CarDocumentType DocumentType,
    string FilePath,
    DocumentVerificationStatus VerificationStatus,
    double? AiConfidenceScore,
    DateTimeOffset CreatedAt);
    
    
public sealed record CarDocumentCreateInfo(
    int CarId,
    CarDocumentType DocumentType,
    IFormFile File);


public sealed record CarDocumentUpdateInfo(
    DocumentVerificationStatus VerificationStatus);

public sealed record FileResultInfo(
    byte[] Bytes,
    string FilName,
    string ContentType); 