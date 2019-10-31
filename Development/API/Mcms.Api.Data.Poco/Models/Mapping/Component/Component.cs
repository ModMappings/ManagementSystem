using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mcms.Api.Data.Poco.Models.Mapping.Component
{
    /// <summary>
    /// Represents a single mappable object in the source code of the game, across versions, mapping types and releases.
    /// </summary>
    public class Component
    {
        /// <summary>
        /// The id of the component.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// The type of the component.
        /// </summary>
        [Required]
        public ComponentType Type { get; set; }

        /// <summary>
        /// The moment that the component was created.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The user id of the user who created this component.
        /// </summary>
        [Required]
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// The components representations in the relevant game versions that it is part of.
        /// </summary>
        public virtual List<VersionedComponent> VersionedComponents { get; set; }
    }
}
