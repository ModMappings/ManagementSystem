using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.WebApi.Extensions;
using Data.WebApi.Model;
using Data.WebApi.Model.Api.Core;
using Data.WebApi.Model.Api.Mapping.Component;
using Data.WebApi.Services.Core;
using Mcms.Api.Data.Core.Manager.Core;
using Mcms.Api.Data.Poco.Models.Core;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.REST
{

    [Route("rest/game_version")]
    [ApiController]
    public class MappingTypeController : Controller
    {

        private readonly IMappingTypeDataManager _mappingTypeDataManager;
        private readonly IMapper _mapper;

        private readonly IUserResolvingService _userResolvingService;

        public MappingTypeController(IMappingTypeDataManager mappingTypeDataManager, IMapper mapper, IUserResolvingService userResolvingService)
        {
            _mappingTypeDataManager = mappingTypeDataManager;
            _mapper = mapper;
            _userResolvingService = userResolvingService;
        }

        /// <summary>
        /// Get method to get a given mapping type with a given id.
        /// </summary>
        /// <param name="id">The id of a mapping type to lookup.</param>
        /// <returns>200 - The mapping type with the given id, 404 - If no mapping type with the given id is found.</returns>
        [HttpGet()]
        [Route("get/{id}")]
        public async Task<ActionResult<MappingTypeDto>> Get(
            Guid id
        )
        {
            var rawResultQuery = await _mappingTypeDataManager.FindById(id);
            if (rawResultQuery == null || !rawResultQuery.Any())
            {
                return NotFound($"No mapping type exists with the given id: {id}");
            }

            var rawResult = rawResultQuery.First();

            return Ok(_mapper.Map<MappingTypeDto>(rawResult));
        }

        /// <summary>
        /// Method that looks up the mapping types based on its properties.
        /// </summary>
        /// <param name="nameRegex">A regex that filters the mapping types on their name.</param>
        /// <param name="releaseNameRegex">A regex that filters the mapping types on them having a release who's name matches the given regex.</param>
        /// <param name="pageIndex">The 0-based page index to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paged list of elements that matches the given data.</returns>
        [HttpGet()]
        [Route("list")]
        public async Task<ActionResult<PagedList<MappingTypeDto>>> List(
            [FromQuery(Name = "nameRegex")] string nameRegex = null,
            [FromQuery(Name = "releaseNameRegex")] string releaseNameRegex = null,
            [FromQuery(Name = "pageIndex")] int pageIndex = 0,
            [FromQuery(Name = "pageSize")] int pageSize = 25
        )
        {
            var rawQueryable = await _mappingTypeDataManager.FindUsingFilter(
                null,
                nameRegex,
                releaseNameRegex
            );

            return rawQueryable.ProjectTo<MappingTypeDto>(_mapper.ConfigurationProvider).AsPagedList(pageIndex, pageSize);
        }

        /// <summary>
        /// Deletes a given mapping type (and all of its related entities).
        /// </summary>
        /// <param name="id">The id of the mapping type to delete.</param>
        /// <returns>200 - Including the data of the mapping type that got deleted, 404 - If no mapping type exists with the given id.</returns>
        [HttpDelete()]
        [Route("delete/{id}")]
        public async Task<ActionResult<MappingTypeDto>> Delete(
            Guid id
        )
        {
            var rawData = await _mappingTypeDataManager.FindById(id);
            if (rawData == null || !rawData.Any())
            {
                return NotFound($"No mapping type can be found with a given id: {id}");
            }

            var target = rawData.First();

            await _mappingTypeDataManager.DeleteMappingType(target);
            await _mappingTypeDataManager.SaveChanges();

            return Ok(_mapper.Map<MappingTypeDto>(target));
        }

        /// <summary>
        /// Creates a new mapping type from the given data.
        /// The api will create a new Id.
        ///
        /// The system returns the data after it has been saved in the database.
        /// </summary>
        /// <param name="mappingTypeDto">The data to create the new mapping type from.</param>
        /// <returns>200 - The updated mapping type data (should be identical to the input), with the id set.</returns>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<MappingTypeDto>> Create(
            [FromBody()] MappingTypeDto mappingTypeDto
        )
        {
            var newId = Guid.NewGuid();
            var mappingType = _mapper.Map<MappingType>(mappingTypeDto);

            mappingType.Id = newId;
            mappingType.CreatedOn = DateTime.Now;
            mappingType.CreatedBy = Guid.Parse(ClaimsPrincipal.Current.FindFirstValue(ClaimTypes.Sid));

            await _mappingTypeDataManager.CreateMappingType(mappingType);
            var rawNewData = _mappingTypeDataManager.FindById(newId);

            return Ok(_mapper.Map<MappingTypeDto>(rawNewData));
        }

        /// <summary>
        /// Updates an existing mapping type with the data given by the dto.
        /// </summary>
        /// <param name="id">The id of the mapping type to update the data with.</param>
        /// <param name="mappingTypeDto">The data to update the mapping type with.</param>
        /// <returns>200 - The updated mapping type data (should be identical to the input), 404 - No mapping type with the given id exists.</returns>
        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult<MappingTypeDto>> Patch(
            Guid id,
            [FromBody()] MappingTypeDto mappingTypeDto
        )
        {
            var rawDataQuery = await _mappingTypeDataManager.FindById(id);
            if (rawDataQuery == null || !rawDataQuery.Any())
            {
                return NotFound("No mapping type with the given dto's id exists.");
            }

            var rawData = rawDataQuery.First();

            _mapper.Map(mappingTypeDto, rawData);

            await _mappingTypeDataManager.UpdateMappingType(rawData);
            await _mappingTypeDataManager.SaveChanges();

            return Ok(rawData);
        }
    }
}
