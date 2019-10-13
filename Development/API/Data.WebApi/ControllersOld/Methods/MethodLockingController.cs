using Mcms.Api.Data.Poco.Readers.Core;
using Mcms.Api.Data.Poco.Writers.Core;
using Mcms.Api.Data.Poco.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Methods
{
    /// <summary>
    /// Controller that handles interactions with regards to locking and unlocking methods.
    /// </summary>
    [Route("/methods/locking")]
    [ApiController]
    public class MethodLockingController
        : LockingControllerBase
    {
        public MethodLockingController(IMethodComponentWriter componentWriter, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(componentWriter, userResolvingService, mappingTypeReader)
        {
        }
    }
}
