using Vaccination.Application.Dtos.Calendar;
using Vaccination.Domain.Shared;

namespace Vaccination.Application.Interfaces
{
    public interface ICalendarVaccinationService
    {
        Task<IEnumerable<CalendarVaccinationResponse>> GetAllAsync();

        Task<PagedList<CalendarVaccinationResponse>> GetAllWithPaginationAsync(GetFilteredCalendarVaccinationRequest getFilteredCalendarVaccinationRequest);
        Task<CalendarVaccinationResponse> GetByIdAsync(GetCalendarVaccinationByIdRequest getCalendarVaccinationByIdRequest);

        Task<IEnumerable<CalendarVaccinationResponse>> GetNextVaccines(string userId);
    }
}