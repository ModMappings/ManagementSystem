using System;
using System.Linq;
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

    /// <summary>
    /// Handles REST interactions for components.
    /// Allows, for the lookup, deletion, creation and updating of components.
    /// Certain elevated rights might be needed to perform, deletion, and creation of components directly via this
    /// controller.
    ///
    /// Use workflows, where applicable, to handle the creation, deletion and updating of components if the user
    /// or application does not have the rights to do it directly via this controller.
    /// </summary>
    [ApiController]
    [Route("/rest/component")]
    public class ComponentController : Controller
    {

        private readonly IComponentDataManager _componentDataManager;
        private readonly IMapper _mapper;

        public ComponentController(IComponentDataManager componentDataManager, IMapper mapper)
        {
            _componentDataManager = componentDataManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Get method to get a given component with a given id.
        /// </summary>
        /// <param name="id">The id of a component to lookup.</param>
        /// <returns>200 - The component with the given id, 404 - If no component with the given id is found.</returns>
        [HttpGet()]
        [Route("get/{id}")]
        public async Task<ActionResult<ComponentDto>> Get(
            Guid id
        )
        {
            var rawResultQuery = await _componentDataManager.FindById(id);
            if (rawResultQuery == null || !rawResultQuery.Any())
            {
                return NotFound($"No component exists with the given id: {id}");
            }

            var rawResult = rawResultQuery.First();

            return Ok(_mapper.Map<ComponentDto>(rawResult));
        }

        /// <summary>
        /// Method that looks up the components based on its properties.
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
        public async Task<ActionResult<PagedList<ComponentDto>>> List(
            [FromQuery(Name = "type")] ComponentType? type =  null,
            [FromQuery(Name = "mapping_type_name")] string mappingTypeName = null,
            [FromQuery(Name = "mapping")] string mapping = null,
            [FromQuery(Name = "releaseName")] string releaseName = null,
            [FromQuery(Name = "gameVersion")] string gameVersionName = null,
            [FromQuery(Name = "pageIndex")] int pageIndex = 0,
            [FromQuery(Name = "pageSize")] int pageSize = 25
        )
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

        /// <summary>
        /// Deletes a given component (and all of its related entities).
        /// </summary>
        /// <param name="id">The id of the component to delete.</param>
        /// <returns>200 - Including the data of the component that got deleted, 404 - If no component exists with the given id.</returns>
        [HttpDelete()]
        [Route("delete/{id}")]
        public async Task<ActionResult<ComponentDto>> Delete(
            Guid id
        )
        {
            var rawData = await _componentDataManager.FindById(id);
            if (rawData == null || !rawData.Any())
            {
                return NotFound($"No component can be found with a given id: {id}");
            }

            var target = rawData.First();

            await _componentDataManager.DeleteComponent(target);
            await _componentDataManager.SaveChanges();

            return Ok(_mapper.Map<ComponentDto>(target));
        }

        /// <summary>
        /// Updates an existing component with the data given by the  dto.
        /// For components only <see cref="ComponentDto.Type"/> can be updated this way.
        /// All other data is treated as readonly.
        /// </summary>
        /// <param name="id">The id of the component to update the data with.</param>
        /// <param name="componentDto">The data to update the component with.</param>
        /// <returns>200 - The updated component data (should be identical to the input), 404 - No component with the given id exists.</returns>
        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult<ComponentDto>> Patch(
            Guid id,
            [FromBody()] ComponentDto componentDto
        )
        {
            var rawDataQuery = await _componentDataManager.FindById(id);
            if (rawDataQuery == null || !rawDataQuery.Any())
            {
                return NotFound("No component with the given dto's id exists.");
            }

            var rawData = rawDataQuery.First();

            _mapper.Map(componentDto, rawData);

            await _componentDataManager.UpdateComponent(rawData);
            await _componentDataManager.SaveChanges();

            return Ok(rawData);
        }
    }
}
