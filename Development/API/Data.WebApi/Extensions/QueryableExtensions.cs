using System;
using System.Linq;
using Data.WebApi.Model;

namespace Data.WebApi.Extensions
{
    public static class QueryableExtensions
    {

        public static PagedList<TElement> AsPagedListWithSelect<TSource, TElement>(this IQueryable<TSource> source,
            Func<TSource, TElement> selectFunc, int pageIndex, int pageSize)
        {
            var totalSize = source.Count();
            return new PagedList<TElement>(source.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(selectFunc), pageIndex, pageSize, totalSize);
        }

        public static PagedList<TSource> AsPagedList<TSource>(this IQueryable<TSource> source, int pageIndex, int pageSize)
        {
            var totalSize = source.Count();
            return new PagedList<TSource>(source.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable(), pageIndex, pageSize, totalSize);
        }
    }
}
