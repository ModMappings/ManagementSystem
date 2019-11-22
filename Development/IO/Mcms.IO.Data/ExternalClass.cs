using System.Collections.Generic;

namespace Mcms.IO.Data
{
    /// <summary>
    /// Represents a class that is not part of the Mcms.
    /// Contains all data required to import and export a class.
    /// </summary>
    public class ExternalClass
        : ExternalMapping
    {

        /// <summary>
        /// The methods which are part of this class.
        /// </summary>
        public IList<ExternalMethod> Methods { get; set; } = new List<ExternalMethod>();

        /// <summary>
        /// The fields which are part of this class.
        /// </summary>
        public IList<ExternalField> Fields { get; set; } = new List<ExternalField>();
    }
}
