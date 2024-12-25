using Application.DTO_s;
using FluentValidation;

namespace Application.Validations.RentalCompany;

public class Create : AbstractValidator<RentalCompanyCreateInfo>
{
    public Create()
    {
        // Имя компании не должно быть пустым и длина не более 100 символов
        RuleFor(company => company.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        // ContactInfo не может быть длиннее 200 символов, если указано
        RuleFor(company => company.ContactInfo)
            .MaximumLength(200).WithMessage("ContactInfo must not exceed 200 characters.");
    }
}