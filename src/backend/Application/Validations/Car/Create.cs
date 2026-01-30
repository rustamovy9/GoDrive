using Application.DTO_s;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validations.Car;

public class Create : AbstractValidator<CarCreateInfo>
{
    public Create()
    {
        RuleFor(x => x.Brand)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Model)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Year)
            .InclusiveBetween(1950, DateTime.UtcNow.Year + 1)
            .WithMessage("Year must be a valid production year.");

        RuleFor(x => x.RegistrationNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId must be greater than 0.");

        RuleFor(x => x.LocationId)
            .GreaterThan(0)
            .WithMessage("LocationId must be greater than 0.");

        RuleFor(x => x.RentalCompanyId)
            .GreaterThan(0)
            .When(x => x.RentalCompanyId.HasValue)
            .WithMessage("RentalCompanyId must be greater than 0.");
    }
}