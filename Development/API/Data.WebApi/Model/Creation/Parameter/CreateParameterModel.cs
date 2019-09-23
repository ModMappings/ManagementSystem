using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.WebApi.Model.Creation.Core;

namespace Data.WebApi.Model.Creation.Parameter
{
    /// <summary>
    /// Model to create a new parameter.
    /// </summary>
    public class CreateParameterModel
    {

        /// <summary>
        /// The initial mappings for this parameter.
        /// </summary>
        [Required]
        public IEnumerable<CreateMappingModel> Mappings { get; set; }

        /// <summary>
        /// The id of the version method that this parameter is part of.
        /// </summary>
        [Required]
        public Guid ParameterOf { get; set; }

        /// <summary>
        /// The zero-based index of the arguments of the method.
        /// </summary>
        [Required]
        public int Index { get; set; }
    }
}
