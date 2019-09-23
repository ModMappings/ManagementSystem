using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.WebApi.Model.Creation.Core;

namespace Data.WebApi.Model.Creation.Field
{
    /// <summary>
    /// Model to create a new field.
    /// </summary>
    public class CreateFieldModel
    {

        /// <summary>
        /// The initial mappings for this field.
        /// </summary>
        [Required]
        public IEnumerable<CreateMappingModel> Mappings { get; set; }

        /// <summary>
        /// The id of the class that this field is part of.
        /// </summary>
        [Required]
        public Guid MemberOf { get; set; }

        /// <summary>
        /// Indicates if the field is static.
        /// </summary>
        public bool IsStatic { get; set; }
    }
}
