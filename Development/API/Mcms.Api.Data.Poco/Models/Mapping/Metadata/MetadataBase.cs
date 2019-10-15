using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mcms.Api.Data.Poco.Models.Mapping.Component;

namespace Mcms.Api.Data.Poco.Models.Mapping.Metadata
{
    /// <summary>
    /// A base class that handles metadata on versioned components.
    /// </summary>
    public class MetadataBase
    {
        /// <summary>
        /// The versioned component this is metadata for.
        /// </summary>
        [ForeignKey("VersionedComponentForeignKey")]
        [Required]
        public virtual VersionedComponent VersionedComponent { get; set; }

        /// <summary>
        /// The id of the versioned component this is metadata for.
        /// Doubles as the metadatas id, since there is a one to one relation ship between a metadata instance and a versioned component.
        /// </summary>
        [Key]
        [Required]
        public Guid VersionedComponentForeignKey { get; set; }
    }
}
