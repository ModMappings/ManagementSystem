using System;
using System.Collections.Generic;

namespace Data.WebApi.Model.Read.Core
{
    /// <summary>
    /// Abstract read model for core components.
    /// </summary>
    /// <typeparam name="TVersionedReadModel">The versioned read model.</typeparam>
    public class AbstractReadModel<TVersionedReadModel>
        where TVersionedReadModel : AbstractVersionedReadModel
    {
        /// <summary>
        /// The id of the class.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The versioned view models.
        /// </summary>
        public IEnumerable<TVersionedReadModel> Versioned { get; set; }
    }
}
