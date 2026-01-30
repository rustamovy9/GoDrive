using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.CarPrice;

public class Create : AbstractValidator<CarPriceCreateInfo>
{
    public Create()
    {
        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage("CarId must be greater than 0.");

        RuleFor(x => x.PricePerDay)
            .GreaterThan(0)
            .WithMessage("PricePerDay must be greater than 0.");
    }
}