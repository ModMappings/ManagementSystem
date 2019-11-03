using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Mcms.Api.Business.Poco.Api.REST.Mapping.Mappings;
using Mcms.Api.Data.Core.Manager.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Mcms.Api.WebApi.Http.Extensions;
using Mcms.Api.WebApi.Http.Model;
using Mcms.Api.WebApi.Http.Services.Core;
using Microsoft.AspNetCore.Mvc;

namespace Mcms.Api.WebApi.Http.Controllers.REST
{

    [Route("rest/proposed_mapping/{type}")]
    [ApiController]
    public class ProposedMappingController : Controller
    {

        private readonly IProposedMappingDataManager _proposedMappingDataManager;
        private readonly IMapper _mapper;

        private readonly IUserResolvingService _userResolvingService;

        public ProposedMappingController(IProposedMappingDataManager proposedMappingDataManager, IMapper mapper, IUserResolvingService userResolvingService)
        {
            _proposedMappingDataManager = proposedMappingDataManager;
            _mapper = mapper;
            _userResolvingService = userResolvingService;
        }

        /// <summary>
        /// The type of the versioned component that is being looked up.
        /// </summary>
        [FromRoute(Name = "type")]
        public ComponentType? Type { get; set; } = null;

        /// <summary>
        /// Get method to get a given proposed mapping with a given id.
        /// </summary>
        /// <param name="id">The id of a proposed mapping to lookup.</param>
        /// <returns>200 - The proposed mapping with the given id, 404 - If no proposed mapping with the given id is found.</returns>
        [HttpGet()]
        [Route("{id}")]
        public async Task<ActionResult<ProposedMappingDto>> Get(
            Guid id
        )
        {
            var rawResultQuery = await _proposedMappingDataManager.FindById(id);
            if (rawResultQuery == null || !rawResultQuery.Any())
            {
                return NotFound($"No proposed mapping exists with the given id: {id}");
            }

            var rawResult = rawResultQuery.First();

            return Ok(_mapper.Map<ProposedMappingDto>(rawResult));
        }

        /// <summary>
        /// Method that looks up the proposed mappings based on its properties.
        /// </summary>
        /// <param name="componentId">The id of the component to find the proposed mappings for.</param>
        /// <param name="versionedComponentId">The id of the versioned component to find the proposed mappings for.</param>
        /// <param name="mappingTypeNameRegex">The regex to match a mappings mapping type name against. Also needs a mapping regex to be supplied to be effective.</param>
        /// <param name="mappingRegex">The regex against which a mapping is matched, for which proposed mappings are found. Also needs a mapping type name regex to be supplied to be effective.</param>
        /// <param name="gameVersionRegex">The regex to match game version names agents.</param>
        /// <param name="isOpen">Indicator used to filter proposals which are open or closed.</param>
        /// <param name="isPublicVote">Indicator used to filter proposals which are public or not.</param>
        /// <param name="closedBy">Indicator used to filter by who closed the proposal.</param>
        /// <param name="closedOn">Indicator used to filter by the date the proposal was closed.</param>
        /// <param name="merged">Indicator used to filter the proposal on merged or not status.</param>
        /// <param name="pageIndex">The 0-based page index to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paged list of elements that matches the given data.</returns>
        [HttpGet()]
        [Route("")]
        public async Task<ActionResult<PagedList<ProposedMappingDto>>> List(
            [FromQuery(Name = "componentId")]Guid? componentId = null,
            [FromQuery(Name = "versionedComponentId")]Guid? versionedComponentId = null,
            [FromQuery(Name = "mappingTypeNameRegex")]string mappingTypeNameRegex = null,
            [FromQuery(Name = "mappingRegex")]string mappingRegex = null,
            [FromQuery(Name = "gameVersionRegex")]string gameVersionRegex = null,
            [FromQuery(Name = "pageIndex")] bool? isOpen = null,
            [FromQuery(Name = "pageIndex")] bool? isPublicVote = null,
            [FromQuery(Name = "pageIndex")] Guid? closedBy = null,
            [FromQuery(Name = "pageIndex")] DateTime? closedOn = null,
            [FromQuery(Name = "pageIndex")] bool? merged = null,
            [FromQuery(Name = "pageIndex")] int pageIndex = 0,
            [FromQuery(Name = "pageSize")] int pageSize = 25
        )
        {
            var rawQueryable = await _proposedMappingDataManager.FindUsingFilter(
                null,
                Type,
                componentId,
                versionedComponentId,
                mappingTypeNameRegex,
                mappingRegex,
                gameVersionRegex,
                isOpen,
                isPublicVote,
                closedBy,
                closedOn,
                merged
            );

            return rawQueryable.ProjectTo<ProposedMappingDto>(_mapper.ConfigurationProvider).AsPagedList(pageIndex, pageSize);
        }

