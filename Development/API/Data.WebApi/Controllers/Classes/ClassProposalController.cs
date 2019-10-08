using Mcms.Api.Data.Poco.Readers.Core;
using Mcms.Api.Data.Poco.Readers.Mapping;
using Mcms.Api.Data.Poco.Writers.Core;
using Mcms.Api.Data.Poco.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Classes
{
    /// <summary>
    /// Controller that handles interactions on proposal mappings for classes.
    /// </summary>
    [Route("/classes/mappings/proposals")]
    [ApiController]
    public class ClassProposalController
        : ProposalControllerBase
    {
        public ClassProposalController(IClassComponentWriter componentWriter, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(componentWriter, userResolvingService, mappingTypeReader)
        {
        }
    }
}
