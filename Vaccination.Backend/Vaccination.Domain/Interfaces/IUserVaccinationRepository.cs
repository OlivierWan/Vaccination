using Vaccination.Domain.Entities;
using Vaccination.Domain.Shared;

namespace Vaccination.Domain.Interfaces
{
    public interface IUserVaccinationRepository
    {
        Task CreateVaccinationAsync(UserVaccination entity);

        Task DeleteVaccinationAsync(UserVaccination entity);

        Task<PagedList<UserVaccination>> GetAllUserVaccinationsAsync(string? orderBy, string orderDirection, int pageNumber, int pageSize, string? criteria, string userId);

        Task<UserVaccination?> GetUserVaccinationByIdAsync(Guid id);

        Task<IEnumerable<UserVaccination>> GetUserVaccinationsByUserIdAsync(string userId);
        
        Task<bool> IsUserVaccinationExists(string userId, Guid calendarVaccinationId);

        Task UpdateVaccinationAsync(UserVaccination entity);
    }
}