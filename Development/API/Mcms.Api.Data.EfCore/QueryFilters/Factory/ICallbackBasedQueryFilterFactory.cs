using System;
using System.Linq;
using Mcms.Api.Data.Core.Stores;

namespace Mcms.Api.Data.EfCore.QueryFilters.Factory
{
    public interface ICallbackBasedQueryFilterFactory<TEntity>
    {
        CallBackBasedQueryFilterFactory<TEntity> AddCallback(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> callback);

        IDataQueryFilter<TEntity> Build();
    }
}