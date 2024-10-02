using Microsoft.EntityFrameworkCore;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Interfaces;
using Vaccination.Domain.Shared;
using Vaccination.Infrastructure.Context;
using Vaccination.Infrastructure.Exceptions;
using Vaccination.Infrastructure.Extensions;

namespace Vaccination.Infrastructure.Repositories
{
    public class UserVaccinationRepository(VaccinationContext context) : BaseRepository<UserVaccination>(context), IUserVaccinationRepository
    {
        public async Task CreateVaccinationAsync(UserVaccination entity)
        {
            await CreateAsync(entity);
        }

        public async Task DeleteVaccinationAsync(UserVaccination entity)
        {
            await DeleteAsync(entity);
        }

        public async Task<PagedList<UserVaccination>> GetAllUserVaccinationsAsync(string? orderBy, string orderDirection, int pageNumber, int pageSize, string? criteria, string userId)
        {
            try
            {
                IQueryable<UserVaccination> query = await FindAllAsync();
                query = query.Include(x => x.User)
                             .Include(x => x.VaccineCalendar)
                             .Where(vu => vu.UserId.Equals(userId));

                if (!string.IsNullOrEmpty(criteria))
                {
                    query = query.Where(vu => vu.VaccineCalendar.Name.Contains(criteria) ||
                                              vu.VaccineCalendar.Description.Contains(criteria) ||
                                              (vu.Description != null && vu.Description.Contains(criteria)));
                }

                if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(orderDirection))
                {
                    if (orderDirection.Equals("asc"))
                    {
                        query = IQueryableExtensions.OrderBy(query, orderBy);
                    }
                    else if (orderDirection.Equals("desc"))
                    {
                        query = IQueryableExtensions.OrderByDescending(query, orderBy);
                    }
                }

                PagedList<UserVaccination> pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                return pagedList ?? new PagedList<UserVaccination>([], 0, pageNumber, pageSize);
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue lors de la récupération des vaccins de l'utilisateur");
            }
        }

        public async Task<UserVaccination?> GetUserVaccinationByIdAsync(Guid id)
        {
            try
            {
                IQueryable<UserVaccination> query = await FindByConditionAsync(vu => vu.Id.Equals(id));
                return await query
                    .Include(x => x.User)
                    .Include(x => x.VaccineCalendar)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue lors de la récupation d'un vaccin de l'utilisateur");
            }
        }

        public async Task<IEnumerable<UserVaccination>> GetUserVaccinationsByUserIdAsync(string userId)
        {
            try
            {
                IQueryable<UserVaccination> query = await FindByConditionAsync(x => x.UserId.Equals(userId));
                return await query.Include(x => x.User)
                                  .Include(x => x.VaccineCalendar)
                                  .ToListAsync();
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue lors la récupération des vaccins de l'utilisateur");
            }
        }

        public async Task<bool> IsUserVaccinationExists(string userId, Guid calendarVaccinationId)
        {
            var query = await FindByConditionAsync(x => x.UserId.Equals(userId) && x.VaccineCalendarId.Equals(calendarVaccinationId));
            return await query.AnyAsync();
        }

        public async Task UpdateVaccinationAsync(UserVaccination entity)
        {
            await UpdateAsync(entity);
        }
    }
}