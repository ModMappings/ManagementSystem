using Mcms.Api.Data.Poco.Readers.Core;
using Mcms.Api.Data.Poco.Writers.Core;
using Mcms.Api.Data.Poco.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Fields
{
    /// <summary>
    /// Controller that handles interactions on proposal mappings for fields.
    /// </summary>
    [Route("/fields/mappings/proposals")]
    [ApiController]
    public class FieldProposalController
        : ProposalControllerBase
    {
        public FieldProposalController(IFieldComponentWriter componentWriter, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(componentWriter, userResolvingService, mappingTypeReader)
        {
        }
    }
}
