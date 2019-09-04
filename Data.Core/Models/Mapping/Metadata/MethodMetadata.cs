using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Core.Models.Mapping.MetaData
{
    public class MethodMetadata
        : ClassMemberComponentMetadataBase
    {
        public virtual List<VersionedComponent> Parameters { get; set; }

        [Required]
        public string Descriptor { get; set; }
    }
}
