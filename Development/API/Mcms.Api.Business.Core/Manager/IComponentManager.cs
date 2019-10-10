using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Business.Core.Manager.Filter;
using Mcms.Api.Business.Core.Stores;

namespace Mcms.Api.Business.Core.Manager
{
    /// <summary>
    /// Manager that handles interaction with components.
    /// </summary>
    public interface IComponentManager
    {

        /// <summary>
        /// Gives access to the component that match the read filter.
        /// If no filter is passed in all component are returned.
        /// </summary>
        /// <param name="filterData">The filter data to filter the components on.</param>
        /// <returns>The task that looks up the components that match the given filter data.</returns>
        Task<IQueryable<Component>> Read(IReadFilter filterData = null);

    }
}
