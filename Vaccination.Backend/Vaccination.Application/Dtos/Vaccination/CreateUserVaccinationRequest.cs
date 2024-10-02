namespace Vaccination.Application.Dtos.Vaccination
{
    public record CreateUserVaccinationRequest(
        DateOnly VaccinationDate,
        string? Description,
        Guid VaccineCalendarId
        );
}