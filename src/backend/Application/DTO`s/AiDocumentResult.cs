namespace Application.DTO_s;

public sealed record AiDocumentResult(
    double ConfidenceScore,
    string ExtractedText,
    string ExtractedJson,
    bool IsValid);