using Mcms.Api.Data.Poco.Writers.Core;
using Mcms.Api.Data.Poco.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Methods
{
    /// <summary>
    /// Controller that handles interactions on live mappings for Methods.
    /// </summary>
    [Route("/methods/mappings/live")]
    [ApiController]
    public class MethodLiveMappingController
        : LiveMappingControllerBase
    {
        public MethodLiveMappingController(IMethodComponentWriter componentWriter) : base(componentWriter)
        {
        }
    }
}
