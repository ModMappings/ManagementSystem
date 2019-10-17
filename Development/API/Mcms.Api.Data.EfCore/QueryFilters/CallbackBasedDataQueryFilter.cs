using System;
using System.Collections.Generic;
using System.Linq;
using Mcms.Api.Data.Core.Stores;

namespace Mcms.Api.Data.EfCore.QueryFilters
{
    public class CallbackBasedDataQueryFilter<TEntity>
        : IDataQueryFilter<TEntity>
    {

        private readonly List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> _callBacks;

        public CallbackBasedDataQueryFilter(List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> callBacks)
        {
            _callBacks = callBacks;
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> input)
        {
            return _callBacks.Aggregate(input, (current, callBack) => callBack(current));
        }
    }
}
