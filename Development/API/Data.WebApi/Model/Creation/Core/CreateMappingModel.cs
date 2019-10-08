using System.ComponentModel.DataAnnotations;
using Mcms.Api.Data.Poco.Models.Core;

namespace Data.WebApi.Model.Creation.Core
{

    /// <summary>
    /// Allows for the creation of a single mapping.
    /// Used in models which are responsible for creating new mappings.
    /// </summary>
    public class CreateMappingModel
    {
        /// <summary>
        /// The name of the mapping type this mapping represents.
        /// </summary>
        [Required]
        public string MappingTypeName { get; set; }

        /// <summary>
        /// The input of the mapping.
        /// </summary>
        [Required]
        public string In { get; set; }

        /// <summary>
        /// The output of the mapping.
        /// </summary>
        [Required]
        public string Out { get; set; }

        /// <summary>
        /// The documentation for the mapping, if applicable.
        /// </summary>
        public string Documentation { get; set; }

        /// <summary>
        /// The distribution that the mapping is part of.
        /// </summary>
        public Distribution Distribution { get; set; }
    }
}
