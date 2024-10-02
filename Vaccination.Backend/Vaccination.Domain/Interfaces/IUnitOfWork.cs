namespace Vaccination.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        ICalendarVaccinationRepository CalendarVaccinations { get; }
        IUserVaccinationRepository UserVaccinations { get; }

        Task SaveAsync();
    }
}