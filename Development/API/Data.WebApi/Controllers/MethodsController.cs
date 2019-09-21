using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.Core.Readers.Core;
using Data.Core.Writers.Core;
using Data.EFCore.Writer.Mapping;
using Data.WebApi.Model.Creation.Method;
using Data.WebApi.Model.Read.Method;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers
{
    /// <summary>
    /// Controller that handles interactions on method levels.
    /// </summary>
    [Route("/methods")]
    [ApiController]
    public class MethodsController : ComponentControllerBase<MethodReadModel, MethodVersionedReadModel>
    {

        private readonly IComponentWriter _classWriter;

        public MethodsController(ComponentWriterFactory componentWriterFactory, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService) : base(componentWriterFactory.Build(ComponentType.METHOD), releaseReader, gameVersionReader, userResolvingService)
        {
            this._classWriter = componentWriterFactory.Build(ComponentType.CLASS);
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
        public async Task<ActionResult> AddToLatest([FromBody] CreateMethodModel mapping)
        {
            var currentLatestGameVersion = await GameVersionReader.GetLatest();
            if (currentLatestGameVersion == null)
                return BadRequest("No game version has been registered yet.");

            var user = await UserResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent memberOf = await _classWriter.GetVersionedMapping(mapping.MemberOf);
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
                Component = versionedMethodMapping,
                MemberOf = memberOf.Metadata as ClassMetadata,
                Parameters = new List<ParameterMetadata>(),
                Descriptor = mapping.Descriptor,
                IsStatic = mapping.IsStatic
            };

            var initialCommittedMappingEntry = new LiveMappingEntry()
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<ReleaseComponent>(),
                Mapping = versionedMethodMapping,
                CreatedOn = DateTime.Now
            };

            versionedMethodMapping.Mappings.Add(initialCommittedMappingEntry);

            var methodMapping = new Component
            {
                Id = Guid.NewGuid(),
                Type = ComponentType.METHOD,
                VersionedMappings = new List<VersionedComponent>() {versionedMethodMapping}
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
        public async Task<ActionResult> AddToVersion([FromBody] CreateVersionedMethodModel mapping)
        {
            var currentGameVersion = await GameVersionReader.GetById(mapping.GameVersion);
            if (currentGameVersion == null)
                return BadRequest("No game version with that id has been registered yet.");

            var user = await UserResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent memberOf = await _classWriter.GetVersionedMapping(mapping.MemberOf);
            if (memberOf == null)
                return BadRequest("Unknown memberOf class.");

            var methodMapping = await ComponentWriter.GetById(mapping.VersionedMappingFor);
            if (methodMapping == null)
                return BadRequest("Unknown method mapping to create the versioned mapping for.");

            if (methodMapping.VersionedMappings.Any(versionedMapping =>
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
                Component = versionedMethodMapping,
                MemberOf = memberOf.Metadata as ClassMetadata,
                Parameters = new List<ParameterMetadata>(),
                Descriptor = mapping.Descriptor,
                IsStatic = mapping.IsStatic
            };

            var initialCommittedMappingEntry = new LiveMappingEntry()
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<ReleaseComponent>(),
                Mapping = versionedMethodMapping,
                CreatedOn = DateTime.Now
            };

            versionedMethodMapping.Mappings.Add(initialCommittedMappingEntry);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", methodMapping.Id, methodMapping);
        }

        protected override MethodReadModel ConvertDbModelToReadModel(Component component)
        {
            return new MethodReadModel
            {
                Id = component.Id,
                Versioned = component.VersionedMappings.ToList().Select(ConvertVersionedDbModelToReadModel)
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
                MemberOf = methodMetaData.MemberOf.Component.Id,
                Descriptor = methodMetaData.Descriptor,
                IsStatic = methodMetaData.IsStatic
            };
        }
    }
}
