using FluentValidation;
using Vaccination.Application.Dtos.User;

namespace Vaccination.Application.Validators.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("L'email n'est pas valide");

            RuleFor(x => x.FirstName)
                .MaximumLength(150)
                .WithMessage("Le prénom doit faire au maximum 150 caractères");

            RuleFor(x => x.LastName)
                .MaximumLength(150)
                .WithMessage("Le nom de famille doit faire au maximum 150 caractères");

            RuleFor(x => x.SocialSecurityNumber)
                .MaximumLength(13)
                .WithMessage("Le numéro de sécurité sociale doit faire au maximum 13 caractères");

            RuleFor(x => x.City)
                .MaximumLength(100)
                .WithMessage("La ville doit faire au maximum 100 caractères");

            RuleFor(x => x.Nationality)
                .MaximumLength(100)
                .WithMessage("La nationalité doit faire au maximum 100 caractères");

            RuleFor(x => x.PostalCode)
                .MaximumLength(20)
                .WithMessage("Le code postal doit faire au maximum 20 caractères");
        }
    }
}