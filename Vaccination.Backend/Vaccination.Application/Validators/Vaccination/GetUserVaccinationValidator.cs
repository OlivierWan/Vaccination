using FluentValidation;
using Vaccination.Application.Dtos.Vaccination;

namespace Vaccination.Application.Validators.Vaccination
{
    public class GetUserVaccinationValidator : AbstractValidator<GetFilteredUserVaccinationRequest>
    {
        public GetUserVaccinationValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Le numéro de page doit être supérieur à 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("La taille de la page doit être supérieure à 0");

            RuleFor(x => x.CriteriaSearch)
                .Must(x => x == null || x.Length >= 3)
                .WithMessage("Le critère de recherche doit comporter au moins 3 caractères");
        }
    }
}