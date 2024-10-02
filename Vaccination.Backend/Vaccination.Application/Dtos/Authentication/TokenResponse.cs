namespace Vaccination.Application.Dtos.Authentication
{
    public record TokenResponse(
        string? RefreshToken,
        string? Token,
        DateTime? Expiration
        );
}