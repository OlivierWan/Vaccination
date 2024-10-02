namespace Vaccination.Application.Dtos.Authentication
{
    public record RegisterRequest(
         string Email,
         string FirstName,
         string LastName,
         string Password
     );
}