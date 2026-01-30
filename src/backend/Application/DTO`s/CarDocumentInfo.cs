using Domain.Enums;

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
    string FilePath);


public sealed record CarDocumentUpdateInfo(
    DocumentVerificationStatus VerificationStatus);