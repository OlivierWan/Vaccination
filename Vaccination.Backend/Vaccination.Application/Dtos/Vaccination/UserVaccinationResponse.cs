namespace Vaccination.Application.Dtos.Vaccination
{
    public record UserVaccinationResponse(
         Guid Id,
         DateOnly VaccinationDate,
         Guid VaccineCalendarId,
         string VaccineName,
         string VaccineDescription
    );
}