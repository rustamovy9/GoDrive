using System.Text.Json;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Tesseract;

namespace Infrastructure.AI;

public class AiDocumentService : IAiDocumentService
{
    private readonly string tessDataPath;

    public AiDocumentService()
    {
        tessDataPath = Path.Combine(AppContext.BaseDirectory, "tessdata");
    }

    public async Task<Result<AiDocumentResult>> VerifyAsync(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Result<AiDocumentResult>.Failure(Error.BadRequest("File path is required"));

            if (!File.Exists(filePath))
                return Result<AiDocumentResult>.Failure(Error.NotFound());

            var res = await Task.Run(() =>
            {
                using var engine = new TesseractEngine(tessDataPath, "eng+rus", EngineMode.Default);

                using var image = Pix.LoadFromFile(filePath);

                using var page = engine.Process(image);

                var text = page.GetText();

                var confidence = page.GetMeanConfidence();

                var extracted = ExtractData(text);

                var json = JsonSerializer.Serialize(extracted);

                var isValid =
                    extracted.ContainsKey("vin") ||
                    extracted.ContainsKey("registration");

                return new AiDocumentResult
                (
                    confidence,
                    text,
                    json,
                    isValid
                );
            });
            return Result<AiDocumentResult>.Success(res);
        }
        catch (Exception e)
        {
            return Result<AiDocumentResult>.Failure(Error.InternalServerError($"AI verification filed: {e.Message}"));
        }
    }

    private Dictionary<string, string> ExtractData(string text)
    {
        var result = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(text))
            return result;

        var lines = text.Split('\n');

        foreach (var line in lines)
        {
            var value = line.Trim();

            if (string.IsNullOrWhiteSpace(value))
                continue;

            if (value.Contains("VIN", StringComparison.OrdinalIgnoreCase))
                result["vin"] = value;

            if (value.Contains("Registration", StringComparison.OrdinalIgnoreCase))
                result["registration"] = value;

            if (value.Contains("Owner", StringComparison.OrdinalIgnoreCase))
                result["owner"] = value;
        }

        return result;
    }
}