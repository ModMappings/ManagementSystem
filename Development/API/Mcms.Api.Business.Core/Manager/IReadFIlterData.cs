using System;
using Mcms.Api.Business.Core.Stores;
using Mcms.Api.Data.Poco.Models.Mapping.Component;

namespace Mcms.Api.Business.Core.Manager
{
    /// <summary>
    /// Data used during the reading of data from a manager.
    /// The filter data is turned into a <see cref="IQueryFilter{TEntity}"/>
    /// </summary>
    public interface IReadFilterData
    {

        Guid ById { get; }

        string ByMappingName { get; }

        ComponentType ByComponentType { get; }


    }
}
