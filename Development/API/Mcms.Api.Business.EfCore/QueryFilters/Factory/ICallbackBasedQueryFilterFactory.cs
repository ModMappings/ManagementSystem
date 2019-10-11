using System;
using System.Linq;
using Mcms.Api.Business.Core.Stores;

namespace Data.EFCore.QueryFilters.Factory
{
    public interface ICallbackBasedQueryFilterFactory<TEntity>
    {
        CallBackBasedQueryFilterFactory<TEntity> AddCallback(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> callback);

        IQueryFilter<TEntity> Build();
    }
}