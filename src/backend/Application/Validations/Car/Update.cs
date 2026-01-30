using Application.DTO_s;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validations.Car;

public class Update : AbstractValidator<CarUpdateInfo>
{
    public Update()
    {
        When(x => x.Brand != null, () =>
        {
            RuleFor(x => x.Brand!)
                .NotEmpty()
                .MaximumLength(100);
        });

        When(x => x.Model != null, () =>
        {
            RuleFor(x => x.Model!)
                .NotEmpty()
                .MaximumLength(100);
        });

        When(x => x.Year.HasValue, () =>
        {
            RuleFor(x => x.Year!.Value)
                .InclusiveBetween(1950, DateTime.UtcNow.Year + 1)
                .WithMessage("Year must be a valid production year.");
        });

        When(x => x.CategoryId.HasValue, () =>
        {
            RuleFor(x => x.CategoryId!.Value)
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than 0.");
        });

        When(x => x.LocationId.HasValue, () =>
        {
            RuleFor(x => x.LocationId!.Value)
                .GreaterThan(0)
                .WithMessage("LocationId must be greater than 0.");
        });

        When(x => x.RentalCompanyId.HasValue, () =>
        {
            RuleFor(x => x.RentalCompanyId!.Value)
                .GreaterThan(0)
                .WithMessage("RentalCompanyId must be greater than 0.");
        });
    }
}