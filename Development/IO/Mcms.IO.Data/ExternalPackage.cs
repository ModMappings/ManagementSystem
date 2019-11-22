using System.Collections.Generic;

namespace Mcms.IO.Data
{
    /// <summary>
    /// Represents a package in an external resource.
    /// Contains all data required to import and export a given package.
    /// </summary>
    public class ExternalPackage : ExternalMapping
    {
        /// <summary>
        /// The child packages for this package.
        /// </summary>
        public IList<ExternalClass> Classes { get; set; } = new List<ExternalClass>();
    }
}
