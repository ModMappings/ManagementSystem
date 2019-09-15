using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Core.Models.Mapping
{
    public class Component
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public ComponentType Type { get; set; }

        public virtual List<VersionedComponent> VersionedMappings { get; set; }
    }
}