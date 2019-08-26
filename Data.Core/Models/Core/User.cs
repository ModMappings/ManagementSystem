using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Core.Models.Core
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool CanEdit { get; set; }

        public bool CanReview { get; set; }

        public bool CanCommit { get; set; }

        public bool CanRelease { get; set; }

        public bool CanCreateGameVersions { get; set; }
    }
}
