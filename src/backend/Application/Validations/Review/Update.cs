using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.Review;

public class Update : AbstractValidator<ReviewUpdateInfo>
{
    public Update(ITextLocalizer localizer)
    {
        RuleFor(x => x.Comment)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage(localizer.Get(TextKeys.Validation.CommentNotEmptyMax500));
    }
}
