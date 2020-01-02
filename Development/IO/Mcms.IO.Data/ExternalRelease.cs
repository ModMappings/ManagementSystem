using System.Collections.Generic;

namespace Mcms.IO.Data
{
    /// <summary>
    /// Represents a release with data that is external with respect to Mcms.
    /// </summary>
    public class ExternalRelease
    {
        /// <summary>
        /// The packages which are part of this release.
        /// Might not include the root package, or any empty child package of the root, which implicitly have to exists, because
        /// a package is contained who's mapping results in the creation of these empty child packages and the root it self.
        ///
        /// This should generally not be empty, since else the release does not contain any data.
        /// </summary>
        public IList<ExternalPackage> Packages { get; set; } = new List<ExternalPackage>();

        /// <summary>
        /// The name of the release.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The game version this release is for.
        /// </summary>
        public string GameVersion { get; set; }

        /// <summary>
        /// The name of the mapping type this release is for.
        /// </summary>
        public string MappingType { get; set; }

        /// <summary>
        /// Indicates if this release is a snapshot.
        /// </summary>
        public bool IsSnapshot { get; set; } = false;
    }
}
