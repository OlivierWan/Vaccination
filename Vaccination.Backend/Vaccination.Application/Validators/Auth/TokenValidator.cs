using FluentValidation;
using Vaccination.Application.Dtos.Authentication;

namespace Vaccination.Application.Validators.Auth
{
    public class TokenValidator : AbstractValidator<TokenRequest>
    {
        public TokenValidator()
        {

            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh token est requis");

            RuleFor(x => x.AccessToken)
                .NotEmpty()
                .WithMessage("Access token est requis");


        }
    }
}
