using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mcms.Api.Business.Core.Stores
{
    /// <summary>
    /// Represents a CRUD store to access the data within MCMS.
    /// </summary>
    /// <typeparam name="TEntity">The type that is stored within the store.</typeparam>
    public interface IStore<TEntity>
    {
        /// <summary>
        /// Creates a new uncommitted entity in the store.
        /// The creation has to be committed before it is visible to external systems.
        /// </summary>
        /// <param name="entityToCreate">The entity to create.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation, if none is supplied no cancellation is possible.</param>
        /// <returns>The task representing the creation of the entity.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called on either a known entity or a uncommitted deleted entity.</exception>
        Task CreateAsync(TEntity entityToCreate, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets all entities, that match the filter.
        /// If no filter is supplied then no filter is applied, and a query with all elements is returned.
        ///
        /// If there are uncommitted changes in this store, these might or might not be included in this query.
        /// </summary>
        /// <param name="filter">The filter to apply, if left to default (or null is supplied) no filter is applied.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation, if none is supplied no cancellation is possible.</param>
        /// <returns>The task that represents the query that represents the request to get the entities that match the filter.</returns>
        Task<IQueryable<TEntity>> ReadAsync(IQueryFilter<TEntity> filter = null, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates an entity within the store, and marks the changes as uncommitted.
        /// The update has to be committed before it is visible to external systems.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation, if none is supplied no cancellation is possible.</param>
        /// <returns>The task representing the update of the entity.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called on either an unknown entity (uncommitted deleted) or an uncommitted created entity.</exception>
        Task Update(TEntity entityToUpdate, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes a given entity from the store using its instance.
        /// The delete has to be committed before it is visible to external systems.
        /// </summary>
        /// <param name="entityToDelete">The entity instance to delete.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation, if none is supplied no cancellation is possible.</param>
        /// <returns></returns>
        Task Delete(TEntity entityToDelete, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Indicates if this store has uncommitted changes.
        /// </summary>
        bool HasPendingChanges { get; }

        /// <summary>
        /// Commits all changes made to the store to memory and makes them visible to external systems.
        /// This method short circuits if <see cref="HasPendingChanges"/> is <code>false</code>, and returns immediately.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the operation, if none is supplied no cancellation is possible.</param>
        /// <returns>The task representing the committing of the changes made.</returns>
        Task CommitChanges(CancellationToken? cancellationToken = null);
    }
}
