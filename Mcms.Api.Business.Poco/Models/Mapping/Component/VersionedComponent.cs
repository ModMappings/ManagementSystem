using Mcms.Api.Business.Poco.Models.Core;
using Mcms.Api.Business.Poco.Models.Mapping.Mappings;
using Mcms.Api.Business.Poco.Models.Mapping.Metadata;

namespace Mcms.Api.Business.Poco.Models.Mapping.Component
{
    /// <summary>
    /// Represents a single mappable component for a given game version.
    /// </summary>
    public class VersionedComponent
    {
        /// <summary>
        /// The id of the versioned component.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// The game version for this versioned component.
        /// </summary>
        [Required]
        public virtual GameVersion GameVersion { get; set; }

        /// <summary>
        /// The id of the user who created the versioned component.
        /// </summary>
        [Required]
        public virtual Guid CreatedBy { get; set; }

        /// <summary>
        /// The moment the versioned component was created.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The overall component this versioned component is an versioned instance for.
        /// </summary>
        [Required]
        public virtual Component Component { get; set; }

        /// <summary>
        /// The metadata for this versioned component.
        /// </summary>
        [Required]
        public virtual MetadataBase Metadata { get; set; }

        /// <summary>
        /// The committed mappings for this versioned component.
        /// </summary>
        public virtual List<CommittedMapping> Mappings { get; set; }

        /// <summary>
        /// The proposals for this versioned component.
        /// </summary>
        public virtual List<ProposedMapping> Proposals { get; set; }

        /// <summary>
        /// The locked mapping types for this versioned component.
        /// </summary>
        public virtual List<LockingEntry> LockedMappingTypes { get; set; }
    }
}
