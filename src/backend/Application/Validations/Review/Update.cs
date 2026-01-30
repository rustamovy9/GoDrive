using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.Review;

public class Update : AbstractValidator<ReviewUpdateInfo>
{
    public Update()
    {
        When(x => x.Comment != null, () =>
        {
            RuleFor(x => x.Comment!)
                .NotEmpty()
                .MaximumLength(500)
                .WithMessage("Comment must not be empty and must not exceed 500 characters.");
        });
    }
}