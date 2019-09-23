using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.WebApi.Model.Creation.Core;

namespace Data.WebApi.Model.Creation.Method
{
    /// <summary>
    /// Model to create a new method.
    /// </summary>
    public class CreateMethodModel
    {

        /// <summary>
        /// The initial mappings for this method.
        /// </summary>
        [Required]
        public IEnumerable<CreateMappingModel> Mappings { get; set; }

        /// <summary>
        /// The descriptor of a method.
        /// </summary>
        [Required]
        public string Descriptor { get; set; }

        /// <summary>
        /// Indicates if the method is static in the class.
        /// </summary>
        [Required]
        public bool IsStatic { get; set; }

        /// <summary>
        /// The id of the class that this method is part of.
        /// </summary>
        [Required]
        public Guid MemberOf { get; set; }
    }
}
