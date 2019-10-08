using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping.Mappings;
using Data.Core.Models.Mapping.Metadata;

namespace Data.Core.Models.Mapping.Component
{
    public class VersionedComponent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public virtual GameVersion GameVersion { get; set; }

        [Required]
        public virtual Guid CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public virtual Component Component { get; set; }

        [Required]
        public virtual MetadataBase Metadata { get; set; }

        public virtual List<CommittedMapping> Mappings { get; set; }

        public virtual List<ProposedMapping> Proposals { get; set; }

        public virtual List<LockingEntry> LockedMappingTypes { get; set; }
    }
}
