using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace Mcms.Api.Data.Poco.Models.Mapping.Metadata
{
    public class ClassInheritanceData
    {
        [Key]
        public Guid Id { get; set; }

        public virtual ClassMetadata Subclass { get; set; }

        public virtual ClassMetadata Superclass { get; set; }
    }
}
