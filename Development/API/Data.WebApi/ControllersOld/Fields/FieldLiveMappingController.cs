using Mcms.Api.Data.Poco.Writers.Core;
using Mcms.Api.Data.Poco.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Fields
{
    /// <summary>
    /// Controller that handles interactions on live mappings for Fields.
    /// </summary>
    [Route("/fields/mappings/live")]
    [ApiController]
    public class FieldLiveMappingController
        : LiveMappingControllerBase
    {
        public FieldLiveMappingController(IFieldComponentWriter componentWriter) : base(componentWriter)
        {
        }
    }
}
