using System;

namespace DataAccess.Core
{
    /// <summary>
    /// Represents a single package within a remapped project.
    /// </summary>
    public interface IPackage
    {
        /// <summary>
        /// The id of the package.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The parent package, if it exists.
        /// Null if otherwise.
        /// </summary>
        IPackage Parent { get; set; }

        /// <summary>
        /// The name of the package.
        /// </summary>
        string Name { get; set; }
    }
}
