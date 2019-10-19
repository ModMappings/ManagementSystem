using Mcms.Api.Business.Poco.Models.Core;
using Mcms.Api.Business.Poco.Models.Mapping.Component;

namespace Mcms.Api.Business.Poco.Models.Mapping.Mappings
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
        /// The distribution that this mapping can be found in.
        /// </summary>
        [Required]
        public Distribution Distribution { get; set; }

        /// <summary>
        /// The mapping type for which this mapping is made.
        /// </summary>
        [Required]
        public virtual MappingType MappingType { get; set; }
    }
}
