using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.Core.Readers.Core;
using Data.Core.Readers.Mapping;
using Data.Core.Writers.Mapping;
using Data.WebApi.Model.Creation.Method;
using Data.WebApi.Model.Read.Method;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.WebApi.Controllers
{
    /// <summary>
    /// Controller that handles interactions on method levels.
    /// </summary>
    [Route("/methods")]
    [ApiController]
    public class MethodsController : ComponentControllerBase<MethodReadModel, MethodVersionedReadModel>
    {
        public MethodsController(IMethodComponentWriter methodComponentWriter, IClassComponentReader classComponentReader, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(methodComponentWriter, releaseReader, gameVersionReader, userResolvingService, mappingTypeReader)
        {
            this.MethodComponentWriter = methodComponentWriter;
            this.ClassComponentReader = classComponentReader;
        }

        private IMethodComponentWriter MethodComponentWriter { get; }

        private IClassComponentReader ClassComponentReader { get; }

        /// <summary>
        /// Gets the methods which are part of a given class.
        /// those methods also need to have at least one mapping (regardless of type) within the latest gameversion.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="classId">The id of the class the method needs to be part of.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The methods who's class match the id, and are part of the latest gameversion.</returns>
        [HttpGet("class/version/{classId}/latest/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<MethodReadModel>>> GetByClassInLatestGameVersion(Guid classId, int pageSize, int pageIndex)
        {
            var dbModels = await MethodComponentWriter.GetByClassInLatestGameVersion(classId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Counts the methods which are part of a given class
        /// those methods also need to have at least one mapping (regardless of type) within the latest gameversion.
        /// </summary>
        /// <param name="classId">The id of the class the method needs to be part of.</param>
        /// <returns>The count of methods who's class match the id, and are part of the latest gameversion.</returns>
        [HttpGet("class/version/{classId}/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountClassInLatestGameVersion(Guid classId)
        {
            var dbModels = await MethodComponentWriter.GetByClassInLatestGameVersion(classId);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the methods which are part of a given class.
        /// those methods also need to have at least one mapping (regardless of type) within a gameversion that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="classId">The id of the class which the method has to be part of.</param>
        /// <param name="gameVersionId">The id of the gameversion that the component needs to be in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The methods who's class match the id, and are part of the given gameversion.</returns>
        [HttpGet("class/version/{classId}/{gameVersionId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<MethodReadModel>>> GetByClassInGameVersion(Guid classId, Guid gameVersionId, int pageSize, int pageIndex)
        {
            var dbModels = await MethodComponentWriter.GetByClassInGameVersion(classId, gameVersionId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Counts the methods which are part of a given class.
        /// those methods also need to have at least one mapping (regardless of type) within a gameversion that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="classId">The id of the class the method has to be part of.</param>
        /// <param name="gameVersionId">The id of the gameversion that the component needs to be in.</param>
        /// <returns>The count of methods who's class match the id, and are part of the given gameversion.</returns>
        [HttpGet("class/version/{classId}/{gameVersionId}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountClassInGameVersion(Guid classId, Guid gameVersionId)
        {
            var dbModels = await MethodComponentWriter.GetByClassInGameVersion(classId, gameVersionId);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the methods which are part of a given class
        /// those methods also need to have at least one mapping (regardless of type) within the latest release.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="classId">The id of the class the method has to be part of.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The methods who's class match the id, and are part of the latest release.</returns>
        [HttpGet("class/release/{classId}/latest/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<MethodReadModel>>> GetByClassInLatestRelease(Guid classId, int pageSize, int pageIndex)
        {
            var dbModels = await MethodComponentWriter.GetByClassInLatestRelease(classId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Counts the methods which are part of a given class
        /// those methods also need to have at least one mapping (regardless of type) within the latest release.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="classId">The id of the class the method has to be part of.</param>
        /// <returns>The count of  methods who's class match the id, and are part of the latest release.</returns>
        [HttpGet("class/release/{classId}/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountClassInLatestRelease(Guid classId)
        {
            var dbModels = await MethodComponentWriter.GetByClassInLatestRelease(classId);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the methods which are part of a given class
        /// those methods also need to have at least one mapping (regardless of type) within a release that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="classId">The id of the class the method has to be part of.</param>
        /// <param name="releaseId">The id of the release that the component needs to be in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The methods who's class match the id, and are part of the given release.</returns>
        [HttpGet("class/release/{classId}/{releaseId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<MethodReadModel>>> GetByClassInRelease(Guid classId, Guid releaseId, int pageSize, int pageIndex)
        {
            var dbModels = await MethodComponentWriter.GetByClassInRelease(classId, releaseId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Counts the methods which are part of a given class
        /// those methods also need to have at least one mapping (regardless of type) within a release that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="classId">The id of the class the method has to be part of.</param>
        /// <param name="releaseId">The id of the release that the component needs to be in.</param>
        /// <returns>The count of methods who's class match the id, and are part of the given release.</returns>
        [HttpGet("class/release/{classId}/{releaseId}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByClassInRelease(Guid classId, Guid releaseId)
        {
            var dbModels = await MethodComponentWriter.GetByClassInRelease(classId, releaseId);

            return Content((await dbModels.CountAsync()).ToString());
        }



        /// <summary>
        /// Creates a new method and its central mapping entry.
        /// Creates a new core mapping method, a versioned mapping method for the latest version, as well a single committed mapping.
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
        public async Task<ActionResult> AddToLatest([FromBody] CreateMethodModel mapping)
        {
            var currentLatestGameVersion = await GameVersionReader.GetLatest();
            if (currentLatestGameVersion == null)
                return BadRequest("No game version has been registered yet.");

            var user = await UserResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent memberOf = await ClassComponentReader.GetVersionedComponent(mapping.MemberOf);
            if (memberOf == null)
                return BadRequest("Unknown memberOf class.");

            var versionedMethodMapping = new VersionedComponent
            {
                CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                GameVersion = currentLatestGameVersion,
                Mappings = new List<LiveMappingEntry>(),
                Proposals = new List<ProposalMappingEntry>()
            };

            versionedMethodMapping.Metadata = new MethodMetadata
            {
                VersionedComponent = versionedMethodMapping,
                VersionedComponentForeignKey = versionedMethodMapping.Id,
                MemberOf = memberOf.Metadata as ClassMetadata,
                Parameters = new List<ParameterMetadata>(),
                Descriptor = mapping.Descriptor,
                IsStatic = mapping.IsStatic
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
                    VersionedComponent = versionedMethodMapping,
                    CreatedOn = DateTime.Now
                });

            versionedMethodMapping.Mappings.AddRange(initialLiveMappings);

            var methodMapping = new Component
            {
                Id = Guid.NewGuid(),
                Type = ComponentType.METHOD,
                VersionedComponents = new List<VersionedComponent>() {versionedMethodMapping}
            };

            await ComponentWriter.Add(methodMapping);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", methodMapping.Id, methodMapping);
        }

        /// <summary>
        /// Creates a new versioned method entry for an already existing method mapping.
        /// Creates a versioned mapping method for the given version, as well a single committed mapping.
        /// </summary>
        /// <param name="mapping">The versioned mapping to create.</param>
        /// <returns>An http response code:201-New mapping created,400-The request was invalid,404-Certain data could not be found,401-Unauthorized,409-A versioned method for that version already exists with the method.</returns>
        [HttpPost("add/version/{versionId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> AddToVersion([FromBody] CreateVersionedMethodModel mapping)
        {
            var currentGameVersion = await GameVersionReader.GetById(mapping.GameVersion);
            if (currentGameVersion == null)
                return BadRequest("No game version with that id has been registered yet.");

            var user = await UserResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent memberOf = await ClassComponentReader.GetVersionedComponent(mapping.MemberOf);
            if (memberOf == null)
                return BadRequest("Unknown memberOf class.");

            var methodMapping = await ComponentWriter.GetById(mapping.VersionedMappingFor);
            if (methodMapping == null)
                return BadRequest("Unknown method mapping to create the versioned mapping for.");

            if (methodMapping.VersionedComponents.Any(versionedMapping =>
                versionedMapping.GameVersion.Id == mapping.GameVersion))
                return Conflict();

            var versionedMethodMapping = new VersionedComponent
            {
                CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                GameVersion = currentGameVersion,
                Mappings = new List<LiveMappingEntry>(),
                Proposals = new List<ProposalMappingEntry>()
            };

            versionedMethodMapping.Metadata = new MethodMetadata
            {
                VersionedComponent = versionedMethodMapping,
                VersionedComponentForeignKey = versionedMethodMapping.Id,
                MemberOf = memberOf.Metadata as ClassMetadata,
                Parameters = new List<ParameterMetadata>(),
                Descriptor = mapping.Descriptor,
                IsStatic = mapping.IsStatic
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
                    VersionedComponent = versionedMethodMapping,
                    CreatedOn = DateTime.Now
                });

            versionedMethodMapping.Mappings.AddRange(initialLiveMappings);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", methodMapping.Id, methodMapping);
        }

        protected override MethodReadModel ConvertDbModelToReadModel(Component component)
        {
            return new MethodReadModel
            {
                Id = component.Id,
                Versioned = component.VersionedComponents.ToList().Select(ConvertVersionedDbModelToReadModel)
            };
        }

        protected override MethodVersionedReadModel ConvertVersionedDbModelToReadModel(VersionedComponent versionedComponent)
        {
            var methodMetaData = versionedComponent.Metadata as MethodMetadata;

            if (methodMetaData == null)
                throw new ArgumentException("The given versioned component does not contain a valid metadata", nameof(versionedComponent));

            return new MethodVersionedReadModel
            {
                Id = versionedComponent.Id,
                VersionedViewModelFor = versionedComponent.Component.Id,
                GameVersion = versionedComponent.GameVersion.Id,
                CurrentMappings = versionedComponent.Mappings.ToList().Select(ConvertLiveDbModelToMappingReadModel),
                Proposals = versionedComponent.Proposals.ToList().Select(ConvertProposalDbModelToProposalReadModel),
                MemberOf = methodMetaData.MemberOf.VersionedComponent.Id,
                Descriptor = methodMetaData.Descriptor,
                IsStatic = methodMetaData.IsStatic,
                LockedMappingNames = versionedComponent.LockedMappingTypes.ToList().Select(lm => lm.MappingType.Name)
            };
        }
    }
}
