using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.Review;

public class Update : AbstractValidator<ReviewUpdateInfo>
{
    public Update()
    {
        // Рейтинг должен быть в диапазоне от 1 до 5
        RuleFor(review => review.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        // Если комментарий предоставлен, его длина не должна превышать 500 символов
        RuleFor(review => review.Comment)
            .MaximumLength(500).WithMessage("Comment must not exceed 500 characters.");
        
    }
}