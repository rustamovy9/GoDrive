using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.Location;

public class Update : AbstractValidator<LocationUpdateInfo>
{
    public Update(ITextLocalizer localizer)
    {
        When(x => x.Country != null, () =>
        {
            RuleFor(x => x.Country!)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage(localizer.Get(TextKeys.Validation.CountryRequiredMax100));
        });

        When(x => x.City != null, () =>
        {
            RuleFor(x => x.City!)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage(localizer.Get(TextKeys.Validation.CityRequiredMax100));
        });

        When(x => x.Latitude.HasValue, () =>
        {
            RuleFor(x => x.Latitude!.Value)
                .InclusiveBetween(-90, 90)
                .WithMessage(localizer.Get(TextKeys.Validation.LatitudeRange));
        });

        When(x => x.Longitude.HasValue, () =>
        {
            RuleFor(x => x.Longitude!.Value)
                .InclusiveBetween(-180, 180)
                .WithMessage(localizer.Get(TextKeys.Validation.LongitudeRange));
        });
    }
}
