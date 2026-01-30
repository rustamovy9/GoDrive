using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class CarDocument : BaseEntity
{
    public int CarId { get; set; }
    public CarDocumentType DocumentType { get; set; }
    public string FilePath { get; set; } = null!;

    public DocumentVerificationStatus VerificationStatus { get; set; }
    public string? AiExtractedDataJson { get; set; }
    public double? AiConfidenceScore { get; set; }
    
    public Car Car { get; set; } = null!;
}