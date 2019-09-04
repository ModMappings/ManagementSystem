using System.Collections.Generic;

namespace Data.Core.Models.Mapping.MetaData
{
    public class ClassMetadata
        : VersionedComponentMetadataBase
    {
        public virtual VersionedComponent Outer { get; set; }

        public virtual List<VersionedComponent> InheritsFrom { get; set; }

        public string Package { get; set; }
    }
}
