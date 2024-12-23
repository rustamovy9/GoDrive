using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.Review;

public class Create : AbstractValidator<ReviewCreateInfo>
{
    public Create()
    {
        // Рейтинг должен быть в диапазоне от 1 до 5
        RuleFor(review => review.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        // Если комментарий предоставлен, его длина не должна превышать 500 символов
        RuleFor(review => review.Comment)
            .MaximumLength(500).WithMessage("Comment must not exceed 500 characters.");

        // UserId и CarId должны быть положительными целыми числами
        RuleFor(review => review.UserId)
            .GreaterThan(0).WithMessage("UserId must be a positive integer.");

        RuleFor(review => review.CarId)
            .GreaterThan(0).WithMessage("CarId must be a positive integer.");
    }
}