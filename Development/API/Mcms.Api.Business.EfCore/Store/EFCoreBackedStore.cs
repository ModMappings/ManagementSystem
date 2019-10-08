using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.EFCore.Context;
using Data.EFCore.Extensions;
using Mcms.Api.Business.Core.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

        public Task CreateAsync(TEntity entityToCreate, CancellationToken? cancellationToken = null)
        {
            _logger.LogDebug($"About to create a new {nameof(TEntity)}, checking for valid entity state.");
            var entityEntry = _context.Entry(entityToCreate);
            if (entityEntry.State != EntityState.Detached)
            {
                _logger.LogCritical($"Failed to create a new {nameof(TEntity)}, the state of the given entity ({entityEntry.GetKeyPropertiesAsString()}) is: {entityEntry.State.ToString()} and as such can not be added.");
                throw new InvalidOperationException($"Failed to create a new {nameof(TEntity)}, the state of the given entity ({entityEntry.GetKeyPropertiesAsString()}) is: {entityEntry.State.ToString()} and as such can not be added.");
            }

            _logger.LogTrace($"Updating the state of: {entityEntry.GetKeyPropertiesAsString()} to {EntityState.Added.ToString()}.");
            entityEntry.State = EntityState.Added;

            _logger.LogDebug($"Updated the state of: {entityEntry.GetKeyPropertiesAsString()} to {EntityState.Added.ToString()}");
            return Task.CompletedTask;
        }

        public async Task<IQueryable<TEntity>> ReadAsync(IQueryFilter<TEntity> filter = null, CancellationToken? cancellationToken = null)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(TEntity entityToUpdate, CancellationToken? cancellationToken = null)
        {
            _logger.LogDebug($"About to create a new {nameof(TEntity)}, checking for valid entity state.");
            var entityEntry = _context.Entry(entityToUpdate);
            if (entityEntry.State != EntityState.Unchanged || entityEntry.State != EntityState.Modified)
            {
                _logger.LogCritical($"Failed to update a {nameof(TEntity)}, the state of the given entity ({entityEntry.GetKeyPropertiesAsString()}) is: {entityEntry.State.ToString()} and as such can not be updated.");
                throw new InvalidOperationException($"Failed to update a {nameof(TEntity)}, the state of the given entity ({entityEntry.GetKeyPropertiesAsString()}) is: {entityEntry.State.ToString()} and as such can not be updated.");
            }

            _logger.LogTrace($"Updating the state of: {entityEntry.GetKeyPropertiesAsString()} to {EntityState.Modified.ToString()}.");
            entityEntry.State = EntityState.Modified;

            _logger.LogDebug($"Updated the state of: {entityEntry.GetKeyPropertiesAsString()} to {EntityState.Modified.ToString()}");
            return Task.CompletedTask;
        }

        public Task Delete(TEntity entityToDelete, CancellationToken? cancellationToken = null)
        {
            _logger.LogDebug($"About to create a new {nameof(TEntity)}, checking for valid entity state.");
            var entityEntry = _context.Entry(entityToDelete);
            if (entityEntry.State != EntityState.Unchanged || entityEntry.State != EntityState.Modified)
            {
                _logger.LogCritical($"Failed to delete a new {nameof(TEntity)}, the state of the given entity ({entityEntry.GetKeyPropertiesAsString()}) is: {entityEntry.State.ToString()} and as such can not be deleted.");
                throw new InvalidOperationException($"Failed to delete a new {nameof(TEntity)}, the state of the given entity ({entityEntry.GetKeyPropertiesAsString()}) is: {entityEntry.State.ToString()} and as such can not be deleted.");
            }

            _logger.LogTrace($"Updating the state of: {entityEntry.GetKeyPropertiesAsString()} to {EntityState.Deleted.ToString()}.");
            entityEntry.State = EntityState.Deleted;

            _logger.LogDebug($"Updated the state of: {entityEntry.GetKeyPropertiesAsString()} to {EntityState.Deleted.ToString()}");
            return Task.CompletedTask;
        }

        public bool HasPendingChanges => _context.GetService<IStateManager>()
            .Entries
            .Any(e => e.EntityState != EntityState.Unchanged && e.EntityState != EntityState.Detached);

        public async Task CommitChanges(CancellationToken? cancellationToken = null)
        {
            _logger.LogInformation($"Saving uncommitted changes of type: {nameof(TEntity)} to {_context.Database.GetDbConnection().Database}");

            if (!HasPendingChanges)
            {
                _logger.LogWarning($"Can not save uncommitted changes of type {nameof(TEntity)}. No changes are available to be saved.");
                return;
            }

            await _context.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
            _logger.LogInformation($"Saved all uncommitted changes of type: {nameof(TEntity)} to {_context.Database.GetDbConnection().Database}");
        }
    }
}
