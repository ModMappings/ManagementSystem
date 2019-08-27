using System;
using System.ComponentModel.DataAnnotations;

namespace API.Model.Creation.Parameter
{
    /// <summary>
    /// Model to create a new parameter.
    /// </summary>
    /// <example>
    /// {
    ///     "name": "name",
    ///     "in": "string1",
    ///     "out": "name",
    ///     "parameterOf": "00000000-0000-0000-0000-000000000000",
    ///     "index": 0
    /// }
    /// </example>
    public class CreateParameterModel
    {
        /// <summary>
        /// The name of the new parameter.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The input mapping of the new parameter.
        /// </summary>
        [Required]
        public string In { get; set; }

        /// <summary>
        /// The output mapping of the new parameter.
        /// </summary>
        [Required]
        public string Out { get; set; }

        /// <summary>
        /// The id of the method that this parameter is part of.
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
