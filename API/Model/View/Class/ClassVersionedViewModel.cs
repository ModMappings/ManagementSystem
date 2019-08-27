using System;
using System.Collections.Generic;
using API.Model.View.Core;

namespace API.Model.View.Class
{
    /// <summary>
    /// A versioned view model for classes.
    /// </summary>
    public class ClassVersionedViewModel : AbstractVersionedViewModel
    {
        /// <summary>
        /// The name of the package that the class is in.
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// The id of the class that is the outer class of this class.
        /// </summary>
        public Guid? Outer { get; set; }
    }
}
