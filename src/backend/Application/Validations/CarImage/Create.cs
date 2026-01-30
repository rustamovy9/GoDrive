using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.CarImage;

public class Create : AbstractValidator<CarImageCreateInfo>
{
    public Create()
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage("CarId must be greater than 0.");

        RuleFor(x => x.ImagePath)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("ImagePath  is required and must not exceed 500 characters.");
    }
}