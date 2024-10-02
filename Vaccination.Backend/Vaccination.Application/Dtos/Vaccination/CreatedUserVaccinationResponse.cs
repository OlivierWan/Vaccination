namespace Vaccination.Application.Dtos.Vaccination
{
    public record CreatedUserVaccinationResponse(
        Guid Id, 
        DateOnly VaccinationDate,
        string? Description, 
        Guid VaccineCalendarId
        );
}