using System.Linq;

namespace Mcms.Api.Data.Core.Stores
{
    /// <summary>
    /// Used to filter a given queryable dynamically when it is requested in a store.
    /// </summary>
    /// <typeparam name="TEntity">The entity in the queryable that is being filtered.</typeparam>
    public interface IDataQueryFilter<TEntity>
    {
        /// <summary>
        /// Applies the filter to given input.
        /// </summary>
        /// <param name="input">The input query to apply the filter to.</param>
        /// <returns>The filtered queryable.</returns>
        IQueryable<TEntity> Apply(IQueryable<TEntity> input);
    }
}
