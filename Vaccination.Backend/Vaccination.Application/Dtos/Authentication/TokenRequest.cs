namespace Vaccination.Application.Dtos.Authentication
{
    public record TokenRequest(
          string AccessToken,
          string RefreshToken
      );
}