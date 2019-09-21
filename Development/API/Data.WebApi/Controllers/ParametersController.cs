using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.Core.Readers.Core;
using Data.Core.Writers.Core;
using Data.EFCore.Writer.Mapping;
using Data.WebApi.Model.Creation.Parameter;
using Data.WebApi.Model.Read.Parameter;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers
{
    /// <summary>
    /// Controller that handles interactions on parameter levels.
    /// </summary>
    [Route("/parameters")]
    [ApiController]
    public class ParametersController : ComponentControllerBase<ParameterReadModel, ParameterVersionedReadModel>
    {

        private readonly IComponentWriter _methodWriter;

        public ParametersController(ComponentWriterFactory componentWriterFactory, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService) : base(componentWriterFactory.Build(ComponentType.PARAMETER), releaseReader, gameVersionReader, userResolvingService)
        {
            this._methodWriter = componentWriterFactory.Build(ComponentType.METHOD);
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
        public async Task<ActionResult> AddToLatest([FromBody] CreateParameterModel mapping)
        {
            var currentLatestGameVersion = await GameVersionReader.GetLatest();
            if (currentLatestGameVersion == null)
                return BadRequest("No game version has been registered yet.");

            var user = await UserResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent memberOf = await _methodWriter.GetVersionedMapping(mapping.ParameterOf);
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
                Component = versionedParameterMapping,
                ParameterOf = memberOf.Metadata as MethodMetadata,
                Index = mapping.Index
            };

            var initialCommittedMappingEntry = new LiveMappingEntry()
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<ReleaseComponent>(),
                Mapping = versionedParameterMapping,
                CreatedOn = DateTime.Now
            };

            versionedParameterMapping.Mappings.Add(initialCommittedMappingEntry);

            var parameterMapping = new Component
            {
                Id = Guid.NewGuid(),
                Type = ComponentType.PARAMETER,
                VersionedMappings = new List<VersionedComponent>() {versionedParameterMapping}
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
        public async Task<ActionResult> AddToVersion([FromBody] CreateVersionedParameterModel mapping)
        {
            var currentGameVersion = await GameVersionReader.GetById(mapping.GameVersion);
            if (currentGameVersion == null)
                return BadRequest("No game version with that id has been registered yet.");

            var user = await UserResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent memberOf = await _methodWriter.GetVersionedMapping(mapping.ParameterOf);
            if (memberOf == null)
                return BadRequest("Unknown memberOf method.");

            var parameterMapping = await ComponentWriter.GetById(mapping.VersionedMappingFor);
            if (parameterMapping == null)
                return BadRequest("Unknown parameter mapping to create the versioned mapping for.");

            if (parameterMapping.VersionedMappings.Any(versionedMapping =>
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
                Component = versionedParameterMapping,
                ParameterOf = memberOf.Metadata as MethodMetadata,
                Index = mapping.Index
            };

            var initialCommittedMappingEntry = new LiveMappingEntry()
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<ReleaseComponent>(),
                Mapping = versionedParameterMapping,
                CreatedOn = DateTime.Now
            };

            versionedParameterMapping.Mappings.Add(initialCommittedMappingEntry);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", parameterMapping.Id, parameterMapping);
        }

        protected override ParameterReadModel ConvertDbModelToReadModel(Component component)
        {
            return new ParameterReadModel
            {
                Id = component.Id,
                Versioned = component.VersionedMappings.ToList().Select(ConvertVersionedDbModelToReadModel)
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
                ParameterOf = parameterMetaData.ParameterOf.Component.Id,
                Index = parameterMetaData.Index,
            };
        }
    }
}
