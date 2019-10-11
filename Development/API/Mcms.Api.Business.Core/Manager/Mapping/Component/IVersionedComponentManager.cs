using System;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Mapping.Component;

namespace Mcms.Api.Business.Core.Manager.Mapping.Component
{
    /// <summary>
    /// Manager that handles interaction with versioned components.
    /// </summary>
    public interface IVersionedComponentManager
    {

        /// <summary>
        /// Finds all versioned components with the given id.
        /// </summary>
        /// <param name="id">The given id to find versioned components by,</param>
        /// <returns>The task that looks up the versioned components that match the given id.</returns>
        Task<IQueryable<VersionedComponent>> FindById(Guid id);

        /// <summary>
        /// Finds all versioned components with a given <see cref="ComponentType"/>.
        /// </summary>
        /// <param name="type">The type tho match versioned components type against.</param>
        /// <returns>That task that looks up the versioned components that match a given type.</returns>
        Task<IQueryable<VersionedComponent>> FindByType(ComponentType type);

        /// <summary>
        /// Finds all versioned components that are part of the component who's id matches the given one.
        /// </summary>
        /// <param name="componentId">The id of the component to find the versioned components for.</param>
        /// <returns>The task that looks up the versioned components that are part of the component who's id matches the given one.</returns>
        Task<IQueryable<VersionedComponent>> FindByComponentId(Guid componentId);

        /// <summary>
        /// Finds a all versioned components that have a mapping (either input or output) that matches the given
        /// mapping regex, additionally the mapping has to be of a type who's name matches the given regex as well.
        ///
        /// Unions <see cref="FindByOutputMapping(string, string)"/> and <see cref="FindByInputMapping(string, string)"/> together.
        /// </summary>
        /// <param name="mappingTypeNameRegex">The regex to match a mappings mapping type name against.</param>
        /// <param name="mappingRegex">The regex against which a mapping is matched, for which versioned components are found.</param>
        /// <returns>The task that looks up versioned components with a mapping that match the given mapping regex.</returns>
        Task<IQueryable<VersionedComponent>> FindByMapping(string mappingTypeNameRegex, string mappingRegex);

        /// <summary>
        /// Finds a all versioned components that have an output mapping that match the given
        /// mapping regex, additionally the mapping has to be of a type who's name matches the given regex as well.
        /// </summary>
        /// <param name="mappingTypeNameRegex">The regex to match a mappings mapping type name against.</param>
        /// <param name="mappingRegex">The regex against which an output mapping is matched, for which versioned components are found.</param>
        /// <returns>The task that looks up versioned components with an output mapping that match the given mapping regex.</returns>
        Task<IQueryable<VersionedComponent>> FindByOutputMapping(string mappingTypeNameRegex, string mappingRegex);

        /// <summary>
        /// Finds a all versioned components that have an input mapping that match the given
        /// mapping regex, additionally the mapping has to be of a type who's name matches the given regex as well.
        /// </summary>
        /// <param name="mappingTypeNameRegex">The regex to match a mappings mapping type name against.</param>
        /// <param name="mappingRegex">The regex against which an input mapping is matched, for which versioned components are found.</param>
        /// <returns>The task that looks up versioned components with an input mapping that match the given mapping regex.</returns>
        Task<IQueryable<VersionedComponent>> FindByInputMapping(string mappingTypeNameRegex, string mappingRegex);

        /// <summary>
        /// Finds all versioned components that are part of a release who's name matches the given regex.
        /// </summary>
        /// <param name="releaseNameRegex">The regex to match release names against.</param>
        /// <returns>The task that looks up versioned components with at least a mapping that is part of a release who's name matches the given regex.</returns>
        Task<IQueryable<VersionedComponent>> FindByRelease(string releaseNameRegex);

        /// <summary>
        /// Finds all versioned components that are part of a game version who's name matches the given regex.
        /// </summary>
        /// <param name="gameVersionRegex">The regex to match game version names agents.</param>
        /// <returns>The task that looks up versioned components with a versioned component for a game version who's name matches the given regex.</returns>
        Task<IQueryable<VersionedComponent>> FindByGameVersion(string gameVersionRegex);

        ///  <summary>
        /// <para>
        /// Finds all versioned components who match the given filter data.
        /// </para>
        /// <para>
        /// This methods finds the intersection between <see cref="FindById(Guid)"/> if the <paramref name="id"/> parameter is not <code>null</code>,
        /// <see cref="FindByType(ComponentType)"/> if the <paramref name="type"/> is not null,
        /// <see cref="FindByComponentId(Guid)"/> if the <paramref name="componentId"/> parameter is not <code>null</code>,
        /// <see cref="FindByMapping(string, string)"/> if either, the <paramref name="mappingTypeNameRegex"/> and or <paramref name="mappingRegex"/> parameter is not <code>null</code>,
        /// <see cref="FindByRelease(string)"/> if the <paramref name="releaseNameRegex"/> parameter is not <code>null</code>, and
        /// <see cref="FindByGameVersion(string)"/> if the <paramref name="gameVersionRegex"/> parameter is not <code>null</code>.
        /// </para>
        /// <para>
        /// This means that for a component to be returned it has to match all the provided filter data.
        /// </para>
        /// <para>
        /// </para>
        /// If no parameter is provided, or all are <code>null</code>, then all entries are returned.
        /// </summary>
        /// <param name="id">The given id to find versioned components by,</param>
        /// <param name="type">The <see cref="ComponentType"/> to match the versioned components against.</param>
        /// <param name="componentId">The id of the component to find the versioned components for.</param>
        /// <param name="mappingTypeNameRegex">The regex to match a mappings mapping type name against.</param>
        /// <param name="mappingRegex">The regex against which a mapping is matched, for which versioned components are found.</param>
        /// <param name="releaseNameRegex">The regex to match release names against.</param>
        /// <param name="gameVersionRegex">The regex to match game version names agents.</param>
        /// <returns>The task that represents the lookup of versioned components that match the filter data, based on intersection.</returns>
        Task<IQueryable<VersionedComponent>> FindUsingFilter(
            Guid? id = null,
            ComponentType? type = null,
            Guid? componentId = null,
            string mappingTypeNameRegex = null,
            string mappingRegex = null,
            string releaseNameRegex = null,
            string gameVersionRegex = null
        );

        /// <summary>
        /// Creates a new versioned component.
        /// The created versioned component is not saved directly, but has to be saved separately.
        /// </summary>
        /// <param name="versionedComponent">The versioned component to create.</param>
        /// <returns>The task that describes the creation of the versioned component.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either a known versioned component or a uncommitted deleted versioned component.</exception>
        Task CreateComponent(
            VersionedComponent versionedComponent
        );

        /// <summary>
        /// Updates an already existing versioned component.
        /// Updates are not committed to the backing store directly, but have to be saved separately.
        /// </summary>
        /// <param name="versionedComponent">The versioned component to update.</param>
        /// <returns>The task that describes the updating of the manager with the data from the versioned component.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either an unknown versioned component (uncommitted deleted) or an uncommitted created versioned component.</exception>
        Task UpdateComponent(
            VersionedComponent versionedComponent
        );

        /// <summary>
        /// Deletes an already existing versioned component.
        /// Deletions are not committed to the backing store directly but have to be saved separately.
        /// </summary>
        /// <param name="versionedComponent">The versioned component to delete.</param>
        /// <returns>The task that describes the deletion of the versioned component from the manager.</returns>
        Task DeleteComponent(
            VersionedComponent versionedComponent
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
