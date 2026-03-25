using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.Review;

public class Create : AbstractValidator<ReviewCreateInfo>
{
    public Create()
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage("CarId must be greater than 0.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage("Comment must not exceed 500 characters.");
    }
}