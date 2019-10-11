using System;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Core;

namespace Mcms.Api.Business.Core.Manager.Core
{
    /// <summary>
    /// Manager that handles interaction with mapping types.
    /// </summary>
    public interface IMappingTypeManager
    {

        /// <summary>
        /// Finds all mapping types with the given id.
        /// </summary>
        /// <param name="id">The given id to find mapping types by,</param>
        /// <returns>The task that looks up the mapping types that match the given id.</returns>
        Task<IQueryable<MappingType>> FindById(Guid id);

        /// <summary>
        /// Finds the mapping types who's name match the given regex.
        /// </summary>
        /// <param name="nameRegex">The regex to match the name of mapping types against.</param>
        /// <returns>The task that looks up the mapping types who's name match the given regex.</returns>
        Task<IQueryable<MappingType>> FindByName(string nameRegex);

        /// <summary>
        /// Finds all mapping types that at least have a single mapping in a release who's name matches the given regex.
        /// </summary>
        /// <param name="releaseNameRegex">The regex to match release names against.</param>
        /// <returns>The task that looks up mapping types with at least a mapping that is part of a release who's name matches the given regex.</returns>
        Task<IQueryable<MappingType>> FindByRelease(string releaseNameRegex);

        /// <summary>
        /// Finds all mapping types that are part of a game version who's name matches the given regex.
        /// </summary>
        /// <param name="gameVersionRegex">The regex to match game version names agents.</param>
        /// <returns>The task that looks up mapping types with a versioned mapping type for a game version who's name matches the given regex.</returns>
        Task<IQueryable<MappingType>> FindByGameVersion(string gameVersionRegex);

        /// <summary>
        /// <para>
        /// Finds all mapping types who match the given filter data.
        /// </para>
        /// <para>
        /// This methods finds the intersection between
        /// <see cref="FindById(Guid)"/> if the <paramref name="id"/> parameter is not <code>null</code>,
        /// <see cref="FindByName(string)"/> if the <paramref name="nameRegex"/> is not null,
        /// <see cref="FindByRelease(string)"/> if the <paramref name="releaseNameRegex"/> parameter is not <code>null</code>, and
        /// <see cref="FindByGameVersion(string)"/> if the <paramref name="gameVersionRegex"/> parameter is not <code>null</code>.
        /// </para>
        /// <para>
        /// This means that for a mapping type to be returned it has to match all the provided filter data.
        /// </para>
        /// <para>
        /// </para>
        /// If no parameter is provided, or all are <code>null</code>, then all entries are returned.
        /// </summary>
        /// <param name="id">The given id to find mapping types by,</param>
        /// <param name="nameRegex">The regex to match the name of mapping types against.</param>
        /// <param name="releaseNameRegex">The regex to match release names against.</param>
        /// <param name="gameVersionRegex">The regex to match game version names agents.</param>
        /// <returns>The task that represents the lookup of mapping types that match the filter data, based on intersection.</returns>
        Task<IQueryable<MappingType>> FindUsingFilter(
            Guid? id = null,
            string nameRegex = null,
            string releaseNameRegex = null,
            string gameVersionRegex = null
        );

        /// <summary>
        /// Creates a new mapping type.
        /// The created mapping type is not saved directly, but has to be saved separately.
        /// </summary>
        /// <param name="mappingType">The mapping type to create.</param>
        /// <returns>The task that describes the creation of the mapping type.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either a known mapping type or a uncommitted deleted mapping type.</exception>
        Task CreateMappingType(
            MappingType mappingType
        );

        /// <summary>
        /// Updates an already existing mapping type.
        /// Updates are not committed to the backing store directly, but have to be saved separately.
        /// </summary>
        /// <param name="mappingType">The mapping type to update.</param>
        /// <returns>The task that describes the updating of the manager with the data from the mapping type.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either an unknown mapping type (uncommitted deleted) or an uncommitted created mapping type.</exception>
        Task UpdateMappingType(
            MappingType mappingType
        );

        /// <summary>
        /// Deletes an already existing mapping type.
        /// Deletions are not committed to the backing store directly but have to be saved separately.
        /// </summary>
        /// <param name="mappingType">The mapping type to delete.</param>
        /// <returns>The task that describes the deletion of the mapping type from the manager.</returns>
        Task DeleteMappingType(
            MappingType mappingType
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
