using System;
using System.Collections.Generic;

namespace API.Model.Read.Core
{
    public class AbstractReadModel<TVersionedReadModel>
        where TVersionedReadModel : AbstractVersionedReadModel
    {
        /// <summary>
        /// The id of the class.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the class.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The versioned view models.
        /// </summary>
        public IEnumerable<TVersionedReadModel> Versioned { get; set; }
    }
}
