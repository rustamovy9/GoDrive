using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.Category;

public class Update : AbstractValidator<CategoryUpdateInfo>
{
    public Update(ITextLocalizer localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage(localizer.Get(TextKeys.Validation.CategoryNameNotEmptyMax100));
    }
}
