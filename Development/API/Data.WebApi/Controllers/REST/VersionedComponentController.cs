using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.WebApi.Extensions;
using Data.WebApi.Model;
using Data.WebApi.Model.Api.Mapping.Component;
using Mcms.Api.Data.Core.Manager.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.WebApi.Controllers.REST
{

    /// <summary>
    /// Handles REST interactions for versioned components.
    /// Allows for lookup, creation deletion and updating versioned component.
    /// </summary>
    [ApiController]
    [Route("/rest/versioned_component/{type}")]
    public class VersionedComponentController : Controller
    {

        private readonly IVersionedComponentDataManager _versionedComponentDataManager;
        private readonly IMapper _mapper;

        public VersionedComponentController(IVersionedComponentDataManager versionedComponentDataManager, IMapper mapper)
        {
            _versionedComponentDataManager = versionedComponentDataManager;
            _mapper = mapper;
        }

        /// <summary>
        /// The type of the versioned component that is being looked up.
        /// </summary>
        [FromRoute(Name = "type")]
        public ComponentType? Type { get; set; } = null;

        /// <summary>
        /// Get method to get a given versioned component with a given id.
        /// </summary>
        /// <param name="id">The id of a versioned component to lookup.</param>
        /// <returns>200 - The versioned component with the given id, 404 - If no versioned component with the given id is found.</returns>
        [HttpGet()]
        [Route("get/{id}")]
        public async Task<ActionResult<VersionedComponentDto>> Get(
            Guid id
        )
        {
            var rawResultQuery = await _versionedComponentDataManager.FindById(id);
            if (rawResultQuery == null || !rawResultQuery.Any())
            {
                return NotFound($"No component exists with the given id: {id}");
            }

            var rawResult = rawResultQuery.First();

            if (rawResult.Component.Type != Type)
            {
                return NotFound($"No component exists with the given id: {id}");
            }

            var dto = _mapper.Map<VersionedComponentDto>(rawResult);
            _mapper.Map(rawResult.Metadata, dto);

            return Ok(dto);
        }

        /// <summary>
        /// Method that looks up the versioned components based on its properties.
        /// </summary>
        /// <param name="componentId">The id of the component to look versioned component variants up for.</param>
        /// <param name="mappingTypeName">A regex that filters the versioned components on having a given mapping in a mapping type who's name matches the given regex. Additionally a mapping regex has to be given to find any.</param>
        /// <param name="mapping">A regex that filters the versioned components on having a mapping who's input or output matches the regex. A mappingTypeName regex is needed to find any.</param>
        /// <param name="releaseName">A regex that filters the versioned components on being part of a release who's name matches the regex.</param>
        /// <param name="gameVersionName">A regex that filters the versioned components on being part of a game version who's name matches the regex.</param>
        /// <param name="pageIndex">The 0-based page index to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paged list of elements that matches the given data.</returns>
        [HttpGet()]
        [Route("list")]
        public async Task<ActionResult<PagedList<VersionedComponentDto>>> List(
            [FromQuery(Name = "component_id")] Guid? componentId = null,
            [FromQuery(Name = "mapping_type_name")] string mappingTypeName = null,
            [FromQuery(Name = "mapping")] string mapping = null,
            [FromQuery(Name = "releaseName")] string releaseName = null,
            [FromQuery(Name = "gameVersion")] string gameVersionName = null,
            [FromQuery(Name = "pageIndex")] int pageIndex = 0,
            [FromQuery(Name = "pageSize")] int pageSize = 25
        )
        {
            var rawQueryable = await _versionedComponentDataManager.FindUsingFilter(
                null,
                Type,
                componentId,
                mappingTypeName,
                mapping,
                releaseName,
                gameVersionName
            );

            return rawQueryable.AsPagedListWithSelect(raw =>
                {
                    var dto = _mapper.Map<VersionedComponentDto>(raw);
                    _mapper.Map(raw.Metadata, dto);
                    return dto;
                },
                pageIndex, pageSize);
        }

        /// <summary>
        /// Deletes a given versioned component (and all of its related entities).
        /// </summary>
        /// <param name="id">The id of the versioned component to delete.</param>
        /// <returns>200 - Including the data of the versioned component that got deleted, 404 - If no versioned component exists with the given id.</returns>
        [HttpDelete()]
        [Route("delete/{id}")]
        public async Task<ActionResult<VersionedComponentDto>> Delete(
            Guid id
        )
        {
            var rawData = await _versionedComponentDataManager.FindById(id);
            if (rawData == null || !rawData.Any())
            {
                return NotFound($"No component can be found with a given id: {id}");
            }

            var target = rawData.First();

            if (target.Component.Type != Type)
            {
                return NotFound($"No component can be found with a given id: {id}");
            }

            await _versionedComponentDataManager.DeleteVersionedComponent(target);
            await _versionedComponentDataManager.SaveChanges();

            return Ok(_mapper.Map<VersionedComponentDto>(target));
        }

        /// <summary>
        /// Creates a new versioned component from the given data.
        /// The api will create a new Id.
        ///
        /// The system returns the data after it has been saved in the database.
        /// </summary>
        /// <param name="componentDto">The data to create the new versioned component from.</param>
        /// <returns>200 - The updated versioned component data (should be identical to the input), with the id set.</returns>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<ComponentDto>> Create(
            [FromBody()] ComponentDto componentDto
        )
        {
            var newId = Guid.NewGuid();
            var versionedComponent = _mapper.Map<VersionedComponent>(componentDto);
            var metadata = _mapper.Map<MetadataBase>(componentDto);

            metadata.Id = newId;
            metadata.VersionedComponent = versionedComponent;

            versionedComponent.Id = newId;
            versionedComponent.Metadata = metadata;

            versionedComponent.CreatedOn = DateTime.Now;
            versionedComponent.CreatedBy = Guid.Parse(ClaimsPrincipal.Current.FindFirstValue(ClaimTypes.Sid));

            await _versionedComponentDataManager.CreateVersionedComponent(versionedComponent);
            var rawNewData = _versionedComponentDataManager.FindById(newId);

            return Ok(_mapper.Map<VersionedComponentDto>(rawNewData));
        }

        /// <summary>
        /// Updates an existing versioned component with the data given by the dto.
        /// </summary>
        /// <param name="id">The id of the versioned component to update the data with.</param>
        /// <param name="versionedComponentDto">The data to update the versioned component with.</param>
        /// <returns>200 - The updated versioned component data (should be identical to the input), 404 - No versioned component with the given id exists.</returns>
        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult<VersionedComponentDto>> Patch(
            Guid id,
            [FromBody()] VersionedComponentDto versionedComponentDto
        )
        {
            var rawDataQuery = await _versionedComponentDataManager.FindById(id);
            if (rawDataQuery == null || !rawDataQuery.Any())
            {
                return NotFound("No component with the given dto's id exists.");
            }

            var rawData = rawDataQuery.First();

            if (rawData.Component.Type != Type)
            {
                return NotFound($"No component can be found with a given id: {id}");
            }

            _mapper.Map(versionedComponentDto, rawData);

            await _versionedComponentDataManager.UpdateVersionedComponent(rawData);
            await _versionedComponentDataManager.SaveChanges();

            return Ok(rawData);
        }
    }
}
