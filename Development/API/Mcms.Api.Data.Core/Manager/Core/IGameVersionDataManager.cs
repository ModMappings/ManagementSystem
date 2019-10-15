using System;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Core;

namespace Mcms.Api.Data.Core.Manager.Core
{
    /// <summary>
    /// Manager that handles interaction with game versions.
    /// </summary>
    public interface IGameVersionDataManager
    {

        /// <summary>
        /// Finds all game versions with the given id.
        /// </summary>
        /// <param name="id">The given id to find game versions by.</param>
        /// <returns>The task that looks up the game versions that match the given id.</returns>
        Task<IQueryable<GameVersion>> FindById(Guid id);

        /// <summary>
        /// Finds the game versions who's name match the given regex.
        /// </summary>
        /// <param name="nameRegex">The regex to match the name of game versions against.</param>
        /// <returns>The task that looks up the game versions who's name match the given regex.</returns>
        Task<IQueryable<GameVersion>> FindByName(string nameRegex);

        /// <summary>
        /// <para>
        /// Finds all game versions who match the given filter data.
        /// </para>
        /// <para>
        /// This methods finds the intersection between
        /// <see cref="FindById(Guid)"/> if the <paramref name="id"/> parameter is not <code>null</code>,
        /// <see cref="FindByName(string)"/> if the <paramref name="nameRegex"/> is not null.
        /// </para>
        /// <para>
        /// This means that for a game version to be returned it has to match all the provided filter data.
        /// </para>
        /// <para>
        /// </para>
        /// If no parameter is provided, or all are <code>null</code>, then all entries are returned.
        /// </summary>
        /// <param name="id">The given id to find game versions by,</param>
        /// <param name="nameRegex">The regex to match the name of game versions against.</param>
        /// <returns>The task that represents the lookup of game versions that match the filter data, based on intersection.</returns>
        Task<IQueryable<GameVersion>> FindUsingFilter(
            Guid? id = null,
            string nameRegex = null
        );

        /// <summary>
        /// Creates a new game version.
        /// The created game version is not saved directly, but has to be saved separately.
        /// </summary>
        /// <param name="gameVersion">The game version to create.</param>
        /// <returns>The task that describes the creation of the game version.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either a known game version or a uncommitted deleted game version.</exception>
        Task CreateGameVersion(
            GameVersion gameVersion
        );

        /// <summary>
        /// Updates an already existing game version.
        /// Updates are not committed to the backing store directly, but have to be saved separately.
        /// </summary>
        /// <param name="gameVersion">The game version to update.</param>
        /// <returns>The task that describes the updating of the manager with the data from the game version.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either an unknown game version (uncommitted deleted) or an uncommitted created game version.</exception>
        Task UpdateGameVersion(
            GameVersion gameVersion
        );

        /// <summary>
        /// Deletes an already existing game version.
        /// Deletions are not committed to the backing store directly but have to be saved separately.
        /// </summary>
        /// <param name="gameVersion">The game version to delete.</param>
        /// <returns>The task that describes the deletion of the game version from the manager.</returns>
        Task DeleteGameVersion(
            GameVersion gameVersion
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
