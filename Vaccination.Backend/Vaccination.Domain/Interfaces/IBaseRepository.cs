using System.Linq.Expressions;

namespace Vaccination.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task CreateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<IQueryable<T>> FindAllAsync();

        Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
        Task UpdateAsync(T entity);
    }
}