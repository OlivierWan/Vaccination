using Vaccination.Application.Dtos.Vaccination;
using Vaccination.Domain.Shared;

namespace Vaccination.Application.Interfaces
{
    public interface IUserVaccinationService
    {
        Task<CreatedUserVaccinationResponse> CreateVaccinationAsync(CreateUserVaccinationRequest createUserVaccinationRequest, string userId);

        Task<bool> DeleteVaccinationAsync(Guid id);

        Task<PagedList<UserVaccinationResponse>> GetAllUserVaccinationsAsync(GetFilteredUserVaccinationRequest getFilteredUserVaccinationRequest, string userId);

        Task<UpdateUserVaccinationResponse> UpdateVaccinationAsync(Guid id, UpdateUserVaccinationRequest updateUserVaccinationRequest);
    }
}