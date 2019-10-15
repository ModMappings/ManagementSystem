using System;
using System.Linq;
using Mcms.Api.Data.Core.Stores;

namespace Data.EFCore.QueryFilters.Factory
{
    public interface ICallbackBasedQueryFilterFactory<TEntity>
    {
        CallBackBasedQueryFilterFactory<TEntity> AddCallback(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> callback);

        IDataQueryFilter<TEntity> Build();
    }
}