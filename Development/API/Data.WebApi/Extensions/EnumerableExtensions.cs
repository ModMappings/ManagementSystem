using System;
using System.Collections.Generic;
using System.Linq;
using Data.WebApi.Model.Read.Core;

namespace Data.WebApi.Extensions
{
    public static class EnumerableExtensions
    {

        public static PagedList<TElement> AsPagedListWithSelect<TSource, TElement>(this IEnumerable<TSource> source,
            Func<TSource, TElement> selectFunc, int pageIndex, int pageSize)
        {
            var totalSize = source.Count();
            return new PagedList<TElement>(source.Skip(pageSize * pageIndex).Take(pageSize).Select(selectFunc), pageIndex, pageSize, totalSize);
        }
    }
}
