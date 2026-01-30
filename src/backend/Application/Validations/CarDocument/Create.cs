using Application.DTO_s;
using Domain.Enums;
using FluentValidation;

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
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("FilePath is required and must not exceed 500 characters.");
    }
}