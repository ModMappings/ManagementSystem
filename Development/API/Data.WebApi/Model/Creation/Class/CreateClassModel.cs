using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.WebApi.Model.Creation.Core;

namespace Data.WebApi.Model.Creation.Class
{
    /// <summary>
    /// Model used to create a new class or a versioned class.
    /// </summary>
    public class CreateClassModel
    {
        /// <summary>
        /// The initial mappings for this class.
        /// </summary>
        [Required]
        public IEnumerable<CreateMappingModel> Mappings { get; set; }

        /// <summary>
        /// The package that the new class resides in.
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// The id of the outer class that the new class resides in.
        /// </summary>
        public Guid? Outer { get; set; }

        /// <summary>
        /// The ids of the classes or interfaces from which this class inherits.
        /// </summary>
        public IEnumerable<Guid> InheritsFrom { get; set; }
    }
}
