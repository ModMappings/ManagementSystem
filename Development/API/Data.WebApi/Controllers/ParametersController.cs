using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.Core.Readers.Core;
using Data.Core.Readers.Mapping;
using Data.Core.Writers.Mapping;
using Data.WebApi.Model.Creation.Parameter;
using Data.WebApi.Model.Read.Parameter;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.WebApi.Controllers
{
    /// <summary>
    /// Controller that handles interactions on parameter levels.
    /// </summary>
    [Route("/parameters")]
    [ApiController]
    public class ParametersController : ComponentControllerBase<ParameterReadModel, ParameterVersionedReadModel>
    {
        public ParametersController(IParameterComponentWriter parameterComponentWriter, IMethodComponentReader methodComponentReader, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(parameterComponentWriter, releaseReader, gameVersionReader, userResolvingService, mappingTypeReader)
        {
            ParameterComponentWriter = parameterComponentWriter;
            MethodComponentReader = methodComponentReader;
        }

        private IParameterComponentWriter ParameterComponentWriter { get; }

        private IMethodComponentReader MethodComponentReader { get; }

        /// <summary>
        /// Gets the parameters which are part of a given method.
        /// those parameters also need to have at least one mapping (regardless of type) within the latest gameversion.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="methodId">The id of the method the parameter needs to be part of.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The parameters who's method match the id, and are part of the latest gameversion.</returns>
        [HttpGet("method/version/{methodId}/latest/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ParameterReadModel>>> GetByMethodInLatestGameVersion(Guid methodId, int pageSize, int pageIndex)
        {
            var dbModels = await ParameterComponentWriter.GetByMethodInLatestGameVersion(methodId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Counts the parameters which are part of a given method
        /// those parameters also need to have at least one mapping (regardless of type) within the latest gameversion.
        /// </summary>
        /// <param name="methodId">The id of the method the parameter needs to be part of.</param>
        /// <returns>The count of parameters who's method match the id, and are part of the latest gameversion.</returns>
        [HttpGet("method/version/{methodId}/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountMethodInLatestGameVersion(Guid methodId)
        {
            var dbModels = await ParameterComponentWriter.GetByMethodInLatestGameVersion(methodId);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the parameters which are part of a given method.
        /// those parameters also need to have at least one mapping (regardless of type) within a gameversion that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="methodId">The id of the method which the parameter has to be part of.</param>
        /// <param name="gameVersionId">The id of the gameversion that the component needs to be in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The parameters who's method match the id, and are part of the given gameversion.</returns>
        [HttpGet("method/version/{methodId}/{gameVersionId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ParameterReadModel>>> GetByMethodInGameVersion(Guid methodId, Guid gameVersionId, int pageSize, int pageIndex)
        {
            var dbModels = await ParameterComponentWriter.GetByMethodInGameVersion(methodId, gameVersionId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Counts the parameters which are part of a given method.
        /// those parameters also need to have at least one mapping (regardless of type) within a gameversion that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="methodId">The id of the method the parameter has to be part of.</param>
        /// <param name="gameVersionId">The id of the gameversion that the component needs to be in.</param>
        /// <returns>The count of parameters who's method match the id, and are part of the given gameversion.</returns>
        [HttpGet("method/version/{methodId}/{gameVersionId}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountMethodInGameVersion(Guid methodId, Guid gameVersionId)
        {
            var dbModels = await ParameterComponentWriter.GetByMethodInGameVersion(methodId, gameVersionId);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the parameters which are part of a given method
        /// those parameters also need to have at least one mapping (regardless of type) within the latest release.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="methodId">The id of the method the parameter has to be part of.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The parameters who's method match the id, and are part of the latest release.</returns>
        [HttpGet("method/release/{methodId}/latest/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ParameterReadModel>>> GetByMethodInLatestRelease(Guid methodId, int pageSize, int pageIndex)
        {
            var dbModels = await ParameterComponentWriter.GetByMethodInLatestRelease(methodId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Counts the parameters which are part of a given method
        /// those parameters also need to have at least one mapping (regardless of type) within the latest release.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="methodId">The id of the method the parameter has to be part of.</param>
        /// <returns>The count of  parameters who's method match the id, and are part of the latest release.</returns>
        [HttpGet("method/release/{methodId}/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountMethodInLatestRelease(Guid methodId)
        {
            var dbModels = await ParameterComponentWriter.GetByMethodInLatestRelease(methodId);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the parameters which are part of a given method
        /// those parameters also need to have at least one mapping (regardless of type) within a release that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="methodId">The id of the method the parameter has to be part of.</param>
        /// <param name="releaseId">The id of the release that the component needs to be in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The parameters who's method match the id, and are part of the given release.</returns>
        [HttpGet("method/release/{methodId}/{releaseId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ParameterReadModel>>> GetByMethodInRelease(Guid methodId, Guid releaseId, int pageSize, int pageIndex)
        {
            var dbModels = await ParameterComponentWriter.GetByMethodInRelease(methodId, releaseId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).AsEnumerable().Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Counts the parameters which are part of a given method
        /// those parameters also need to have at least one mapping (regardless of type) within a release that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="methodId">The id of the method the parameter has to be part of.</param>
        /// <param name="releaseId">The id of the release that the component needs to be in.</param>
        /// <returns>The count of parameters who's method match the id, and are part of the given release.</returns>
        [HttpGet("method/release/{methodId}/{releaseId}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByMethodInRelease(Guid methodId, Guid releaseId)
        {
            var dbModels = await ParameterComponentWriter.GetByMethodInRelease(methodId, releaseId);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Creates a new parameter and its central mapping entry.
        /// Creates a new core mapping parameter, a versioned mapping parameter for the latest version, as well a single committed mapping.
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
        public async Task<ActionResult> AddToLatest([FromBody] CreateParameterModel mapping)
        {
            var currentLatestGameVersion = await GameVersionReader.GetLatest();
            if (currentLatestGameVersion == null)
                return BadRequest("No game version has been registered yet.");

            var user = await UserResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            var memberOf = await MethodComponentReader.GetVersionedComponent(mapping.ParameterOf);
            if (memberOf == null)
                return BadRequest("Unknown memberOf method.");

            var versionedParameterMapping = new VersionedComponent
            {
                CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                GameVersion = currentLatestGameVersion,
                Mappings = new List<LiveMappingEntry>(),
                Proposals = new List<ProposalMappingEntry>()
            };

            versionedParameterMapping.Metadata = new ParameterMetadata
            {
                VersionedComponent = versionedParameterMapping,
                VersionedComponentForeignKey = versionedParameterMapping.Id,
                ParameterOf = memberOf.Metadata as MethodMetadata,
                Index = mapping.Index
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
                    VersionedComponent = versionedParameterMapping,
                    CreatedOn = DateTime.Now
                });

            versionedParameterMapping.Mappings.AddRange(initialLiveMappings);

            var parameterMapping = new Component
            {
                Id = Guid.NewGuid(),
                Type = ComponentType.PARAMETER,
                VersionedComponents = new List<VersionedComponent>() {versionedParameterMapping}
            };

            await ComponentWriter.Add(parameterMapping);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", parameterMapping.Id, parameterMapping);
        }

        /// <summary>
        /// Creates a new versioned parameter entry for an already existing parameter mapping.
        /// Creates a versioned mapping parameter for the given version, as well a single committed mapping.
        /// </summary>
        /// <param name="mapping">The versioned mapping to create.</param>
        /// <returns>An http response code:201-New mapping created,400-The request was invalid,404-Certain data could not be found,401-Unauthorized,409-A versioned parameter for that version already exists with the parameter.</returns>
        [HttpPost("add/version/{versionId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> AddToVersion([FromBody] CreateVersionedParameterModel mapping)
        {
            var currentGameVersion = await GameVersionReader.GetById(mapping.GameVersion);
            if (currentGameVersion == null)
                return BadRequest("No game version with that id has been registered yet.");

            var user = await UserResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent memberOf = await MethodComponentReader.GetVersionedComponent(mapping.ParameterOf);
            if (memberOf == null)
                return BadRequest("Unknown memberOf method.");

            var parameterMapping = await ComponentWriter.GetById(mapping.VersionedMappingFor);
            if (parameterMapping == null)
                return BadRequest("Unknown parameter mapping to create the versioned mapping for.");

            if (parameterMapping.VersionedComponents.Any(versionedMapping =>
                versionedMapping.GameVersion.Id == mapping.GameVersion))
                return Conflict();

            var versionedParameterMapping = new VersionedComponent
            {
                CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                GameVersion = currentGameVersion,
                Mappings = new List<LiveMappingEntry>(),
                Proposals = new List<ProposalMappingEntry>()
            };

            versionedParameterMapping.Metadata = new ParameterMetadata
            {
                VersionedComponent = versionedParameterMapping,
                VersionedComponentForeignKey = versionedParameterMapping.Id,
                ParameterOf = memberOf.Metadata as MethodMetadata,
                Index = mapping.Index
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
                    VersionedComponent = versionedParameterMapping,
                    CreatedOn = DateTime.Now
                });

            versionedParameterMapping.Mappings.AddRange(initialLiveMappings);

            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", parameterMapping.Id, parameterMapping);
        }

        protected override ParameterReadModel ConvertDbModelToReadModel(Component component)
        {
            return new ParameterReadModel
            {
                Id = component.Id,
                Versioned = component.VersionedComponents.ToList().Select(ConvertVersionedDbModelToReadModel)
            };
        }

        protected override ParameterVersionedReadModel ConvertVersionedDbModelToReadModel(VersionedComponent versionedComponent)
        {
            var parameterMetaData = versionedComponent.Metadata as ParameterMetadata;

            if (parameterMetaData == null)
                throw new ArgumentException("The given versioned component does not contain a valid metadata", nameof(versionedComponent));

            return new ParameterVersionedReadModel
            {
                Id = versionedComponent.Id,
                VersionedViewModelFor = versionedComponent.Component.Id,
                GameVersion = versionedComponent.GameVersion.Id,
                CurrentMappings = versionedComponent.Mappings.ToList().Select(ConvertLiveDbModelToMappingReadModel),
                Proposals = versionedComponent.Proposals.ToList().Select(ConvertProposalDbModelToProposalReadModel),
                ParameterOf = parameterMetaData.ParameterOf.VersionedComponent.Id,
                Index = parameterMetaData.Index,
                LockedMappingNames = versionedComponent.LockedMappingTypes.ToList().Select(lm => lm.MappingType.Name)
            };
        }
    }
}
