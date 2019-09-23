using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Core.Models.Mapping;

namespace Data.Core.Models.Core
{
    /// <summary>
    /// Represents a single mapping type.
    /// So from OBF to TSRG or from TSRG to MCP for example.
    /// </summary>
    public class MappingType
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual Guid CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public virtual List<Release> Releases { get; set; }

        public virtual List<LockingEntry> LockedVersionedComponents { get; set; }
    }
}
