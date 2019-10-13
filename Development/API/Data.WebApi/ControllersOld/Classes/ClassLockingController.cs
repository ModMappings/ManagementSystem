using Mcms.Api.Data.Poco.Readers.Core;
using Mcms.Api.Data.Poco.Writers.Core;
using Mcms.Api.Data.Poco.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Classes
{
    /// <summary>
    /// Controller that handles interactions with regards to locking and unlocking classes.
    /// </summary>
    [Route("/classes/locking")]
    [ApiController]
    public class ClassLockingController
        : LockingControllerBase
    {
        public ClassLockingController(IClassComponentWriter componentWriter, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(componentWriter, userResolvingService, mappingTypeReader)
        {
        }
    }
}
