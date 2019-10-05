using System;
using System.Collections.Generic;
using Data.WebApi.Model.Read.Core;

namespace Data.WebApi.Model.Read.Class
{
    /// <summary>
    /// A versioned view model for classes.
    /// </summary>
    public class ClassVersionedReadModel : AbstractVersionedReadModel
    {
        /// <summary>
        /// The name of the package that the class is in.
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// The id of the class that is the outer class of this class.
        /// </summary>
        public Guid? Outer { get; set; }

        /// <summary>
        /// The class this class inherits from.
        /// </summary>
        public IEnumerable<Guid> InheritsFrom { get; set; }

        /// <summary>
        /// The versioned id of the methods that are part of the class in this version.
        /// </summary>
        public IEnumerable<Guid> Methods { get; set; }


        /// <summary>
        /// The versioned id of the fields that are part of the class in this version.
        /// </summary>
        public IEnumerable<Guid> Fields { get; set; }
    }
}
