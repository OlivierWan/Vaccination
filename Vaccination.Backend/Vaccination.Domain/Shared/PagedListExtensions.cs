using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vaccination.Domain.Shared
{
    public static class PagedListExtensions
    {
        /// <summary>
        /// Creates a paged list from a queryable source.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="source">The queryable source.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The paged list.</returns>
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            int count = await source.CountAsync();
            List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}