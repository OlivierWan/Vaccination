using Microsoft.EntityFrameworkCore;
using Vaccination.Domain.Entities;
using Vaccination.Domain.Interfaces;
using Vaccination.Domain.Shared;
using Vaccination.Infrastructure.Context;
using Vaccination.Infrastructure.Exceptions;

namespace Vaccination.Infrastructure.Repositories
{
    public class CalendarVaccinationRepository(VaccinationContext context) : BaseRepository<CalendarVaccination>(context), ICalendarVaccinationRepository
    {
        public async Task<IEnumerable<CalendarVaccination>> GetAllAsync()
        {
            try
            {
                IQueryable<CalendarVaccination> query = await FindAllAsync();
                return query.OrderBy(vu => vu.MonthAge);
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue lors de la récupération du calendrier des vaccins");
            }
        }

        public async Task<PagedList<CalendarVaccination>> GetAllCalendarVaccinationsAsync(int pageNumber, int pageSize, string? criteria)
        {
            try
            {
                IQueryable<CalendarVaccination> query = await FindAllAsync();

                if (!string.IsNullOrEmpty(criteria))
                {
                    query = query.Where(vu => vu.Name.Contains(criteria) ||
                                              vu.Description.Contains(criteria) ||
                                              vu.MonthAge.ToString().Contains(criteria))
                        .OrderBy(vu => vu.MonthAge);
                }

                PagedList<CalendarVaccination> pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                return pagedList ?? new PagedList<CalendarVaccination>([], 0, pageNumber, pageSize);
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue lors de la récupération du calendrier des vaccins");
            }
        }

        public async Task<CalendarVaccination?> GetCalendarVaccinationByIdAsync(Guid id)
        {
            try
            {
                IQueryable<CalendarVaccination> query = await FindByConditionAsync(vu => vu.Id.Equals(id));
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue lors de la récupération du vaccin");
            }
        }

        public async Task<IEnumerable<CalendarVaccination>> GetNextVaccinesAsync(IEnumerable<Guid> userVaccins)
        {
            try
            {
                IQueryable<CalendarVaccination> query = await FindAllAsync();
                return query.Where(vu => !userVaccins.Contains(vu.Id)).OrderBy(vu => vu.MonthAge);
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue lors de la récupération des prochains vaccins");
            }
        }
    }
}