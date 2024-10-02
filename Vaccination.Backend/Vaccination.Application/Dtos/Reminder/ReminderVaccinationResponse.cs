namespace Vaccination.Application.Dtos.Reminder
{
    public record ReminderVaccinationResponse(
        string? Name,
        string? Description,
        string Message,
        int MonthAge,
        Guid CalendarVaccinationId
        );
}