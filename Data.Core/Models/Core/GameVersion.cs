using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Data.Core.Models.Core
{
    public class GameVersion
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

        public bool IsPreRelease { get; set; }

        public bool IsSnapshot { get; set; }
    }
}
