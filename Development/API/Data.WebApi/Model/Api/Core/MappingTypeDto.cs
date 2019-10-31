using System;
using System.Collections.Generic;

namespace Data.WebApi.Model.Api.Core
{
    /// <summary>
    /// Represents a single mapping type in the system.
    /// For example from an obfuscated source code to source code in tsrg-mappings.
    /// </summary>
    public class MappingTypeDto
    {
        /// <summary>
        /// The id of the type.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the mapping type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The if of the user who created the type.
        /// </summary>
        public virtual Guid CreatedBy { get; }

        /// <summary>
        /// The moment the type was created within the system.
        /// </summary>
        public DateTime CreatedOn { get; }

        /// <summary>
        /// The releases that where made for this type.
        /// </summary>
        public virtual ISet<Guid> Releases { get; set; }
    }
}
