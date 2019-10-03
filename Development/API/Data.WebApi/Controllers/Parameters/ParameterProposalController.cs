using Data.Core.Readers.Core;
using Data.Core.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Parameters
{
    /// <summary>
    /// Controller that handles interactions on proposal mappings for parameters.
    /// </summary>
    [Route("/parameters/mappings/proposals")]
    [ApiController]
    public class ParameterProposalController
        : ProposalControllerBase
    {
        public ParameterProposalController(IParameterComponentWriter componentWriter, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(componentWriter, userResolvingService, mappingTypeReader)
        {
        }
    }
}
