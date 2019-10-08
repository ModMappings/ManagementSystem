using Mcms.Api.Data.Poco.Readers.Core;
using Mcms.Api.Data.Poco.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Parameters
{
    /// <summary>
    /// Controller that handles interactions with regards to locking and unlocking parameters.
    /// </summary>
    [Route("/parameters/locking")]
    [ApiController]
    public class ParameterLockingController
        : LockingControllerBase
    {
        public ParameterLockingController(IParameterComponentWriter componentWriter, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(componentWriter, userResolvingService, mappingTypeReader)
        {
        }
    }
}
