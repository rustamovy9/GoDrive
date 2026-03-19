using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.Location;

public class Create : AbstractValidator<LocationCreateInfo>
{
    public Create(ITextLocalizer localizer)
    {
        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage(localizer.Get(TextKeys.Validation.CountryRequiredMax100));

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage(localizer.Get(TextKeys.Validation.CityRequiredMax100));

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage(localizer.Get(TextKeys.Validation.LatitudeRange));

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage(localizer.Get(TextKeys.Validation.LongitudeRange));
    }
}
