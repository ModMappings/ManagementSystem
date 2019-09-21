using System.ComponentModel.DataAnnotations;

namespace Data.Core.Models.Mapping.Metadata
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
