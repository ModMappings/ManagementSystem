using System;
using System.Linq;
using System.Threading.Tasks;
using API.Model.Creation.Core;
using API.Model.Read.Core;
using API.Services.Core;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Readers.Core;
using Data.Core.Writers.Core;
using Data.EFCore.Writer.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// A controller for access to game version.
    /// </summary>
    [ApiController()]
    [Route("/releases")]
    public class ReleaseController : Controller
    {
        /// <summary>
        /// The game version reader and writer.
        /// </summary>
        private readonly IReleaseWriter _releaseWriter;

        /// <summary>
        /// The user resolving service.
        /// </summary>
        private readonly IUserResolvingService _userResolvingService;

        /// <summary>
        /// Game versoin reader.
        /// </summary>
        private readonly IGameVersionReader _gameVersionReader;

        private readonly ClassWriter _classes;

        private readonly MethodWriter _methods;

        private readonly FieldWriter _fields;

        private readonly ParameterWriter _parameters;

        /// <summary>
        /// Creates a new game version controller.
        /// </summary>
        /// <param name="releaseWriter">The writer and reader for releases.</param>
        /// <param name="userResolvingService">The user resolving service.</param>
        /// <param name="gameVersionReader">The reader for releases.</param>
        /// <param name="classes">The class mapping reader.</param>
        /// <param name="methods">The method mapping reader.</param>
        /// <param name="fields">The field mapping reader.</param>
        /// <param name="parameters">The parameter mapping reader.</param>
        public ReleaseController(IReleaseWriter releaseWriter, IUserResolvingService userResolvingService,
            IGameVersionReader gameVersionReader, ClassWriter classes, MethodWriter methods, FieldWriter fields, ParameterWriter parameters)
        {
            _releaseWriter = releaseWriter;
            _userResolvingService = userResolvingService;
            _gameVersionReader = gameVersionReader;
            _classes = classes;
            _methods = methods;
            _fields = fields;
            _parameters = parameters;
        }

        /// <summary>
        /// Allows look up of the releases by a given id.
        /// </summary>
        /// <param name="id">The id of the game version that wants to be looked up.</param>
        /// <returns>The game version api model that reflects the lookup game version table entry.</returns>
        [HttpGet("id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<ReleaseReadModel>> GetById(Guid id)
        {
            var dbModel = await _releaseWriter.GetById(id);
            if (dbModel == null)
                return NotFound();

            return ConvertDbModelToApiModel(dbModel);
        }

        /// <summary>
        /// Allows look up of the releases by a given name.
        /// </summary>
        /// <param name="name">The name of the game version that wants to be looked up.</param>
        /// <returns>The game version api model that reflects the lookup game version table entry.</returns>
        [HttpGet("name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<ReleaseReadModel>> GetByName(string name)
        {
            var dbModel = await _releaseWriter.GetByName(name);
            if (dbModel == null)
                return NotFound();

            return ConvertDbModelToApiModel(dbModel);
        }

        /// <summary>
        /// Allows for the lookup of the entire releases table.
        /// </summary>
        /// <returns>All known releases.</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<ReleaseReadModel>>> AsQueryable()
        {
            var dbModels = await _releaseWriter.AsQueryable();

            return Json(dbModels.ToList().Select(ConvertDbModelToApiModel));
        }

        /// <summary>
        /// Gives access to all releases created by a given user.
        /// </summary>
        /// <param name="userId">The id of the user who created the releases in question.</param>
        /// <returns>The releases created by the user in question.</returns>
        [HttpGet("made/by/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<Release>>> GetMadeBy(Guid userId)
        {
            var dbModels = await _releaseWriter.GetMadeBy(userId);

            return Json(dbModels.ToList().Select(ConvertDbModelToApiModel));
        }

        /// <summary>
        /// Gives access to all releases created on a given date.
        /// </summary>
        /// <param name="date">The date that the release was created in question.</param>
        /// <returns>The releases created on the date in question.</returns>
        [HttpGet("made/on/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<Release>>> GetMadeOn(DateTime date)
        {
            var dbModels = await _releaseWriter.GetMadeOn(date.Date);

            return Json(dbModels.ToList().Select(ConvertDbModelToApiModel));
        }

        /// <summary>
        /// Gives access to all releases made for a given game version.
        /// </summary>
        /// <param name="id">The id of the game version in question.</param>
        /// <returns>The releases created for the given release.</returns>
        [HttpGet("made/for/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<Release>>> GetMadeForVersion(Guid id)
        {
            var dbModels = await _releaseWriter.GetMadeForVersion(id);

            return Json(dbModels.ToList().Select(ConvertDbModelToApiModel));
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
        public async Task<ActionResult<ReleaseReadModel>> GetLatest()
        {
            var dbModel = await _releaseWriter.GetLatest();
            if (dbModel == null)
                return BadRequest();

            return ConvertDbModelToApiModel(dbModel);
        }

        /// <summary>
        /// Allows for the creation of a release, from a given name and game version.
        /// Collects all current mappings in that version in one release.
        /// </summary>
        [HttpPost("latest")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> Add([FromBody] CreateReleaseModel mapping)
        {
            var user = await _userResolvingService.Get();
            if (user == null || !user.CanRelease)
                return Unauthorized();

            var gameVersion = await _gameVersionReader.GetById(mapping.GameVersion);

            var release = new Release
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                Name = mapping.Name,
                Id = Guid.NewGuid(),
                GameVersion = gameVersion
            };

            release.Classes = (await _classes.GetByVersion(gameVersion)).Select(classMapping =>
                classMapping.VersionedMappings
                    .FirstOrDefault(versionedMapping => versionedMapping.GameVersion == gameVersion).Mappings
                    .OrderByDescending(committedMappings => committedMappings.CreatedOn).FirstOrDefault()).Select(
                committedMapping => new ReleaseComponent
                {
                    Id = new Guid(),
                    ComponentType = ComponentType.CLASS,
                    Release = release,
                    Member = committedMapping
                }).ToList();

            release.Methods = (await _methods.GetByVersion(gameVersion)).Select(methodMapping =>
                methodMapping.VersionedMappings
                    .FirstOrDefault(versionedMapping => versionedMapping.GameVersion == gameVersion).Mappings
                    .OrderByDescending(committedMappings => committedMappings.CreatedOn).FirstOrDefault()).Select(
                committedMapping => new ReleaseComponent()
                {
                    Id = new Guid(),
                    ComponentType = ComponentType.METHOD,
                    Release = release,
                    Member = committedMapping
                }).ToList();

            release.Fields = (await _fields.GetByVersion(gameVersion)).Select(fieldMapping =>
                fieldMapping.VersionedMappings
                    .FirstOrDefault(versionedMapping => versionedMapping.GameVersion == gameVersion).Mappings
                    .OrderByDescending(committedMappings => committedMappings.CreatedOn).FirstOrDefault()).Select(
                committedMapping => new ReleaseComponent()
                {
                    Id = new Guid(),
                    ComponentType = ComponentType.FIELD,
                    Release = release,
                    Member = committedMapping
                }).ToList();

            release.Parameters = (await _parameters.GetByVersion(gameVersion)).Select(parameterMapping =>
                parameterMapping.VersionedMappings
                    .FirstOrDefault(versionedMapping => versionedMapping.GameVersion == gameVersion).Mappings
                    .OrderByDescending(committedMappings => committedMappings.CreatedOn).FirstOrDefault()).Select(
                committedMapping => new ReleaseComponent()
                {
                    Id = new Guid(),
                    ComponentType = ComponentType.FIELD,
                    Release = release,
                    Member = committedMapping
                }).ToList();

            await _releaseWriter.Add(release);
            await _releaseWriter.SaveChanges();

            return CreatedAtAction("GetById", release.Id, release);
        }

        private ReleaseReadModel ConvertDbModelToApiModel(Release release)
        {
            return new ReleaseReadModel
            {
                Id = release.Id,
                CreatedBy = release.CreatedBy.Id,
                CreatedOn = release.CreatedOn,
                Name = release.Name,
                Classes = release.Classes.Select(member => member.Id).ToList(),
                Methods = release.Methods.Select(member => member.Id).ToList(),
                Parameters = release.Parameters.Select(member => member.Id).ToList(),
                Fields = release.Fields.Select(member => member.Id).ToList(),
            };
        }
    }
}
