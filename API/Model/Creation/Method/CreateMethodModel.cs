using System;
using System.ComponentModel.DataAnnotations;

namespace API.Model.Creation.Method
{
    /// <summary>
    /// Model to create a new method.
    /// </summary>
    /// <example>
    /// {
    ///     "name": "a",
    ///     "in": "a",
    ///     "out": "func_195695_a",
    ///     "memberOf": "00000000-0000-0000-0000-000000000000"
    /// }
    /// </example>
    public class CreateMethodModel
    {
        /// <summary>
        /// The input mapping of the new method.
        /// </summary>
        [Required]
        public string In { get; set; }

        /// <summary>
        /// The output mapping of the new method.
        /// </summary>
        [Required]
        public string Out { get; set; }

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
        /// The documentation for method.
        /// </summary>
        [Required]
        public string Documentation { get; set; }

        /// <summary>
        /// The id of the class that this method is part of.
        /// </summary>
        [Required]
        public Guid MemberOf { get; set; }
    }
}
