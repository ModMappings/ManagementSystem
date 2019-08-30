using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Data.Core.Models.Class;
using Data.Core.Models.Field;
using Data.Core.Models.Method;
using Data.Core.Models.Parameter;

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
        public User CreatedBy { get; set; }

        [Required]
        public GameVersion GameVersion { get; set; }

        public List<ClassReleaseMember> Classes { get; set; }

        public List<MethodReleaseMember> Methods { get; set; }

        public List<ParameterReleaseMember> Parameters { get; set; }

        public List<FieldReleaseMember> Fields { get; set; }
    }
}
