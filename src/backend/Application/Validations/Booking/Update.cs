namespace Application.Validations.Specialization.Application.Validations.Specialization;

public class Update : AbstractValidator<SpecializationUpdateInfo>
{
    public UpdateSpecializationValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .Matches("^[A-Za-z0-9]+$").WithMessage("Code must contain only alphanumeric characters.")
            .Length(3, 20).WithMessage("Code must be between 3 and 20 characters.");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive must be specified.");
    }
}