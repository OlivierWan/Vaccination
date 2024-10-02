namespace Vaccination.Application.Dtos.User
{
    public record UserDetailsResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string? SocialSecurityNumber,
        DateOnly? DateOfBirth,
        string? City,
        string? Nationality,
        string? Address,
        string? PostalCode,
        string? PhoneNumber
    );
}