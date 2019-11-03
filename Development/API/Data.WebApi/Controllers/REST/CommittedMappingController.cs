using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.WebApi.Extensions;
using Data.WebApi.Model;
using Data.WebApi.Services.Core;
using Mcms.Api.Business.Poco.Api.REST.Mapping.Mappings;
using Mcms.Api.Data.Core.Manager.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.REST
{

    [Route("rest/committed_mapping/{type}")]
    [ApiController]
    public class CommittedMappingController : Controller
    {

        private readonly ICommittedMappingDataManager _committedMappingDataManager;
        private readonly IMapper _mapper;

        private readonly IUserResolvingService _userResolvingService;

        public CommittedMappingController(ICommittedMappingDataManager committedMappingDataManager, IMapper mapper, IUserResolvingService userResolvingService)
        {
            _committedMappingDataManager = committedMappingDataManager;
            _mapper = mapper;
            _userResolvingService = userResolvingService;
        }

        /// <summary>
        /// The type of the versioned component that is being looked up.
        /// </summary>
        [FromRoute(Name = "type")]
        public ComponentType? Type { get; set; } = null;

        /// <summary>
        /// Get method to get a given committed mapping with a given id.
        /// </summary>
        /// <param name="id">The id of a committed mapping to lookup.</param>
        /// <returns>200 - The committed mapping with the given id, 404 - If no committed mapping with the given id is found.</returns>
        [HttpGet()]
        [Route("get/{id}")]
        public async Task<ActionResult<CommittedMappingDto>> Get(
            Guid id
        )
        {
            var rawResultQuery = await _committedMappingDataManager.FindById(id);
            if (rawResultQuery == null || !rawResultQuery.Any())
            {
                return NotFound($"No committed mapping exists with the given id: {id}");
            }

            var rawResult = rawResultQuery.First();

            return Ok(_mapper.Map<CommittedMappingDto>(rawResult));
        }

        /// <summary>
        /// Method that looks up the committed mappings based on its properties.
        /// </summary>
        /// <param name="componentId">The id of the component to find the committed mappings for.</param>
        /// <param name="versionedComponentId">The id of the versioned component to find the committed mappings for.</param>
        /// <param name="mappingTypeNameRegex">The regex to match a mappings mapping type name against. Also needs a mapping regex to be supplied to be effective.</param>
        /// <param name="mappingRegex">The regex against which a mapping is matched, for which committed mappings are found. Also needs a mapping type name regex to be supplied to be effective.</param>
        /// <param name="releaseNameRegex">The regex to match release names against.</param>
        /// <param name="gameVersionRegex">The regex to match game version names agents.</param>
        /// <param name="pageIndex">The 0-based page index to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paged list of elements that matches the given data.</returns>
        [HttpGet()]
        [Route("list")]
        public async Task<ActionResult<PagedList<CommittedMappingDto>>> List(
            [FromQuery(Name = "componentId")]Guid? componentId = null,
            [FromQuery(Name = "versionedComponentId")]Guid? versionedComponentId = null,
            [FromQuery(Name = "mappingTypeNameRegex")]string mappingTypeNameRegex = null,
            [FromQuery(Name = "mappingRegex")]string mappingRegex = null,
            [FromQuery(Name = "releaseNameRegex")]string releaseNameRegex = null,
            [FromQuery(Name = "gameVersionRegex")]string gameVersionRegex = null,
            [FromQuery(Name = "pageIndex")] int pageIndex = 0,
            [FromQuery(Name = "pageSize")] int pageSize = 25
        )
        {
            var rawQueryable = await _committedMappingDataManager.FindUsingFilter(
                null,
                Type,
                componentId,
                versionedComponentId,
                mappingTypeNameRegex,
                mappingRegex,
                releaseNameRegex,
                gameVersionRegex
            );

            return rawQueryable.ProjectTo<CommittedMappingDto>(_mapper.ConfigurationProvider).AsPagedList(pageIndex, pageSize);
        }

        /// <summary>
        /// Deletes a given committed mapping (and all of its related entities).
        /// </summary>
        /// <param name="id">The id of the committed mapping to delete.</param>
        /// <returns>200 - Including the data of the committed mapping that got deleted, 404 - If no committed mapping exists with the given id.</returns>
        [HttpDelete()]
        [Route("delete/{id}")]
        public async Task<ActionResult<CommittedMappingDto>> Delete(
            Guid id
        )
        {
            var rawData = await _committedMappingDataManager.FindById(id);
            if (rawData == null || !rawData.Any())
            {
                return NotFound($"No committed mapping can be found with a given id: {id}");
            }

            var target = rawData.First();

            await _committedMappingDataManager.DeleteCommittedMapping(target);
            await _committedMappingDataManager.SaveChanges();

            return Ok(_mapper.Map<CommittedMappingDto>(target));
        }

        /// <summary>
        /// Creates a new committed mapping from the given data.
        /// The api will create a new Id.
        ///
        /// The system returns the data after it has been saved in the database.
        /// </summary>
        /// <param name="committedMappingDto">The data to create the new committed mapping from.</param>
        /// <returns>200 - The updated committed mapping data (should be identical to the input), with the id set.</returns>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<CommittedMappingDto>> Create(
            [FromBody()] CommittedMappingDto committedMappingDto
        )
        {
            var newId = Guid.NewGuid();
            var committedMapping = _mapper.Map<CommittedMapping>(committedMappingDto);

            committedMapping.Id = newId;
            committedMapping.CreatedOn = DateTime.Now;
            committedMapping.CreatedBy = Guid.Parse(ClaimsPrincipal.Current.FindFirstValue(ClaimTypes.Sid));

            await _committedMappingDataManager.CreateCommittedMapping(committedMapping);
            var rawNewData = _committedMappingDataManager.FindById(newId);

            return Ok(_mapper.Map<CommittedMappingDto>(rawNewData));
        }

        /// <summary>
        /// Updates an existing committed mapping with the data given by the dto.
        /// </summary>
        /// <param name="id">The id of the committed mapping to update the data with.</param>
        /// <param name="committedMappingDto">The data to update the committed mapping with.</param>
        /// <returns>200 - The updated committed mapping data (should be identical to the input), 404 - No committed mapping with the given id exists.</returns>
        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult<CommittedMappingDto>> Patch(
            Guid id,
            [FromBody()] CommittedMappingDto committedMappingDto
        )
        {
            var rawDataQuery = await _committedMappingDataManager.FindById(id);
            if (rawDataQuery == null || !rawDataQuery.Any())
            {
                return NotFound("No committed mapping with the given dto's id exists.");
            }

            var rawData = rawDataQuery.First();

            _mapper.Map(committedMappingDto, rawData);

            await _committedMappingDataManager.UpdateCommittedMapping(rawData);
            await _committedMappingDataManager.SaveChanges();

            return Ok(rawData);
        }
    }
}
