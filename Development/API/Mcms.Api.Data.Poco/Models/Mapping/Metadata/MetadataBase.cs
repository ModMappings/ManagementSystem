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
        [ForeignKey("Id")]
        [Required]
        public virtual VersionedComponent VersionedComponent { get; set; }

        /// <summary>
        /// The id of the metadata, is equal to the id of the versioned component the metadata belongs to.
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
    }
}
