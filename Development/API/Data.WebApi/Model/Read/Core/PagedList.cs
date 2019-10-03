using System;
using System.Collections.Generic;

namespace Data.WebApi.Model.Read.Core
{
    /// <summary>
    /// Represents a part of an existing enumerable.
    /// Indicates also how many pages are still available.
    /// </summary>
    /// <typeparam name="TElement">The type that is contained in the paged list.</typeparam>
    public class PagedList<TElement>
    {
        public PagedList(IEnumerable<TElement> data, int index, int pageSize, int totalElementCount)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Index = index;
            PageSize = pageSize;
            TotalPageCount = (int) Math.Ceiling((double) pageSize / totalElementCount);
            TotalElementCount = totalElementCount;
        }

        /// <summary>
        /// The data contained in the page.
        /// </summary>
        public IEnumerable<TElement> Data { get; }

        /// <summary>
        /// The 0-based index of the page.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The size of the page.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// The total amount of pages with the current page size.
        /// </summary>
        public int TotalPageCount { get; }

        /// <summary>
        /// The total amount of elements in the original enumerable.
        /// </summary>
        public int TotalElementCount { get; }
    }
}
