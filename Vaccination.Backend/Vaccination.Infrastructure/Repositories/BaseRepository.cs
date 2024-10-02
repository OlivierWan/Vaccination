using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vaccination.Domain.Interfaces;
using Vaccination.Infrastructure.Context;
using Vaccination.Infrastructure.Exceptions;

namespace Vaccination.Infrastructure.Repositories
{
    public abstract class BaseRepository<T>(VaccinationContext context) : IBaseRepository<T> where T : class
    {
        protected readonly VaccinationContext context = context;

        public async Task CreateAsync(T entity)
        {
            try
            {
                await context.Set<T>().AddAsync(entity);
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue lors de la création dans la base de données");
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                context.Set<T>().Remove(entity);
                await Task.CompletedTask;
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue lors de la suppression de la donnée");
            }
        }

        public async Task<IQueryable<T>> FindAllAsync()
        {
            try
            {
                return await Task.FromResult(context.Set<T>().AsNoTracking());
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue lors de la récupération de toutes les données");
            }
        }

        public async Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await Task.FromResult(context.Set<T>().Where(expression).AsNoTracking());
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue lors de la recherche par condition d'une donnée");
            }
        }
        public async Task UpdateAsync(T entity)
        {
            try
            {
                context.Set<T>().Update(entity);
                await Task.CompletedTask;
            }
            catch (Exception)
            {
                throw new DatabaseException("Une erreur est survenue lors de la mise à jour d'une donnée");
            }
        }
    }
}