using Microsoft.EntityFrameworkCore;

namespace Vaccination.Domain.Shared
{
    /// <summary>
    /// Represents a paged list of items.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <summary>
    /// Represents a paged list of items.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// Gets the current page number.
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets the total number of items.
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether there is a previous page.
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page.
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="items">The items in the list.</param>
        /// <param name="count">The total number of items.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The page size.</param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
    }
}