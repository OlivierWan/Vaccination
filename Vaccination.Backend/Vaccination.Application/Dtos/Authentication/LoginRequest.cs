namespace Vaccination.Application.Dtos.Authentication
{
    public record LoginRequest(
        string Email,
        string Password
        );
}