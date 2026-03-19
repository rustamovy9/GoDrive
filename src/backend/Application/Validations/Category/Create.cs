using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Localization;
using FluentValidation;

namespace Application.Validations.Category;

public class Create : AbstractValidator<CategoryCreateInfo>
{
    public Create(ITextLocalizer localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage(localizer.Get(TextKeys.Validation.CategoryNameRequiredMax100));
    }
}
