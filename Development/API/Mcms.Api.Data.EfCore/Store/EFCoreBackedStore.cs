using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mcms.Api.Data.Core.Stores;
using Mcms.Api.Data.EfCore.Context;
using Mcms.Api.Data.EfCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Mcms.Api.Data.EfCore.Store
{
    /// <summary>
    /// An EntityFramework core back implementation of the <see cref="IStore{TEntity}"/> interface.
    /// </summary>
    /// <typeparam name="TEntity">The entity type stored in the store.</typeparam>
    public class EfCoreBackedStore<TEntity>
        : IStore<TEntity> where TEntity : class
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

        public Task Create(TEntity entityToCreate, CancellationToken? cancellationToken = null)
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

        public async Task<IQueryable<TEntity>> ReadAsync(IDataQueryFilter<TEntity> filter = null, CancellationToken? cancellationToken = null)
        {
            _logger.LogDebug($"About to create a filtered query for: {nameof(TEntity)} using filter: {DescribeFilter(filter)}.");

            var unfilteredQueryable = _context.Query<TEntity>().AsQueryable();

            var filteredQueryable = unfilteredQueryable;
            if (filter != null)
            {
                filteredQueryable = filter.Apply(filteredQueryable);
            }

            _logger.LogDebug($"Created a filtered query for: {nameof(TEntity)} using filter {DescribeFilter(filter)}");
            return filteredQueryable;
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

        /// <summary>
        /// Describes a query filter using its <see cref="Object#toString()"/> method.
        /// Returns 'none' when the given query filter is null.
        /// </summary>
        /// <param name="dataQueryFilter">The describe the query filter.</param>
        /// <returns>A string describing the given query filter, or none if null is given.</returns>
        private string DescribeFilter(IDataQueryFilter<TEntity> dataQueryFilter) => dataQueryFilter == null ? "none" : dataQueryFilter.ToString();
    }
}
