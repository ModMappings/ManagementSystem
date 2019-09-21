using System.Collections.Generic;

namespace Data.Core.Models.Mapping.Metadata
{
    public class ClassMetadata
        : VersionedComponentMetadataBase
    {
        public virtual ClassMetadata Outer { get; set; }

        public virtual List<ClassMetadata> InheritsFrom { get; set; }

        public string Package { get; set; }

        public virtual List<MethodMetadata> Methods { get; set; }

        public virtual List<FieldMetadata> Fields { get; set; }
    }
}
