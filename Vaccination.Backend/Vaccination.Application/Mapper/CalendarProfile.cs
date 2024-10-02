using AutoMapper;
using Vaccination.Application.Dtos.Calendar;
using Vaccination.Domain.Entities;

namespace Vaccination.Application.Mapper
{
    public class CalendarProfile : Profile
    {
        public CalendarProfile()
        {
            CreateMap<CalendarVaccination, CalendarVaccinationResponse>();
            CreateMap<GetFilteredCalendarVaccinationRequest, CalendarVaccination>();
            CreateMap<GetCalendarVaccinationByIdRequest, CalendarVaccination>();
        }
    }
}