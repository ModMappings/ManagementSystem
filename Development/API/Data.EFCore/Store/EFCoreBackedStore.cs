using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.EFCore.Context;
using Mcms.Api.Business.Core.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.EFCore.Store
{
    public class EfCoreBackedStore<TEntity>
        : IStore<TEntity>
    {

        private readonly MCMSContext _context;
        private readonly ILogger<EfCoreBackedStore<TEntity>> _logger;

        public EfCoreBackedStore(MCMSContext context, ILogger<EfCoreBackedStore<TEntity>> logger)
        {
            //Check for null and save value.
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            //Disable the change tracker in the context.
            _logger.LogTrace(
                $"Creating new EfCoreBackedStore for: {nameof(TEntity)}. Disabling change tracking and lazy loading.");
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            _context.ChangeTracker.LazyLoadingEnabled = false;
        }

        public async Task CreateAsync(TEntity entityToCreate, CancellationToken? cancellationToken = null)
        {
            _logger.LogInformation($"About to create a new {nameof(TEntity)}, checking for valid entity state.");
            var entityEntry = _context.Entry(entityToCreate);
            if (entityEntry.State != EntityState.Detached)
            {
                _logger.LogCritical($"Failed to create a new {nameof(TEntity)}, the state of the given entity ({}");
                throw new InvalidOperationException("The given entity is either already a known entity or a uncommitted deleted entity.");
            }

            entityEntry.
        }

        public async Task<IQueryable<TEntity>> ReadAsync(IQueryFilter<TEntity> filter = null, CancellationToken? cancellationToken = null)
        {
            throw new System.NotImplementedException();
        }

        public async Task Update(TEntity entityToUpdate, CancellationToken? cancellationToken = null)
        {
            throw new System.NotImplementedException();
        }

        public async Task Delete(TEntity entityToDelete, CancellationToken? cancellationToken = null)
        {
            throw new System.NotImplementedException();
        }

        public bool HasPendingChanges { get; } =

        public async Task CommitChanges(CancellationToken? cancellationToken = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
