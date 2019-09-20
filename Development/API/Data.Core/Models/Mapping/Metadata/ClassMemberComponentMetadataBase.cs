using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Core.Models.Mapping.MetaData
{
    public class ClassMemberComponentMetadataBase
        : VersionedComponentMetadataBase
    {
        [Required]
        public virtual ClassMetadata MemberOf { get; set; }

        [Required]
        public bool IsStatic { get; set; }
    }
}
