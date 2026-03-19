using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.Review;

public class Create : AbstractValidator<ReviewCreateInfo>
{
    public Create(ITextLocalizer localizer)
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage(localizer.Get(TextKeys.Validation.CarIdGreaterThanZero));

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage(localizer.Get(TextKeys.Validation.RatingRange1To5));

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage(localizer.Get(TextKeys.Validation.CommentMax500));
    }
}
