using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.Category;

public class Update : AbstractValidator<CategoryUpdateInfo>
{
    public Update()
    {
        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name!)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Category name must not be empty and must not exceed 100 characters.");
        });
    }
}