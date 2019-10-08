using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping.Component;

namespace Data.Core.Models.Mapping.Mappings
{
    /// <summary>
    /// A base class with the core data for each mapping.
    /// </summary>
    public abstract class MappingBase
    {
        /// <summary>
        /// The id of the mapping.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// The versioned component that this mapping is for.
        /// </summary>
        [Required]
        public virtual VersionedComponent VersionedComponent { get; set; }

        /// <summary>
        /// The moment the mapping was created.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The input of the mapping.
        /// </summary>
        [Required]
        public string InputMapping { get; set; }

        /// <summary>
        /// The output of the mapping.
        /// </summary>
        [Required]
        public string OutputMapping { get; set; }

        /// <summary>
        /// The documentation that accompanies the mapping.
        /// </summary>
        public string Documentation { get; set; }

        /// <summary>
        /// The distribution that
        /// </summary>
        [Required]
        public Distribution Distribution { get; set; }

        [Required]
        public virtual MappingType MappingType { get; set; }
    }
}
