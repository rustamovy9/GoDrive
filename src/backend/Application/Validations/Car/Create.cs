using Application.DTO_s;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validations.Car;

public class Create : AbstractValidator<CarCreateInfo>
{
    public Create()
    {
          // Validate Brand
        RuleFor(car => car.Brand)
            .NotEmpty().WithMessage("Brand is required.")
            .MaximumLength(100).WithMessage("Brand must not exceed 100 characters.");

        // Validate Model
        RuleFor(car => car.Model)
            .NotEmpty().WithMessage("Model is required.")
            .MaximumLength(100).WithMessage("Model must not exceed 100 characters.");

        // Validate Year
        RuleFor(car => car.Year)
            .GreaterThan(1885).WithMessage("Year must be greater than 1885.") // First car invention year
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year must not be in the future.");

        // Validate Category (optional)
        RuleFor(car => car.Category)
            .MaximumLength(50).WithMessage("Category must not exceed 50 characters.");

        // Validate Registration Number
        RuleFor(car => car.RegistrationNumber)
            .NotEmpty().WithMessage("Registration number is required.")
            .Matches("^[A-Z0-9-]+$").WithMessage("Registration number can only contain uppercase letters, numbers, and hyphens.");

        // Validate Location (optional)
        RuleFor(car => car.Location)
            .MaximumLength(100).WithMessage("Location must not exceed 100 characters.");

        // Validate CarStatus
        RuleFor(car => car.CarStatus)
            .IsInEnum().WithMessage("Invalid car status value.");

        // Validate File (optional)
        RuleFor(car => car.File)
            .Must(file => file == null || IsValidFileType(file))
            .WithMessage("Invalid file type. Only JPEG and PNG are allowed.")
            .Must(file => file == null || file.Length <= 50 * 1024 * 1024) // 5 MB
            .WithMessage("File size must not exceed 5 MB.");
    }

    private bool IsValidFileType(IFormFile? file)
    {
        if (file == null) return true;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(file.FileName)?.ToLower();

        return allowedExtensions.Contains(fileExtension);
    }
}