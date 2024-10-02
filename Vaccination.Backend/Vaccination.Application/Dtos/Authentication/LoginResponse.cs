namespace Vaccination.Application.Dtos.Authentication
{
    public record LoginResponse(
        DateTime? Expiration,
        string? RefreshToken,
        string? Token
        );
}