using System.Collections.Generic;

namespace Mcms.IO.Data
{
    /// <summary>
    /// Represents a method who's mapping is not yet part of the Mcms.
    /// Contains all data needed to import and export a given method.
    /// </summary>
    public class ExternalMethod
        : ExternalMapping
    {

        /// <summary>
        /// The parameters which are part of this method.
        /// </summary>
        public IList<ExternalParameter> ExternalParameters { get; set; } = new List<ExternalParameter>();

        /// <summary>
        /// The descriptor of the method.
        /// </summary>
        public string Descriptor { get; set; } = "";

        /// <summary>
        /// Indicates if this method is static.
        /// </summary>
        public bool IsStatic { get; set; } = false;
    }
}
