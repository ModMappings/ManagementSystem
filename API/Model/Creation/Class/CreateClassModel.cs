using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Model.Creation.Class
{
    /// <summary>
    /// Model used to create a new class.
    /// </summary>
    /// <example>
    /// {
    ///     "name": "ItemStack",
    ///     "in": "bcj",
    ///     "out": "ItemStack",
    ///     "package": "net.minecraft.item",
    ///     "outer": "00000000-0000-0000-0000-000000000000"
    /// }
    /// </example>
    public class CreateClassModel
    {
        /// <summary>
        /// The name of the new class.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The input mapping of the new class.
        /// </summary>
        [Required]
        public string In { get; set; }

        /// <summary>
        /// THe output mapping of the new class.
        /// </summary>
        [Required]
        public string Out { get; set; }

        /// <summary>
        /// The package that the new class resides in.
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// The id of the outer class that the new class resides in.
        /// </summary>
        public Guid Outer { get; set; }

        /// <summary>
        /// The ids of the classes or interfaces from which this class inherits.
        /// </summary>
        public IEnumerable<Guid> InheritsFrom { get; set; }
    }
}
