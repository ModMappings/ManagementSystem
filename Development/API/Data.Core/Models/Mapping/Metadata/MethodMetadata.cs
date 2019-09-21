using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Core.Models.Mapping.Metadata
{
    public class MethodMetadata
        : ClassMemberComponentMetadataBase
    {
        public virtual List<ParameterMetadata> Parameters { get; set; }

        [Required]
        public string Descriptor { get; set; }
    }
}