        /// <summary>
        /// Deletes a given proposed mapping (and all of its related entities).
        /// </summary>
        /// <param name="id">The id of the proposed mapping to delete.</param>
        /// <returns>200 - Including the data of the proposed mapping that got deleted, 404 - If no proposed mapping exists with the given id.</returns>
        [HttpDelete()]
        [Route("{id}")]
        public async Task<ActionResult<ProposedMappingDto>> Delete(
            Guid id
        )
        {
            var rawData = await _proposedMappingDataManager.FindById(id);
            if (rawData == null || !rawData.Any())
            {
                return NotFound($"No proposed mapping can be found with a given id: {id}");
            }

            var target = rawData.First();

            await _proposedMappingDataManager.DeleteProposedMapping(target);
            await _proposedMappingDataManager.SaveChanges();

            return Ok(_mapper.Map<ProposedMappingDto>(target));
        }

        /// <summary>
        /// Creates a new proposed mapping from the given data.
        /// The api will create a new Id.
        ///
        /// The system returns the data after it has been saved in the database.
        /// </summary>
        /// <param name="proposedMappingDto">The data to create the new proposed mapping from.</param>
        /// <returns>200 - The updated proposed mapping data (should be identical to the input), with the id set.</returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ProposedMappingDto>> Create(
            [FromBody()] ProposedMappingDto proposedMappingDto
        )
        {
            var newId = Guid.NewGuid();
            var proposedMapping = _mapper.Map<ProposedMapping>(proposedMappingDto);

            proposedMapping.Id = newId;
            proposedMapping.CreatedOn = DateTime.Now;
            proposedMapping.CreatedBy = Guid.Parse(ClaimsPrincipal.Current.FindFirstValue(ClaimTypes.Sid));

            await _proposedMappingDataManager.CreateProposedMapping(proposedMapping);
            var rawNewData = _proposedMappingDataManager.FindById(newId);

            return Ok(_mapper.Map<ProposedMappingDto>(rawNewData));
        }

        /// <summary>
        /// Updates an existing proposed mapping with the data given by the dto.
        /// </summary>
        /// <param name="id">The id of the proposed mapping to update the data with.</param>
        /// <param name="proposedMappingDto">The data to update the proposed mapping with.</param>
        /// <returns>200 - The updated proposed mapping data (should be identical to the input), 404 - No proposed mapping with the given id exists.</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<ProposedMappingDto>> Patch(
            Guid id,
            [FromBody()] ProposedMappingDto proposedMappingDto
        )
        {
            var rawDataQuery = await _proposedMappingDataManager.FindById(id);
            if (rawDataQuery == null || !rawDataQuery.Any())
            {
                return NotFound("No proposed mapping with the given dto's id exists.");
            }

            var rawData = rawDataQuery.First();

            _mapper.Map(proposedMappingDto, rawData);

            await _proposedMappingDataManager.UpdateProposedMapping(rawData);
            await _proposedMappingDataManager.SaveChanges();

            return Ok(rawData);
        }
    }
}
