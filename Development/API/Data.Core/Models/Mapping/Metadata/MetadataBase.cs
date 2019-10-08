using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Core.Models.Mapping.Component;

namespace Data.Core.Models.Mapping.Metadata
{
    public class MetadataBase
    {
        [ForeignKey("VersionedComponentForeignKey")]
        public virtual VersionedComponent VersionedComponent { get; set; }

        [Key]
        public Guid VersionedComponentForeignKey { get; set; }
    }
}
