using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.Category;

public class Create : AbstractValidator<CategoryCreateInfo>
{
    public Create()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Category name is required and must not exceed 100 characters.");
    }
}