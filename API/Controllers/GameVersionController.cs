using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using API.Model.Creation.Core;
using API.Model.Read.Core;
using API.Services.Core;
using Data.Core.Models.Core;
using Data.Core.Writers.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// A controller for access to game version.
    /// </summary>
    [ApiController()]
    [Route("/gameversions")]
    public class GameVersionController : Controller
    {

        /// <summary>
        /// The game version reader and writer.
        /// </summary>
        private readonly IGameVersionWriter _gameVersionWriter;

        /// <summary>
        /// The user resolving service.
        /// </summary>
        private readonly IUserResolvingService _userResolvingService;

        /// <summary>
        /// Creates a new game version controller.
        /// </summary>
        /// <param name="gameVersionWriter">The writer and reader for game versions.</param>
        /// <param name="userResolvingService">The user resolving service.</param>
        public GameVersionController(IGameVersionWriter gameVersionWriter, IUserResolvingService userResolvingService)
        {
            _gameVersionWriter = gameVersionWriter;
            _userResolvingService = userResolvingService;
        }

        /// <summary>
        /// Allows look up of the game versions by a given id.
        /// </summary>
        /// <param name="id">The id of the game version that wants to be looked up.</param>
        /// <returns>The game version api model that reflects the lookup game version table entry.</returns>
        [HttpGet("id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<GameVersionReadModel>> GetById(Guid id)
        {
            var dbModel = await _gameVersionWriter.GetById(id);
            if (dbModel == null)
                return NotFound();

            return ConvertDbModelToApiModel(dbModel);
        }

        /// <summary>
        /// Allows look up of the game versions by a given name.
        /// </summary>
        /// <param name="name">The name of the game version that wants to be looked up.</param>
        /// <returns>The game version api model that reflects the lookup game version table entry.</returns>
        [HttpGet("name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<GameVersionReadModel>> GetByName(string name)
        {
            var dbModel = await _gameVersionWriter.GetByName(name);
            if (dbModel == null)
                return NotFound();

            return ConvertDbModelToApiModel(dbModel);
        }

        /// <summary>
        /// Allows for the lookup of the entire game versions table.
        /// </summary>
        /// <returns>All known game versions.</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<GameVersionReadModel>>> AsQueryable()
        {
            var dbModels = await _gameVersionWriter.AsQueryable();

            return  Json(dbModels.ToList().Select(ConvertDbModelToApiModel));
        }

        /// <summary>
        /// Allows for the lookup of the entire game versions table, filtered by the fact if they are full releases or not.
        /// </summary>
        /// <returns>All known full game versions.</returns>
        [HttpGet("releases/full")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<GameVersionReadModel>>> GetAllFullReleases()
        {
            var dbModels = await _gameVersionWriter.GetAllFullReleases();

            return  Json(dbModels.ToList().Select(ConvertDbModelToApiModel));
        }

        /// <summary>
        /// Allows for the lookup of the entire game versions table, filtered by the fact if they are pre releases or not.
        /// </summary>
        /// <returns>All known pre release game versions.</returns>
        [HttpGet("releases/pre")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<GameVersionReadModel>>> GetAllPreReleases()
        {
            var dbModels = await _gameVersionWriter.GetAllPreReleases();

            return  Json(dbModels.ToList().Select(ConvertDbModelToApiModel));
        }

        /// <summary>
        /// Allows for the lookup of the entire game versions table, filtered by the fact if they are pre releases or not.
        /// </summary>
        /// <returns>All known pre release game versions.</returns>
        [HttpGet("releases/snapshot")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<GameVersionReadModel>>> GetAllSnapshots()
        {
            var dbModels = await _gameVersionWriter.GetAllSnapshots();

            return  Json(dbModels.ToList().Select(ConvertDbModelToApiModel));
        }

        /// <summary>
        /// Allows for the lookup of the latest game version.
        /// </summary>
        /// <returns>The latest game version.</returns>
        [HttpGet("latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<GameVersionReadModel>> GetLatest()
        {
            var dbModel = await _gameVersionWriter.GetLatest();
            if (dbModel == null)
                return BadRequest();

            return ConvertDbModelToApiModel(dbModel);
        }

        /// <summary>
        /// Allows for the creation of a new game version.
        /// </summary>
        [HttpPost("latest")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> Add([FromBody] CreateGameVersionModel mapping)
        {
            var user = await _userResolvingService.Get();
            if (user == null || !user.CanCreateGameVersions)
                return Unauthorized();

            var gameVersion = new GameVersion
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                IsPreRelease = mapping.IsPreRelease,
                IsSnapshot = mapping.IsSnapshot,
                Name = mapping.Name,
                Id = Guid.NewGuid()
            };

            await _gameVersionWriter.Add(gameVersion);
            await _gameVersionWriter.SaveChanges();

            return CreatedAtAction("GetById", gameVersion.Id, gameVersion);
        }

        private GameVersionReadModel ConvertDbModelToApiModel(GameVersion gameVersion)
        {
            return new GameVersionReadModel
            {
                Id = gameVersion.Id,
                CreatedBy = gameVersion.CreatedBy.Id,
                CreatedOn = gameVersion.CreatedOn,
                IsPreRelease = gameVersion.IsPreRelease,
                IsSnapshot = gameVersion.IsSnapshot,
                Name = gameVersion.Name
            };
        }
    }
}
