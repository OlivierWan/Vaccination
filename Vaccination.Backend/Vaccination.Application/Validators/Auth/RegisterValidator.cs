using FluentValidation;
using Vaccination.Application.Dtos.Authentication;

namespace Vaccination.Application.Validators.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("L'email est requis")
                .EmailAddress()
                .WithMessage("L'email n'est pas valide");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Le mot de passe est requis")
                .MinimumLength(10)
                .WithMessage("Le mot de passe doit faire au moins 10 caractères")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{10,}$")
                .WithMessage("Le mot de passe doit contenir au moins une lettre minuscule, une lettre majuscule, un chiffre, et un caractère spécial.");


            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Le prénom est requis")
                .MaximumLength(150)
                .WithMessage("Le prénom doit faire moins de 150 caractères");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Le nom de famille est requis")
                .MaximumLength(150)
                .WithMessage("Le nom de famille doit faire moins de 150 caractères");
        }
    }
}