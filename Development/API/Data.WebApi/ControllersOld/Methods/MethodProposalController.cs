using Mcms.Api.Data.Poco.Readers.Core;
using Mcms.Api.Data.Poco.Writers.Core;
using Mcms.Api.Data.Poco.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Methods
{
    /// <summary>
    /// Controller that handles interactions on proposal mappings for methods.
    /// </summary>
    [Route("/methods/mappings/proposals")]
    [ApiController]
    public class MethodProposalController
        : ProposalControllerBase
    {
        public MethodProposalController(IMethodComponentWriter componentWriter, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(componentWriter, userResolvingService, mappingTypeReader)
        {
        }
    }
}
