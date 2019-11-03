using System;
using System.Collections.Generic;
using System.Linq;
using Mcms.Api.WebApi.Http.Model;

namespace Mcms.Api.WebApi.Http.Extensions
{
    public static class EnumerableExtensions
    {

        public static PagedList<TElement> AsPagedListWithSelect<TSource, TElement>(this IEnumerable<TSource> source,
            Func<TSource, TElement> selectFunc, int pageIndex, int pageSize)
        {
            var totalSize = source.Count();
            return new PagedList<TElement>(source.PerformPaging(pageIndex, pageSize).Select(selectFunc), pageIndex, pageSize, totalSize);
        }

        public static IEnumerable<TElement> PerformPaging<TElement>(this IEnumerable<TElement> source, int pageIndex,
            int pageSize)
        {
            return source.Skip(pageSize * pageIndex).Take(pageSize);
        }

        public static PagedList<TElement> WrapInPaging<TElement>(this IEnumerable<TElement> source, int pageIndex, int totalSize)
        {
            return new PagedList<TElement>(source, pageIndex, source.Count(), totalSize);
        }
    }
}
