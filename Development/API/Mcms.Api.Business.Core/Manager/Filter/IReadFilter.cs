using System;
using Mcms.Api.Business.Core.Stores;
using Mcms.Api.Data.Poco.Models.Mapping.Component;

namespace Mcms.Api.Business.Core.Manager.Filter
{
    /// <summary>
    /// Data used during the reading of data from a manager.
    /// The filter data is turned into a <see cref="IQueryFilter{TEntity}"/>.
    /// </summary>
    public interface IReadFilter
    {
        /// <summary>
        /// Getter for the ID to filter on.
        /// </summary>
        Guid ById { get; }

        /// <summary>
        /// Getter for the mapping name regex to filter on.
        /// </summary>
        string ByMappingName { get; }

        /// <summary>
        /// Getter for the mapping regex to filter on.
        /// </summary>
        string ByMapping { get; }

        /// <summary>
        /// Getter for the component type to filter on.
        /// </summary>
        ComponentType ByComponentType { get; }

        /// <summary>
        /// Getter for the release name regex to filter on.
        /// </summary>
        string ByReleaseName { get; }

        /// <summary>
        /// Getter for the game version name regex to filter on.
        /// </summary>
        string ByGameVersionName { get; }
    }
}
