using AutoMapper;
using Vaccination.Application.Dtos.Calendar;
using Vaccination.Application.Interfaces;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Interfaces;
using Vaccination.Domain.Shared;

namespace Vaccination.Application.Services
{
    public class CalendarVaccinationService(IUnitOfWork unitOfWork, IMapper mapper) : ICalendarVaccinationService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<CalendarVaccinationResponse>> GetAllAsync()
        {
            IEnumerable<CalendarVaccination> calendarVaccinations = await _unitOfWork.CalendarVaccinations.GetAllAsync();
            return _mapper.Map<IEnumerable<CalendarVaccinationResponse>>(calendarVaccinations);
        }

        public async Task<PagedList<CalendarVaccinationResponse>> GetAllWithPaginationAsync(GetFilteredCalendarVaccinationRequest getFilteredCalendarVaccinationRequest)
        {
            PagedList<CalendarVaccination> calendarVaccinations = await _unitOfWork.CalendarVaccinations.GetAllCalendarVaccinationsAsync(
                                                                    getFilteredCalendarVaccinationRequest.PageNumber,
                                                                    getFilteredCalendarVaccinationRequest.PageSize,
                                                                    getFilteredCalendarVaccinationRequest.CriteriaSearch);
            return _mapper.Map<PagedList<CalendarVaccinationResponse>>(calendarVaccinations);
        }

        public async Task<CalendarVaccinationResponse> GetByIdAsync(GetCalendarVaccinationByIdRequest getCalendarVaccinationByIdRequest)
        {
            CalendarVaccination? calendarVaccination = await _unitOfWork.CalendarVaccinations.GetCalendarVaccinationByIdAsync(getCalendarVaccinationByIdRequest.Id);

            return calendarVaccination == null
                ? throw new KeyNotFoundException("Calendrier de vaccination non trouvé")
                : _mapper.Map<CalendarVaccinationResponse>(calendarVaccination);
        }

        public async Task<IEnumerable<CalendarVaccinationResponse>> GetNextVaccines(string userId)
        {
            IEnumerable<UserVaccination> userVaccins = await _unitOfWork.UserVaccinations.GetUserVaccinationsByUserIdAsync(userId);
            IEnumerable<CalendarVaccination> calendarVaccinations = await _unitOfWork.CalendarVaccinations.GetNextVaccinesAsync(userVaccins.Select(x => x.VaccineCalendarId));
            return _mapper.Map<IEnumerable<CalendarVaccinationResponse>>(calendarVaccinations);
        }
    }
}