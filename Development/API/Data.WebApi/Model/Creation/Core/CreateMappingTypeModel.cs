using System;
using System.Collections.Generic;

namespace Data.WebApi.Model.Creation.Core
{
    /// <summary>
    /// Model used to create a new mapping type.
    /// </summary>
    public class CreateMappingTypeModel
    {
        /// <summary>
        /// The name of the mapping type.
        /// </summary>
        public string Name { get; set; }
    }
}
