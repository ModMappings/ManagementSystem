using System;
using Data.Core.Models.Core;

namespace Data.WebApi.Model.Read.Core
{
    /// <summary>
    /// A single mappings core data.
    /// </summary>
    public class MappingReadModel
    {
        /// <summary>
        /// The id of the mapping.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The input of the mapping.
        /// </summary>
        public string In { get; set; }

        /// <summary>
        /// The output of the mapping.
        /// </summary>
        public string Out { get; set; }

        /// <summary>
        /// The documentation associated with the mapping.
        /// </summary>
        public string Documentation { get; set; }

        /// <summary>
        /// The name of the mapping used.
        /// </summary>
        public string MappingName { get; set; }

        /// <summary>
        /// The distribution that the mapping is part of.
        /// </summary>
        public Distribution Distribution { get; set; }
    }
}
