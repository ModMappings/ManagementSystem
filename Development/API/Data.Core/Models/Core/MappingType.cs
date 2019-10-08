using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Component;

namespace Data.Core.Models.Core
{
    /// <summary>
    /// Represents a single mapping type.
    /// So from OBF to TSRG or from TSRG to MCP for example.
    /// </summary>
    public class MappingType
    {

        /// <summary>
        /// The id of the type.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the mapping type.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The if of the user who created the type.
        /// </summary>
        [Required]
        public virtual Guid CreatedBy { get; set; }

        /// <summary>
        /// The moment the type was created within the system.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The releases that where made for this type.
        /// </summary>
        public virtual List<Release.Release> Releases { get; set; }

        /// <summary>
        /// The versioned components that are locked for this mapping.
        /// </summary>
        public virtual List<LockingEntry> LockedVersionedComponents { get; set; }
    }
}
