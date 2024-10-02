using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using Vaccination.Application.Dtos.Vaccination;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Shared;

namespace Vaccination.Application.Mapper
{
    public class VaccinationProfile : Profile
    {
        public VaccinationProfile()
        {
            CreateMap<UserVaccination, CreatedUserVaccinationResponse>();

            CreateMap<UserVaccination, UpdateUserVaccinationResponse>();

            CreateMap<UserVaccination, UserVaccinationResponse>()
                .ForMember(dest => dest.VaccineName, opt => opt.MapFrom(src => src.VaccineCalendar.Name))
                .ForMember(dest => dest.VaccineDescription, opt => opt.MapFrom(src => src.Description))
                .ConstructUsing(dest => new UserVaccinationResponse(dest.Id, dest.VaccinationDate, dest.VaccineCalendarId, dest.VaccineCalendar.Name, dest.Description ?? string.Empty));

            CreateMap<CreateUserVaccinationRequest, UserVaccination>();
            CreateMap<UpdateUserVaccinationRequest, UserVaccination>();

            CreateMap<PagedList<UserVaccination>, PagedList<UserVaccinationResponse>>()
                .ConvertUsing<PagedListConverter<UserVaccination, UserVaccinationResponse>>();
        }
    }
}