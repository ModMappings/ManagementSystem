using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Core.Models.Mapping.Metadata
{
    public class VersionedComponentMetadataBase
    {
        [ForeignKey("VersionedComponentForeignKey")]
        public virtual VersionedComponent VersionedComponent { get; set; }

        [Key]
        public Guid VersionedComponentForeignKey { get; set; }
    }
}
