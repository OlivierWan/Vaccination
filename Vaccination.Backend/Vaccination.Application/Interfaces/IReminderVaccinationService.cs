using Vaccination.Application.Dtos.Reminder;

namespace Vaccination.Application.Interfaces
{
    public interface IReminderVaccinationService
    {
        Task<IEnumerable<ReminderVaccinationResponse>> GetUpcomingRemindersAsync(string userId);
    }
}
