using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.WebApi.Extensions;
using Data.WebApi.Model;
using Data.WebApi.Services.Core;
using Mcms.Api.Business.Poco.Api.REST.Core;
using Mcms.Api.Data.Core.Manager.Core;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.REST
{

    [Route("rest/release")]
    [ApiController]
    public class ReleaseController : Controller
    {

        private readonly IReleaseDataManager _releaseDataManager;
        private readonly IMapper _mapper;

        private readonly IUserResolvingService _userResolvingService;

        public ReleaseController(IReleaseDataManager releaseDataManager, IMapper mapper, IUserResolvingService userResolvingService)
        {
            _releaseDataManager = releaseDataManager;
            _mapper = mapper;
            _userResolvingService = userResolvingService;
        }

        /// <summary>
        /// Get method to get a given release with a given id.
        /// </summary>
        /// <param name="id">The id of a release to lookup.</param>
        /// <returns>200 - The release with the given id, 404 - If no release with the given id is found.</returns>
        [HttpGet()]
        [Route("get/{id}")]
        public async Task<ActionResult<ReleaseDto>> Get(
            Guid id
        )
        {
            var rawResultQuery = await _releaseDataManager.FindById(id);
            if (rawResultQuery == null || !rawResultQuery.Any())
            {
                return NotFound($"No release exists with the given id: {id}");
            }

            var rawResult = rawResultQuery.First();

            return Ok(_mapper.Map<ReleaseDto>(rawResult));
        }

        /// <summary>
        /// Method that looks up the releases based on its properties.
        /// </summary>
        /// <param name="nameRegex">A regex that filters the releases on their name.</param>
        /// <param name="mappingTypeNameRegex">A regex that filters the releases on their mapping type. The name of the mapping type needs to match the given regex.</param>
        /// <param name="gameVersionNameRegex">A regex that filters the releases on their game version. The name of the game version needs to match the given regex.</param>
        /// <param name="pageIndex">The 0-based page index to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paged list of elements that matches the given data.</returns>
        [HttpGet()]
        [Route("list")]
        public async Task<ActionResult<PagedList<ReleaseDto>>> List(
            [FromQuery(Name = "nameRegex")] string nameRegex = null,
            [FromQuery(Name = "mappingTypeRegex")] string mappingTypeNameRegex = null,
            [FromQuery(Name = "gameVersionNameRegex")] string gameVersionNameRegex = null,
            [FromQuery(Name = "pageIndex")] int pageIndex = 0,
            [FromQuery(Name = "pageSize")] int pageSize = 25
        )
        {
            var rawQueryable = await _releaseDataManager.FindUsingFilter(
                null,
                nameRegex,
                mappingTypeNameRegex,
                gameVersionNameRegex
            );

            return rawQueryable.ProjectTo<ReleaseDto>(_mapper.ConfigurationProvider).AsPagedList(pageIndex, pageSize);
        }

        /// <summary>
        /// Deletes a given release (and all of its related entities).
        /// </summary>
        /// <param name="id">The id of the release to delete.</param>
        /// <returns>200 - Including the data of the release that got deleted, 404 - If no release exists with the given id.</returns>
        [HttpDelete()]
        [Route("delete/{id}")]
        public async Task<ActionResult<ReleaseDto>> Delete(
            Guid id
        )
        {
            var rawData = await _releaseDataManager.FindById(id);
            if (rawData == null || !rawData.Any())
            {
                return NotFound($"No release can be found with a given id: {id}");
            }

            var target = rawData.First();

            await _releaseDataManager.DeleteRelease(target);
            await _releaseDataManager.SaveChanges();

            return Ok(_mapper.Map<ReleaseDto>(target));
        }

        /// <summary>
        /// Creates a new release from the given data.
        /// The api will create a new Id.
        ///
        /// The system returns the data after it has been saved in the database.
        /// </summary>
        /// <param name="releaseDto">The data to create the new release from.</param>
        /// <returns>200 - The updated release data (should be identical to the input), with the id set.</returns>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<ReleaseDto>> Create(
            [FromBody()] ReleaseDto releaseDto
        )
        {
            var newId = Guid.NewGuid();
            var release = _mapper.Map<Release>(releaseDto);

            release.Id = newId;
            release.CreatedOn = DateTime.Now;
            release.CreatedBy = Guid.Parse(ClaimsPrincipal.Current.FindFirstValue(ClaimTypes.Sid));

            await _releaseDataManager.CreateRelease(release);
            var rawNewData = _releaseDataManager.FindById(newId);

            return Ok(_mapper.Map<ReleaseDto>(rawNewData));
        }

        /// <summary>
        /// Updates an existing release with the data given by the dto.
        /// </summary>
        /// <param name="id">The id of the release to update the data with.</param>
        /// <param name="releaseDto">The data to update the release with.</param>
        /// <returns>200 - The updated release data (should be identical to the input), 404 - No release with the given id exists.</returns>
        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult<ReleaseDto>> Patch(
            Guid id,
            [FromBody()] ReleaseDto releaseDto
        )
        {
            var rawDataQuery = await _releaseDataManager.FindById(id);
            if (rawDataQuery == null || !rawDataQuery.Any())
            {
                return NotFound("No release with the given dto's id exists.");
            }

            var rawData = rawDataQuery.First();

            _mapper.Map(releaseDto, rawData);

            await _releaseDataManager.UpdateRelease(rawData);
            await _releaseDataManager.SaveChanges();

            return Ok(rawData);
        }
    }
}
