using System;
using System.Collections.Generic;
using System.Linq;
using Mcms.Api.Business.Core.Stores;
using Microsoft.Extensions.Logging;

namespace Data.EFCore.QueryFilters.Factory
{
    public class CallBackBasedQueryFilterFactory<TEntity> : ICallbackBasedQueryFilterFactory<TEntity>
    {
        private readonly List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> _callbacks = new List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>();
        private readonly ILogger<CallBackBasedQueryFilterFactory<TEntity>> _logger;

        public CallBackBasedQueryFilterFactory(ILogger<CallBackBasedQueryFilterFactory<TEntity>> logger)
        {
            _logger = logger;
        }

        public CallBackBasedQueryFilterFactory<TEntity> AddCallback(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> callback)
        {
            this._callbacks.Add(callback);

            return this;
        }

        public IQueryFilter<TEntity> Build()
        {
            return new CallbackBasedQueryFilter<TEntity>(_callbacks);
        }
    }
}
