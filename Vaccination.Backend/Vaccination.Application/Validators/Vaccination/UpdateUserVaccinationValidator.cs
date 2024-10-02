using FluentValidation;
using Vaccination.Application.Dtos.Vaccination;

namespace Vaccination.Application.Validators.Vaccination
{
    public class UpdateUserVaccinationValidator : AbstractValidator<UpdateUserVaccinationRequest>
    {
        public UpdateUserVaccinationValidator()
        {
            RuleFor(x => x.VaccinationDate)
                .NotEmpty()
                .WithMessage("La date de vaccination est requise")
                .Must(x => x != default(DateOnly))
                .WithMessage("Doit être une date réelle");
        }
    }
}