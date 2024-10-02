namespace Vaccination.Application.Dtos.Calendar
{
    public record CalendarVaccinationResponse(
        Guid Id,
        string Name,
        string Description,
        int MonthAge,
        int MonthDelay
        );
}