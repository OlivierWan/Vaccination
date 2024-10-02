namespace Vaccination.Application.Dtos.Vaccination
{
    public record UpdateUserVaccinationRequest(
        DateOnly VaccinationDate,
        string? Description
        );
}