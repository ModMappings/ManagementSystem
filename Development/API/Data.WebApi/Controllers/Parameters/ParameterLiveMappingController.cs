using Data.Core.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Parameters
{
    /// <summary>
    /// Controller that handles interactions on live mappings for Parameters.
    /// </summary>
    [Route("/parameters/mappings/live")]
    [ApiController]
    public class ParameterLiveMappingController
        : LiveMappingControllerBase
    {
        public ParameterLiveMappingController(IParameterComponentWriter componentWriter) : base(componentWriter)
        {
        }
    }
}
