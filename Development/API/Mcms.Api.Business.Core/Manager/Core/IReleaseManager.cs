using System;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Core.Release;

namespace Mcms.Api.Business.Core.Manager.Core
{
    /// <summary>
    /// Manager that handles interaction with releases.
    /// </summary>
    public interface IReleaseManager
    {

        /// <summary>
        /// Finds all releases with the given id.
        /// </summary>
        /// <param name="id">The given id to find releases by,</param>
        /// <returns>The task that looks up the releases that match the given id.</returns>
        Task<IQueryable<Release>> FindById(Guid id);

        /// <summary>
        /// Finds the releases who's name match the given regex.
        /// </summary>
        /// <param name="nameRegex">The regex to match the name of releases against.</param>
        /// <returns>The task that looks up the releases who's name match the given regex.</returns>
        Task<IQueryable<Release>> FindByName(string nameRegex);

        /// <summary>
        /// Finds all releases that at least have a single mapping in a mapping type who's name matches the given regex.
        /// </summary>
        /// <param name="mappingTypeNameRegex">The regex to match mapping type names against.</param>
        /// <returns>The task that looks up mapping types with at least a mapping that is part of a mapping type who's name matches the given regex.</returns>
        Task<IQueryable<Release>> FindByMappingType(string mappingTypeNameRegex);

        /// <summary>
        /// Finds all releases that are part of a game version who's name matches the given regex.
        /// </summary>
        /// <param name="gameVersionRegex">The regex to match game version names agents.</param>
        /// <returns>The task that looks up releases with a versioned mapping type for a game version who's name matches the given regex.</returns>
        Task<IQueryable<Release>> FindByGameVersion(string gameVersionRegex);

        /// <summary>
        /// <para>
        /// Finds all releases who match the given filter data.
        /// </para>
        /// <para>
        /// This methods finds the intersection between
        /// <see cref="FindById(Guid)"/> if the <paramref name="id"/> parameter is not <code>null</code>,
        /// <see cref="FindByName(string)"/> if the <paramref name="nameRegex"/> is not null,
        /// <see cref="FindByMappingType(string)"/> if the <paramref name="mappingTypeNameRegex"/> parameter is not <code>null</code>, and
        /// <see cref="FindByGameVersion(string)"/> if the <paramref name="gameVersionRegex"/> parameter is not <code>null</code>.
        /// </para>
        /// <para>
        /// This means that for a release to be returned it has to match all the provided filter data.
        /// </para>
        /// <para>
        /// </para>
        /// If no parameter is provided, or all are <code>null</code>, then all entries are returned.
        /// </summary>
        /// <param name="id">The given id to find releases by,</param>
        /// <param name="nameRegex">The regex to match the name of releases against.</param>
        /// <param name="mappingTypeNameRegex">The regex to match mapping type names against.</param>
        /// <param name="gameVersionRegex">The regex to match game version names agents.</param>
        /// <returns>The task that represents the lookup of releases that match the filter data, based on intersection.</returns>
        Task<IQueryable<Release>> FindUsingFilter(
            Guid? id = null,
            string nameRegex = null,
            string mappingTypeNameRegex = null,
            string gameVersionRegex = null
        );

        /// <summary>
        /// Creates a new release.
        /// The created release is not saved directly, but has to be saved separately.
        /// </summary>
        /// <param name="release">The release to create.</param>
        /// <returns>The task that describes the creation of the release.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either a known release or a uncommitted deleted release.</exception>
        Task CreateRelease(
            Release release
        );

        /// <summary>
        /// Updates an already existing release.
        /// Updates are not committed to the backing store directly, but have to be saved separately.
        /// </summary>
        /// <param name="release">The release to update.</param>
        /// <returns>The task that describes the updating of the manager with the data from the release.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either an unknown release (uncommitted deleted) or an uncommitted created release.</exception>
        Task UpdateRelease(
            Release release
        );

        /// <summary>
        /// Deletes an already existing release.
        /// Deletions are not committed to the backing store directly but have to be saved separately.
        /// </summary>
        /// <param name="release">The release to delete.</param>
        /// <returns>The task that describes the deletion of the release from the manager.</returns>
        Task DeleteRelease(
            Release release
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
