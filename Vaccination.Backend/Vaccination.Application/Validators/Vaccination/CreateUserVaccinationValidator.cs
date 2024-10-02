using FluentValidation;
using Vaccination.Application.Dtos.Vaccination;

namespace Vaccination.Application.Validators.Vaccination
{
    public class CreateUserVaccinationValidator : AbstractValidator<CreateUserVaccinationRequest>
    {
        public CreateUserVaccinationValidator()
        {
            RuleFor(x => x.VaccinationDate)
                .NotEmpty()
                .WithMessage("Vaccination date est requis")
                .Must(x => x != default(DateOnly))
                .WithMessage("Doit être une vraie date")
                ;

            RuleFor(x => x.VaccineCalendarId)
                .NotEmpty()
                .WithMessage("Vaccination type  est requis");
        }
    }
}