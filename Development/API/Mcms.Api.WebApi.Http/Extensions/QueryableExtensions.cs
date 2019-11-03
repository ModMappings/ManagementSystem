using System;
using System.Linq;
using Mcms.Api.WebApi.Http.Model;

namespace Mcms.Api.WebApi.Http.Extensions
{
    public static class QueryableExtensions
    {

        public static PagedList<TElement> AsPagedListWithSelect<TSource, TElement>(this IQueryable<TSource> source,
            Func<TSource, TElement> selectFunc, int pageIndex, int pageSize)
        {
            var totalSize = source.Count();
            return new PagedList<TElement>(source.PerformPaging(pageIndex, pageSize).AsEnumerable().Select(selectFunc), pageIndex, pageSize, totalSize);
        }

        public static PagedList<TSource> AsPagedList<TSource>(this IQueryable<TSource> source, int pageIndex, int pageSize)
        {
            var totalSize = source.Count();
            return new PagedList<TSource>(source.PerformPaging(pageIndex, pageSize).AsEnumerable(), pageIndex, pageSize, totalSize);
        }

        public static IQueryable<TElement> PerformPaging<TElement>(this IQueryable<TElement> source, int pageIndex,
            int pageSize)
        {
            return source.Skip(pageSize * pageIndex).Take(pageSize);
        }
    }
}
