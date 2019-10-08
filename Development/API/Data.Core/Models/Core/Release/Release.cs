using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Core.Models.Comments;

namespace Data.Core.Models.Core.Release
{
    /// <summary>
    /// Represents a single release of mapping data, for a given mapping type.
    /// </summary>
    public class Release
    {
        /// <summary>
        /// The id of the release.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the release.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The moment the release was created within the system.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The id of the user who created the release.
        /// </summary>
        [Required]
        public virtual Guid CreatedBy { get; set; }

        /// <summary>
        /// The version that the release was created for.
        /// </summary>
        [Required]
        public virtual GameVersion GameVersion { get; set; }

        /// <summary>
        /// The mapping type the release was created for.
        /// </summary>
        [Required]
        public virtual MappingType MappingType { get; set; }

        /// <summary>
        /// The release components that are part of this release.
        /// Each one represents a single mapping in this release.
        /// </summary>
        public virtual List<ReleaseComponent> Components { get; set; }

        /// <summary>
        /// Indicates if this release is a snapshot within the releases for the given mapping type.
        /// </summary>
        public bool IsSnapshot { get; set; } = false;

        /// <summary>
        /// The comments made on the release.
        /// </summary>
        public virtual List<Comment> Comments { get; set; }
    }
}
