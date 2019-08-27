using System;
using System.Collections.Generic;

namespace API.Model.View.Core
{
    public class AbstractViewModel<TVersionedViewModel>
        where TVersionedViewModel : AbstractVersionedViewModel
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
        public IEnumerable<TVersionedViewModel> Versioned { get; set; }
    }
}
