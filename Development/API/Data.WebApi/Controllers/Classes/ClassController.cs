using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.Core.Readers.Core;
using Data.Core.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Extensions;
using Data.WebApi.Model.Creation.Class;
using Data.WebApi.Model.Read.Class;
using Data.WebApi.Model.Read.Core;
using Data.WebApi.Services.Converters;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Classes
{
    /// <summary>
    /// Controller that handles interactions on class levels.
    /// </summary>
    [Route("/classes")]
    [ApiController]
    public class ClassController : ComponentControllerBase<ClassReadModel, ClassVersionedReadModel>
    {

        public ClassController(IClassComponentWriter classComponentWriter, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(classComponentWriter, releaseReader, gameVersionReader, userResolvingService, mappingTypeReader)
        {
            ClassComponentWriter = classComponentWriter;
        }

        private IClassComponentWriter ClassComponentWriter { get; }

        /// <summary>
        /// Gets the classes which are part of a given package (or who's packages match the regex pattern given)
        /// those classes also need to have at least one mapping (regardless of type) within the latest gameversion.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="packagePattern">The regex pattern to match packages to.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The classes who's packages match the pattern, and are part of the latest gameversion.</returns>
        [HttpGet("package/version/{packagePattern}/latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<ClassReadModel>>> GetByPackageInLatestGameVersion(string packagePattern, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await ClassComponentWriter.GetByPackageInLatestGameVersion(packagePattern);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Gets the classes which are part of a given package (or who's packages match the regex pattern given)
        /// those classes also need to have at least one mapping (regardless of type) within a gameversion that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="packagePattern">The regex pattern to match packages to.</param>
        /// <param name="gameVersionId">The id of the gameversion that the component needs to be in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The classes who's packages match the pattern, and are part of the given gameversion.</returns>
        [HttpGet("package/version/{packagePattern}/{gameVersionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<ClassReadModel>>> GetByPackageInGameVersion(string packagePattern, Guid gameVersionId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await ClassComponentWriter.GetByPackageInGameVersion(packagePattern, gameVersionId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Gets the classes which are part of a given package (or who's packages match the regex pattern given)
        /// those classes also need to have at least one mapping (regardless of type) within the latest release.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="packagePattern">The regex pattern to match packages to.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The classes who's packages match the pattern, and are part of the latest release.</returns>
        [HttpGet("package/release/{packagePattern}/latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<ClassReadModel>>> GetByPackageInLatestRelease(string packagePattern, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await ClassComponentWriter.GetByPackageInLatestRelease(packagePattern);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Gets the classes which are part of a given package (or who's packages match the regex pattern given)
        /// those classes also need to have at least one mapping (regardless of type) within a release that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="packagePattern">The regex pattern to match packages to.</param>
        /// <param name="releaseId">The id of the release that the component needs to be in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The classes who's packages match the pattern, and are part of the given release.</returns>
        [HttpGet("package/release/{packagePattern}/{releaseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<ClassReadModel>>> GetByPackageInRelease(string packagePattern, Guid releaseId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await ClassComponentWriter.GetByPackageInRelease(packagePattern, releaseId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Creates a new class and its central mapping entry.
        /// Creates a new core mapping class, a versioned mapping class for the latest version, as well a single committed mapping.
        /// </summary>
        /// <param name="mapping">The mapping to create.</param>
        /// <returns>An http response code:201-New mapping created,400-The request was invalid,404-Certain data could not be found,401-Unauthorized.</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> Create([FromBody] CreateClassModel mapping)
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

        protected override ClassReadModel ConvertDbModelToReadModel(Component component)
        {
            return new ClassReadModel
            {
                Id = component.Id,
                Versioned = component.VersionedComponents.ToList().Select(ConvertVersionedDbModelToReadModel)
            };
        }

        private ClassVersionedReadModel ConvertVersionedDbModelToReadModel(VersionedComponent versionedComponent)
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
                CurrentMappings = versionedComponent.Mappings.ToList().Select(ConverterUtils.ConvertLiveDbModelToMappingReadModel),
                Proposals = versionedComponent.Proposals.ToList().Select(ConverterUtils.ConvertProposalDbModelToProposalReadModel),
                LockedMappingNames = versionedComponent.LockedMappingTypes.ToList().Select(lm => lm.MappingType.Name),
                Methods = (versionedComponent.Metadata as ClassMetadata)?.Methods.Select(m => m.VersionedComponent.Id),
                Fields = (versionedComponent.Metadata as ClassMetadata)?.Fields.Select(f => f.VersionedComponent.Id)
            };
        }
    }
}
