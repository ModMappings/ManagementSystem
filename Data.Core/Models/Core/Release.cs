using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Core.Models.Mapping;

namespace Data.Core.Models.Core
{
    public class Release
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public virtual User CreatedBy { get; set; }

        [Required]
        public virtual GameVersion GameVersion { get; set; }

        [Required]
        public virtual MappingType MappingType { get; set; }

        public virtual List<ReleaseComponent> Classes { get; set; }

        public virtual List<ReleaseComponent> Methods { get; set; }

        public virtual List<ReleaseComponent> Parameters { get; set; }

        public virtual List<ReleaseComponent> Fields { get; set; }
    }
}
