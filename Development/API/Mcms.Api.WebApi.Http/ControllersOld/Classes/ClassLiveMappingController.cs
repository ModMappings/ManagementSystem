using Mcms.Api.Data.Poco.Writers.Core;
using Mcms.Api.Data.Poco.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Classes
{
    /// <summary>
    /// Controller that handles interactions on live mappings for classes.
    /// </summary>
    [Route("/classes/mappings/live")]
    [ApiController]
    public class ClassLiveMappingController
        : LiveMappingControllerBase
    {
        public ClassLiveMappingController(IClassComponentWriter componentWriter) : base(componentWriter)
        {
        }
    }
}
