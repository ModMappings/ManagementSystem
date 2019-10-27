using System.Collections.Generic;

namespace Mcms.Api.Data.Poco.Models.Mapping.Metadata
{
    /// <summary>
    /// Metadata for a versioned component representing a package.
    /// </summary>
    public class PackageMetadata
        : MetadataBase
    {
        /// <summary>
        /// The parent package of this package.
        /// </summary>
        public virtual PackageMetadata Parent { get; set; }

        /// <summary>
        /// The packages which are part of the
        /// </summary>
        public virtual List<PackageMetadata> ChildPackages { get; set; }

        /// <summary>
        /// The classes which are part of this package.
        /// Might be empty.
        /// </summary>
        public virtual List<ClassMetadata> Classes { get; set; }
    }
}
