using Application.DTO_s;
using Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validations.CarDocument;

public class Create : AbstractValidator<CarDocumentCreateInfo>
{
    public Create()
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage("CarId must be greater than 0.");

        RuleFor(x => x.DocumentType)
            .IsInEnum()
            .Must(x => x != CarDocumentType.Unknown)
            .WithMessage("DocumentType must be a valid document type.");

        RuleFor(x => x.FilePath)
            .NotNull().WithMessage("Document Image is required.")
            .Must(BeValidImage)
            .WithMessage("File must be a valid image (jpg, png, jpeg, webp) and less than 5MB.");
    }
    
    private static bool BeValidImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        // 5MB лимит
        const long maxSize = 5 * 1024 * 1024;
        if (file.Length > maxSize)
            return false;

        var allowedTypes = new[]
        {
            "image/jpeg",
            "image/pdf",
            "image/png",
            "image/jpg",
            "image/webp"
        };

        return allowedTypes.Contains(file.ContentType.ToLower());
    }
}