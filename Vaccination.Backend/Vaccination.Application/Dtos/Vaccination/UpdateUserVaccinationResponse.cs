namespace Vaccination.Application.Dtos.Vaccination
{
    public record UpdateUserVaccinationResponse(
        Guid Id,
        DateOnly VaccinationDate,
        string? Description,
        Guid VaccineCalendarId
        );
}