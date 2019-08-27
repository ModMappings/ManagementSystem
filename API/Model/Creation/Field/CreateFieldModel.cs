using System;
using System.ComponentModel.DataAnnotations;

namespace API.Model.Creation.Field
{
    /// <summary>
    /// Model to create a new field.
    /// </summary>
    /// <example>
    /// {
    ///     "name": "a",
    ///     "in": "a",
    ///     "out": "field_190927_a",
    ///     "memberOf": "00000000-0000-0000-0000-000000000000"
    /// }
    /// </example>
    public class CreateFieldModel
    {
        /// <summary>
        /// The name of the new field.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The input mapping of the new field.
        /// </summary>
        [Required]
        public string In { get; set; }

        /// <summary>
        /// The output mapping of the new field.
        /// </summary>
        [Required]
        public string Out { get; set; }

        /// <summary>
        /// The id of the class that this field is part of.
        /// </summary>
        [Required]
        public Guid MemberOf { get; set; }
    }
}
