using System;

namespace API.Model.Read.Core
{
    public class MappingReadModel
    {
        /// <summary>
        /// The id of the mapping.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The input of the mapping.
        /// </summary>
        public string In { get; set; }

        /// <summary>
        /// The output of the mapping.
        /// </summary>
        public string Out { get; set; }
    }
}
