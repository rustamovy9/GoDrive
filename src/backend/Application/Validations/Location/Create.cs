using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.Location;

public class Create : AbstractValidator<LocationCreatInfo>
{
    public Create()
    {
        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Country is required and must not exceed 100 characters.");

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("City is required and must not exceed 100 characters.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180.");
    }
}