using AutoMapper;
using Mcms.Api.Data.Core.Manager.Mapping.Component;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.REST
{

    /// <summary>
    /// Handles REST interactions for versioned components.
    /// Allows for lookup, creation deletion and updating versioned component.
    ///
    ///
    /// </summary>
    [ApiController]
    [Route("/rest/versioned_component")]
    public class VersionedComponentController
    {

        private readonly IVersionedComponentDataManager _versionedComponentDataManager;
        private readonly IMapper _mapper;

        public VersionedComponentController(IVersionedComponentDataManager versionedComponentDataManager, IMapper mapper)
        {
            _versionedComponentDataManager = versionedComponentDataManager;
            _mapper = mapper;
        }


    }
}
