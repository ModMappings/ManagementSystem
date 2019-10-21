using System;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Mapping.Component;

namespace Mcms.Api.Business.Core.Manager.Mapping.Component
{
    public interface IComponentManager
    {
        /// <summary>
        /// <para>
        /// Finds all components who match the given filter data.
        /// </para>
        /// <para>
        /// This means that for a component to be returned it has to match all the provided filter data.
        /// </para>
        /// <para>
        /// </para>
        /// If no parameter is provided, or all are <code>null</code>, then all entries are returned.
        /// </summary>
        /// <param name="id">The given id to find components by,</param>
        /// <param name="type">The <see cref="ComponentType"/> to match the components against.</param>
        /// <param name="mappingTypeNameRegex">The regex to match a mappings mapping type name against.</param>
        /// <param name="mappingRegex">The regex against which a mapping is matched, for which components are found.</param>
        /// <param name="releaseNameRegex">The regex to match release names against.</param>
        /// <param name="gameVersionRegex">The regex to match game version names agents.</param>
        /// <returns>The task that represents the lookup of components that match the filter data, based on intersection.</returns>
        Task<IQueryable<Data.Poco.Models.Mapping.Component.Component>> FindUsingFilter(
            Guid? id = null,
            ComponentType? type = null,
            string mappingTypeNameRegex = null,
            string mappingRegex = null,
            string releaseNameRegex = null,
            string gameVersionRegex = null
        );

        /// <summary>
        /// Creates a new component.
        /// The created component is not saved directly, but has to be saved separately.
        /// </summary>
        /// <param name="component">The component to create.</param>
        /// <returns>The task that describes the creation of the component.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either a known component or a uncommitted deleted component.</exception>
        Task CreateComponent(
            Data.Poco.Models.Mapping.Component.Component component
        );

        /// <summary>
        /// Updates an already existing component.
        /// Updates are not committed to the backing store directly, but have to be saved separately.
        /// </summary>
        /// <param name="component">The component to update.</param>
        /// <returns>The task that describes the updating of the manager with the data from the component.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either an unknown component (uncommitted deleted) or an uncommitted created component.</exception>
        Task UpdateComponent(
            Data.Poco.Models.Mapping.Component.Component component
        );

        /// <summary>
        /// Deletes an already existing component.
        /// Deletions are not committed to the backing store directly but have to be saved separately.
        /// </summary>
        /// <param name="component">The component to delete.</param>
        /// <returns>The task that describes the deletion of the component from the manager.</returns>
        Task DeleteComponent(
            Data.Poco.Models.Mapping.Component.Component component
        );

        /// <summary>
        /// Indicates if the manager has pending changes that need to be saved.
        /// </summary>
        bool HasPendingChanges { get; }

        /// <summary>
        /// Saves the pending changes.
        /// If <see cref="HasPendingChanges"/> is <code>false</code> then this method short circuits into a <code>noop;</code>
        /// </summary>
        /// <returns>The task that describes the saving of the pending changes in this manager.</returns>
        Task SaveChanges();
    }
}
