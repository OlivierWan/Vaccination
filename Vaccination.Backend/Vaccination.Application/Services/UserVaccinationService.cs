using AutoMapper;
using Vaccination.Application.Dtos.Vaccination;
using Vaccination.Application.Exceptions;
using Vaccination.Application.Interfaces;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Interfaces;
using Vaccination.Domain.Shared;

namespace Vaccination.Application.Services
{
    public class UserVaccinationService(IMapper mapper, IUnitOfWork unitOfWork) : IUserVaccinationService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CreatedUserVaccinationResponse> CreateVaccinationAsync(CreateUserVaccinationRequest createUserVaccinationRequest, string userId)
        {
            UserVaccination userVaccination = _mapper.Map<UserVaccination>(createUserVaccinationRequest);
            userVaccination.UserId = userId;

            bool vaccineCalendarExists = await _unitOfWork.UserVaccinations.IsUserVaccinationExists(userId, userVaccination.VaccineCalendarId);

            if (vaccineCalendarExists)
            {
                throw new DuplicateDataException("Le vaccin de l'utilisateur est déjà dans la base de données");
            }

            await _unitOfWork.UserVaccinations.CreateVaccinationAsync(userVaccination);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<CreatedUserVaccinationResponse>(userVaccination);
        }

        public async Task<bool> DeleteVaccinationAsync(Guid id)
        {
            UserVaccination? userVaccination = await _unitOfWork.UserVaccinations.GetUserVaccinationByIdAsync(id) ?? throw new NotFoundException("Le vaccin de l'utilisateur n'existe pas");

            await _unitOfWork.UserVaccinations.DeleteVaccinationAsync(userVaccination);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<PagedList<UserVaccinationResponse>> GetAllUserVaccinationsAsync(GetFilteredUserVaccinationRequest getFilteredUserVaccinationRequest, string userId)
        {
            if (getFilteredUserVaccinationRequest.PageNumber < 1)
            {
                throw new PaginationException("PageNumber doit être plus grand que 0");
            }

            if (getFilteredUserVaccinationRequest.PageSize < 1)
            {
                throw new PaginationException("PageSize doit être plus grand que 0");
            }

            // Mapping the orderBy parameter to the corresponding column in the database
            getFilteredUserVaccinationRequest = getFilteredUserVaccinationRequest with
            {
                OrderBy = getFilteredUserVaccinationRequest.OrderBy.ToUpper() switch
                {
                    "VACCINENAME" => "VaccineCalendar.Name",
                    "VACCINEDESCRIPTION" => "VaccineCalendar.Description",
                    _ => getFilteredUserVaccinationRequest.OrderBy
                }
            };


            PagedList<UserVaccination> userVaccinations = await _unitOfWork.UserVaccinations.GetAllUserVaccinationsAsync(
                                        getFilteredUserVaccinationRequest.OrderBy,
                                        getFilteredUserVaccinationRequest.OrderDirection,
                                        getFilteredUserVaccinationRequest.PageNumber,
                                        getFilteredUserVaccinationRequest.PageSize,
                                        getFilteredUserVaccinationRequest.CriteriaSearch,
                                        userId);

            return _mapper.Map<PagedList<UserVaccinationResponse>>(userVaccinations);
        }

        public async Task<UpdateUserVaccinationResponse> UpdateVaccinationAsync(Guid id, UpdateUserVaccinationRequest updateUserVaccinationRequest)
        {
            UserVaccination? userVaccination = await _unitOfWork.UserVaccinations.GetUserVaccinationByIdAsync(id) ?? throw new NotFoundException("Le vaccin de l'utilisateur n'existe pas");

            // Store the values of CreatedBy and CreatedOnUtc
            Guid createdBy = userVaccination.CreatedBy;
            DateTime createdOnUtc = userVaccination.CreatedOnUtc;

            _mapper.Map(updateUserVaccinationRequest, userVaccination);

            // Restore the values of CreatedBy and CreatedOnUtc
            userVaccination.CreatedBy = createdBy;
            userVaccination.CreatedOnUtc = createdOnUtc;

            await _unitOfWork.UserVaccinations.UpdateVaccinationAsync(userVaccination);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<UpdateUserVaccinationResponse>(userVaccination);
        }
    }
}