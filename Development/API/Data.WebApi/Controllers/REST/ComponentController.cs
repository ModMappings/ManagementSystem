using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.WebApi.Extensions;
using Data.WebApi.Model;
using Data.WebApi.Model.Api.Mapping.Component;
using Mcms.Api.Data.Core.Manager.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Data.WebApi.Controllers.REST
{
    [ApiController]
    [Route("/rest/component")]
    public class ComponentController : Controller
    {

        private readonly IComponentDataManager _componentDataManager;
        private readonly IMapper _mapper;

        public ComponentController(IComponentDataManager componentDataManager, IMapper mapper, ILogger<ComponentController> logger)
        {
            _componentDataManager = componentDataManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Get method that looks up the components based on its properties.
        /// </summary>
        /// <param name="type">The type of components to look for.</param>
        /// <param name="mappingTypeName">A regex that filters the components on having a given mapping in a mapping type who's name matches the given regex. Additionally a mapping regex has to be given to find any.</param>
        /// <param name="mapping">A regex that filters the components on having a mapping who's input or output matches the regex. A mappingTypeName regex is needed to find any.</param>
        /// <param name="releaseName">A regex that filters the components on being part of a release who's name matches the regex.</param>
        /// <param name="gameVersionName">A regex that filters the components on being part of a game version who's name matches the regex.</param>
        /// <param name="pageIndex">The 0-based page index to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paged list of elements that matches the given data.</returns>
        [HttpGet()]
        [Route("list")]
        public async Task<PagedList<ComponentDto>> List(
            [FromQuery(Name = "type")] ComponentType? type =  null,
            [FromQuery(Name = "mapping_type_name")] string mappingTypeName = ".*",
            [FromQuery(Name = "mapping")] string mapping = ".*",
            [FromQuery(Name = "releaseName")] string releaseName = ".*",
            [FromQuery(Name = "gameVersion")] string gameVersionName = ".*",
            [FromQuery(Name = "pageIndex")] int pageIndex = 0,
            [FromQuery(Name = "pageSize")] int pageSize = 25)
        {
            var rawQueryable = await _componentDataManager.FindUsingFilter(
                null,
                type,
                mappingTypeName,
                mapping,
                releaseName,
                gameVersionName
            );

            return rawQueryable.ProjectTo<ComponentDto>(_mapper.ConfigurationProvider).AsPagedList(pageIndex, pageSize);
        }
    }
}
