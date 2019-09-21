using System.ComponentModel.DataAnnotations;

namespace Data.Core.Models.Mapping.Metadata
{
    public class VersionedComponentMetadataBase
    {
        [Key]
        public virtual VersionedComponent Component { get; set; }
    }
}
