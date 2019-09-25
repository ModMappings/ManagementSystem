using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Core.Models.Core;

namespace Data.Core.Models.Mapping
{
    public abstract class MappingEntryBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public virtual VersionedComponent VersionedComponent { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string InputMapping { get; set; }

        [Required]
        public string OutputMapping { get; set; }

        public string Documentation { get; set; }

        [Required]
        public Distribution Distribution { get; set; }

        [Required]
        public virtual MappingType MappingType { get; set; }
    }
}
