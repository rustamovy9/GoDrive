using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.Location;

public class Update : AbstractValidator<LocationUpdateInfo>
{
    public Update()
    {
        When(x => x.Country != null, () =>
        {
            RuleFor(x => x.Country!)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Country must not be empty and must not exceed 100 characters.");
        });

        When(x => x.City != null, () =>
        {
            RuleFor(x => x.City!)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("City must not be empty and must not exceed 100 characters.");
        });

        When(x => x.Latitude.HasValue, () =>
        {
            RuleFor(x => x.Latitude!.Value)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be between -90 and 90.");
        });

        When(x => x.Longitude.HasValue, () =>
        {
            RuleFor(x => x.Longitude!.Value)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be between -180 and 180.");
        });
    }
}