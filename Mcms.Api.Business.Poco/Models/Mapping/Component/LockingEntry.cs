using Mcms.Api.Business.Poco.Models.Core;

namespace Mcms.Api.Business.Poco.Models.Mapping.Component
{
    /// <summary>
    /// An record that indicates a many to many relationship between a component in a given game version, as well
    /// as a given mapping type.
    /// If such a record exists, no proposals can be made for the given component in the given game version with the given mapping type.
    /// </summary>
    public class LockingEntry
    {
        /// <summary>
        /// The id of the locking entry.
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// The versioned component that is being locked for a given mapping type.
        /// </summary>
        [Required]
        public virtual VersionedComponent VersionedComponent { get; set; }

        /// <summary>
        /// The mapping type which is locked for a given versioned component.
        /// </summary>
        [Required]
        public virtual MappingType MappingType { get; set; }
    }
}
