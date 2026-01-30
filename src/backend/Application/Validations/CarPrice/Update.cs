using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.CarPrice;

public class Update : AbstractValidator<CarPriceUpdateInfo>
{
    public Update()
    {
        When(x => x.PricePerDay.HasValue, () =>
        {
            RuleFor(x => x.PricePerDay!.Value)
                .GreaterThan(0)
                .WithMessage("PricePerDay must be greater than 0.");
        });
    }
}