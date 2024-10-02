using Vaccination.Application.Dtos.Authentication;

namespace Vaccination.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);

        Task<TokenResponse> RefreshTokenAsync(TokenRequest tokenRequest);

        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
    }
}