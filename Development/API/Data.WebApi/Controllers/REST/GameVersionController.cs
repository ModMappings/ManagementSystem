using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.WebApi.Extensions;
using Data.WebApi.Model;
using Data.WebApi.Services.Core;
using Mcms.Api.Business.Poco.Api.REST.Core;
using Mcms.Api.Data.Core.Manager.Core;
using Mcms.Api.Data.Poco.Models.Core;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.REST
{

    [Route("rest/game_version")]
    [ApiController]
    public class GameVersionController : Controller
    {

        private readonly IGameVersionDataManager _gameVersionDataManager;
        private readonly IMapper _mapper;

        private readonly IUserResolvingService _userResolvingService;

        public GameVersionController(IGameVersionDataManager gameVersionDataManager, IMapper mapper, IUserResolvingService userResolvingService)
        {
            _gameVersionDataManager = gameVersionDataManager;
            _mapper = mapper;
            _userResolvingService = userResolvingService;
        }

        /// <summary>
        /// Get method to get a given game version with a given id.
        /// </summary>
        /// <param name="id">The id of a game version to lookup.</param>
        /// <returns>200 - The game version with the given id, 404 - If no game version with the given id is found.</returns>
        [HttpGet()]
        [Route("get/{id}")]
        public async Task<ActionResult<GameVersionDto>> Get(
            Guid id
        )
        {
            var rawResultQuery = await _gameVersionDataManager.FindById(id);
            if (rawResultQuery == null || !rawResultQuery.Any())
            {
                return NotFound($"No game version exists with the given id: {id}");
            }

            var rawResult = rawResultQuery.First();

            return Ok(_mapper.Map<GameVersionDto>(rawResult));
        }

        /// <summary>
        /// Method that looks up the game versions based on its properties.
        /// </summary>
        /// <param name="nameRegex">A regex that filters the game versions on their name.</param>
        /// <param name="pageIndex">The 0-based page index to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paged list of elements that matches the given data.</returns>
        [HttpGet()]
        [Route("list")]
        public async Task<ActionResult<PagedList<GameVersionDto>>> List(
            [FromQuery(Name = "nameRegex")] string nameRegex = null,
            [FromQuery(Name = "pageIndex")] int pageIndex = 0,
            [FromQuery(Name = "pageSize")] int pageSize = 25
        )
        {
            var rawQueryable = await _gameVersionDataManager.FindUsingFilter(
                null,
                nameRegex
            );

            return rawQueryable.ProjectTo<GameVersionDto>(_mapper.ConfigurationProvider).AsPagedList(pageIndex, pageSize);
        }

        /// <summary>
        /// Deletes a given game version (and all of its related entities).
        /// </summary>
        /// <param name="id">The id of the game version to delete.</param>
        /// <returns>200 - Including the data of the game version that got deleted, 404 - If no game version exists with the given id.</returns>
        [HttpDelete()]
        [Route("delete/{id}")]
        public async Task<ActionResult<GameVersionDto>> Delete(
            Guid id
        )
        {
            var rawData = await _gameVersionDataManager.FindById(id);
            if (rawData == null || !rawData.Any())
            {
                return NotFound($"No game version can be found with a given id: {id}");
            }

            var target = rawData.First();

            await _gameVersionDataManager.DeleteGameVersion(target);
            await _gameVersionDataManager.SaveChanges();

            return Ok(_mapper.Map<GameVersionDto>(target));
        }

        /// <summary>
        /// Creates a new game version from the given data.
        /// The api will create a new Id.
        ///
        /// The system returns the data after it has been saved in the database.
        /// </summary>
        /// <param name="gameversionDto">The data to create the new game version from.</param>
        /// <returns>200 - The updated game version data (should be identical to the input), with the id set.</returns>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<GameVersionDto>> Create(
            [FromBody()] GameVersionDto gameversionDto
        )
        {
            var newId = Guid.NewGuid();
            var gameVersion = _mapper.Map<GameVersion>(gameversionDto);

            gameVersion.Id = newId;
            gameVersion.CreatedOn = DateTime.Now;
            gameVersion.CreatedBy = Guid.Parse(ClaimsPrincipal.Current.FindFirstValue(ClaimTypes.Sid));

            await _gameVersionDataManager.CreateGameVersion(gameVersion);
            var rawNewData = _gameVersionDataManager.FindById(newId);

            return Ok(_mapper.Map<GameVersionDto>(rawNewData));
        }

        /// <summary>
        /// Updates an existing game version with the data given by the dto.
        /// </summary>
        /// <param name="id">The id of the game version to update the data with.</param>
        /// <param name="gameversionDto">The data to update the game version with.</param>
        /// <returns>200 - The updated game version data (should be identical to the input), 404 - No game version with the given id exists.</returns>
        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult<GameVersionDto>> Patch(
            Guid id,
            [FromBody()] GameVersionDto gameversionDto
        )
        {
            var rawDataQuery = await _gameVersionDataManager.FindById(id);
            if (rawDataQuery == null || !rawDataQuery.Any())
            {
                return NotFound("No game version with the given dto's id exists.");
            }

            var rawData = rawDataQuery.First();

            _mapper.Map(gameversionDto, rawData);

            await _gameVersionDataManager.UpdateGameVersion(rawData);
            await _gameVersionDataManager.SaveChanges();

            return Ok(rawData);
        }
    }
}
