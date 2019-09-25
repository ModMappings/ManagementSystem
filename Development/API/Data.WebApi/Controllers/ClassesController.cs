using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.Core.Readers.Core;
using Data.Core.Writers.Mapping;
using Data.EFCore.Writer.Mapping;
using Data.WebApi.Model.Creation.Class;
using Data.WebApi.Model.Read.Class;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.WebApi.Controllers
{
    /// <summary>
    /// Controller that handles interactions on class levels.
    /// </summary>
    [Route("/classes")]
    [ApiController]
    public class ClassesController : ComponentControllerBase<ClassReadModel, ClassVersionedReadModel>
    {

        public ClassesController(IClassComponentWriter classComponentWriter, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(classComponentWriter, releaseReader, gameVersionReader, userResolvingService, mappingTypeReader)
        {
            ClassComponentWriter = classComponentWriter;
        }

        private IClassComponentWriter ClassComponentWriter { get; }

        /// <summary>
        /// Gets the classes which are part of a given package (or who's packages match the regex pattern given)
        /// those classes also need to have at least one mapping (regardless of type) within the latest gameversion.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="packagePattern">The regex pattern to match packages to.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The classes who's packages match the pattern, and are part of the latest gameversion.</returns>
        [HttpGet("package/version/{packagePattern}/latest/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ClassReadModel>>> GetByPackageInLatestGameVersion(string packagePattern, int pageSize, int pageIndex)
        {
            var dbModels = await ClassComponentWriter.GetByPackageInLatestGameVersion(packagePattern);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Counts the classes which are part of a given package (or who's packages match the regex pattern given)
        /// those classes also need to have at least one mapping (regardless of type) within the latest gameversion.
        /// </summary>
        /// <param name="packagePattern">The regex pattern to match packages to.</param>
        /// <returns>The count of classes who's packages match the pattern, and are part of the latest gameversion.</returns>
        [HttpGet("package/version/{packagePattern}/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountPackageInLatestGameVersion(string packagePattern)
        {
            var dbModels = await ClassComponentWriter.GetByPackageInLatestGameVersion(packagePattern);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the classes which are part of a given package (or who's packages match the regex pattern given)
        /// those classes also need to have at least one mapping (regardless of type) within a gameversion that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="packagePattern">The regex pattern to match packages to.</param>
        /// <param name="gameVersionId">The id of the gameversion that the component needs to be in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The classes who's packages match the pattern, and are part of the given gameversion.</returns>
        [HttpGet("package/version/{packagePattern}/{gameVersionId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ClassReadModel>>> GetByPackageInGameVersion(string packagePattern, Guid gameVersionId, int pageSize, int pageIndex)
        {
            var dbModels = await ClassComponentWriter.GetByPackageInGameVersion(packagePattern, gameVersionId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Counts the classes which are part of a given package (or who's packages match the regex pattern given)
        /// those classes also need to have at least one mapping (regardless of type) within a gameversion that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="packagePattern">The regex pattern to match packages to.</param>
        /// <param name="gameVersionId">The id of the gameversion that the component needs to be in.</param>
        /// <returns>The count of classes who's packages match the pattern, and are part of the given gameversion.</returns>
        [HttpGet("package/version/{packagePattern}/{gameVersionId}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountPackageInGameVersion(string packagePattern, Guid gameVersionId)
        {
            var dbModels = await ClassComponentWriter.GetByPackageInGameVersion(packagePattern, gameVersionId);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the classes which are part of a given package (or who's packages match the regex pattern given)
        /// those classes also need to have at least one mapping (regardless of type) within the latest release.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="packagePattern">The regex pattern to match packages to.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The classes who's packages match the pattern, and are part of the latest release.</returns>
        [HttpGet("package/release/{packagePattern}/latest/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ClassReadModel>>> GetByPackageInLatestRelease(string packagePattern, int pageSize, int pageIndex)
        {
            var dbModels = await ClassComponentWriter.GetByPackageInLatestRelease(packagePattern);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Counts the classes which are part of a given package (or who's packages match the regex pattern given)
        /// those classes also need to have at least one mapping (regardless of type) within the latest release.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="packagePattern">The regex pattern to match packages to.</param>
        /// <returns>The count of  classes who's packages match the pattern, and are part of the latest release.</returns>
        [HttpGet("package/release/{packagePattern}/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountPackageInLatestRelease(string packagePattern)
        {
            var dbModels = await ClassComponentWriter.GetByPackageInLatestRelease(packagePattern);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the classes which are part of a given package (or who's packages match the regex pattern given)
        /// those classes also need to have at least one mapping (regardless of type) within a release that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="packagePattern">The regex pattern to match packages to.</param>
        /// <param name="releaseId">The id of the release that the component needs to be in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The classes who's packages match the pattern, and are part of the given release.</returns>
        [HttpGet("package/release/{packagePattern}/{releaseId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ClassReadModel>>> GetByPackageInRelease(string packagePattern, Guid releaseId, int pageSize, int pageIndex)
        {
            var dbModels = await ClassComponentWriter.GetByPackageInRelease(packagePattern, releaseId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Counts the classes which are part of a given package (or who's packages match the regex pattern given)
        /// those classes also need to have at least one mapping (regardless of type) within a release that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="packagePattern">The regex pattern to match packages to.</param>
        /// <param name="releaseId">The id of the release that the component needs to be in.</param>
        /// <returns>The count of classes who's packages match the pattern, and are part of the given release.</returns>
        [HttpGet("package/release/{packagePattern}/{releaseId}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByPackageInRelease(string packagePattern, Guid releaseId)
        {
            var dbModels = await ClassComponentWriter.GetByPackageInRelease(packagePattern, releaseId);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Creates a new class and its central mapping entry.
        /// Creates a new core mapping class, a versioned mapping class for the latest version, as well a single committed mapping.
        /// </summary>
        /// <param name="mapping">The mapping to create.</param>
        /// <returns>An http response code:201-New mapping created,400-The request was invalid,404-Certain data could not be found,401-Unauthorized.</returns>
        [HttpPost("add/version/latest")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> AddToLatest([FromBody] CreateClassModel mapping)
        {
            var currentLatestGameVersion = await GameVersionReader.GetLatest();
            if (currentLatestGameVersion == null)
                return BadRequest("No game version has been registered yet.");

            var user = await UserResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent outer = null;
            if (mapping.Outer.HasValue)
            {
                outer = await ComponentWriter.GetVersionedComponent(mapping.Outer.Value);
                if (outer == null)
                    return BadRequest("Unknown outer class");
            }

            var inheritsFrom =
                (await Task.WhenAll(
                    mapping.InheritsFrom.Select(async id => (await ComponentWriter.GetVersionedComponent(id)).Metadata as ClassMetadata))).ToList();

            if (inheritsFrom.Any(m => m == null))
                return BadRequest("Unknown inheriting class.");

            var versionedClassMapping = new VersionedComponent
            {
                CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                GameVersion = currentLatestGameVersion,
                Mappings = new List<LiveMappingEntry>(),
                Proposals = new List<ProposalMappingEntry>()
            };

            versionedClassMapping.Metadata = new ClassMetadata
            {
                VersionedComponent = versionedClassMapping,
                VersionedComponentForeignKey = versionedClassMapping.Id,
                Outer=outer?.Metadata as ClassMetadata,
                Package = mapping.Package,
                InheritsFrom = inheritsFrom,
            };

            var initialLiveMappings = mapping.Mappings
                .Select(mappingData => new LiveMappingEntry()
                {
                    Documentation = mappingData.Documentation,
                    Distribution = mappingData.Distribution,
                    InputMapping = mappingData.In,
                    OutputMapping = mappingData.Out,
                    MappingType = MappingTypeReader.GetByName(mappingData.MappingTypeName).Result,
                    Proposal = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = versionedClassMapping,
                    CreatedOn = DateTime.Now
                });

            versionedClassMapping.Mappings.AddRange(initialLiveMappings);

            var classMapping = new Component
            {
                Id = Guid.NewGuid(),
                Type = ComponentType.CLASS,
                VersionedComponents = new List<VersionedComponent>() {versionedClassMapping}
            };

            await ComponentWriter.Add(classMapping);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", classMapping.Id, classMapping);
        }

        /// <summary>
        /// Creates a new versioned class entry for an already existing class mapping.
        /// Creates a versioned mapping class for the given version, as well a single committed mapping.
        /// </summary>
        /// <param name="mapping">The versioned mapping to create.</param>
        /// <returns>An http response code:201-New mapping created,400-The request was invalid,404-Certain data could not be found,401-Unauthorized,409-A versioned class for that version already exists with the class.</returns>
        [HttpPost("add/version/{versionId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> AddToVersion([FromBody] CreateVersionedClassModel mapping)
        {
            var currentGameVersion = await GameVersionReader.GetById(mapping.GameVersion);
            if (currentGameVersion == null)
                return BadRequest("No game version with that id has been registered yet.");

            var user = await UserResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent outer = null;
            if (mapping.Outer.HasValue)
            {
                outer = await ComponentWriter.GetVersionedComponent(mapping.Outer.Value);
                if (outer == null)
                    return BadRequest("Unknown outer class");
            }

            var inheritsFrom =
                (await Task.WhenAll(
                    mapping.InheritsFrom.Select(async id => (await ComponentWriter.GetVersionedComponent(id)).Metadata as ClassMetadata))).ToList();

            if (inheritsFrom.Any(m => m == null))
                return BadRequest("Unknown inheriting class.");

            var classMapping = await ComponentWriter.GetById(mapping.VersionedMappingFor);
            if (classMapping == null)
                return BadRequest("Unknown class mapping to create the versioned mapping for.");

            if (classMapping.VersionedComponents.Any(versionedMapping =>
                versionedMapping.GameVersion.Id == mapping.GameVersion))
                return Conflict();

            var versionedClassMapping = new VersionedComponent
            {
                CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                GameVersion = currentGameVersion,
                Mappings = new List<LiveMappingEntry>(),
                Proposals = new List<ProposalMappingEntry>()
            };

            versionedClassMapping.Metadata = new ClassMetadata
            {
                VersionedComponent = versionedClassMapping,
                VersionedComponentForeignKey = versionedClassMapping.Id,
                Outer=outer?.Metadata as ClassMetadata,
                Package = mapping.Package,
                InheritsFrom = inheritsFrom,
            };

            var initialLiveMappings = mapping.Mappings
                .Select(mappingData => new LiveMappingEntry()
                {
                    Documentation = mappingData.Documentation,
                    Distribution = mappingData.Distribution,
                    InputMapping = mappingData.In,
                    OutputMapping = mappingData.Out,
                    MappingType = MappingTypeReader.GetByName(mappingData.MappingTypeName).Result,
                    Proposal = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = versionedClassMapping,
                    CreatedOn = DateTime.Now
                });

            versionedClassMapping.Mappings.AddRange(initialLiveMappings);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", classMapping.Id, classMapping);
        }

        protected override ClassReadModel ConvertDbModelToReadModel(Component component)
        {
            return new ClassReadModel
            {
                Id = component.Id,
                Versioned = component.VersionedComponents.ToList().Select(ConvertVersionedDbModelToReadModel)
            };
        }

        protected override ClassVersionedReadModel ConvertVersionedDbModelToReadModel(VersionedComponent versionedComponent)
        {
            var outerId = (versionedComponent.Metadata as ClassMetadata)?.Outer?.VersionedComponent.Id;

            return new ClassVersionedReadModel
            {
                Id = versionedComponent.Id,
                VersionedViewModelFor = versionedComponent.Component.Id,
                GameVersion = versionedComponent.GameVersion.Id,
                Outer = outerId,
                Package = (versionedComponent.Metadata as ClassMetadata)?.Package,
                InheritsFrom = (versionedComponent.Metadata as ClassMetadata)?.InheritsFrom.ToList().Select(parentClass => parentClass.VersionedComponent.Id),
                CurrentMappings = versionedComponent.Mappings.ToList().Select(ConvertLiveDbModelToMappingReadModel),
                Proposals = versionedComponent.Proposals.ToList().Select(ConvertProposalDbModelToProposalReadModel),
                LockedMappingNames = versionedComponent.LockedMappingTypes.ToList().Select(lm => lm.MappingType.Name)
            };
        }
    }
}
