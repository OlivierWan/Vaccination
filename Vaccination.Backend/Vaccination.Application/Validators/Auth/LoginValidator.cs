using FluentValidation;
using Vaccination.Application.Dtos.Authentication;

namespace Vaccination.Application.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email est requis")
                .EmailAddress()
                .WithMessage("Email n'est pas une adresse valide");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Mot de passe est requis")
                .MinimumLength(10)
                .WithMessage("Mot de passe doit faire au moins 10 caractères");
        }
    }
}
