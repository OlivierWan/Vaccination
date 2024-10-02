using Vaccination.Domain.Entities;
using Vaccination.Domain.Shared;

namespace Vaccination.Domain.Interfaces
{
    public interface ICalendarVaccinationRepository
    {
        Task<IEnumerable<CalendarVaccination>> GetAllAsync();

        Task<PagedList<CalendarVaccination>> GetAllCalendarVaccinationsAsync(int pageNumber, int pageSize, string? criteria);
        Task<CalendarVaccination?> GetCalendarVaccinationByIdAsync(Guid id);

        Task<IEnumerable<CalendarVaccination>> GetNextVaccinesAsync(IEnumerable<Guid> userVaccins);
    }
}