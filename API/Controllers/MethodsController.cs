using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.Model.Creation.Method;
using API.Model.Read.Method;
using API.Services.Core;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.MetaData;
using Data.Core.Readers.Core;
using Data.EFCore.Writer.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    /// <summary>
    /// Controller that handles interactions on method levels.
    /// </summary>
    [Route("/methods")]
    [ApiController]
    public class MethodsController : ComponentControllerBase<MethodReadModel, MethodVersionedReadModel>
    {

        public MethodsController(ComponentWriterFactory componentWriterFactory, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService) : base(componentWriterFactory.Build(ComponentType.METHOD), releaseReader, gameVersionReader, userResolvingService)
        {
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

            VersionedComponent memberOf = await ComponentWriter.GetVersionedMapping(mapping.MemberOf);
            if (memberOf == null)
                return BadRequest("Unknown memberOf class.");

            var versionedMethodMapping = new VersionedComponent
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                GameVersion = currentLatestGameVersion,
                Mappings = new List<LiveMappingEntry>(),
                Proposals = new List<ProposalMappingEntry>()
            };

            versionedMethodMapping.Metadata = new MethodMetadata
            {
                Component = versionedMethodMapping,
                Outer=memberOf,
                Package = mapping.Package,
                InheritsFrom = inheritsFrom,
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

            VersionedComponent outer = null;
            if (mapping.Outer.HasValue)
            {
                outer = await ComponentWriter.GetVersionedMapping(mapping.Outer.Value);
                if (outer == null)
                    return BadRequest("Unknown outer method");
            }

            var methodMapping = await ComponentWriter.GetById(mapping.VersionedMappingFor);
            if (methodMapping == null)
                return BadRequest("Unknown method mapping to create the versioned mapping for.");

            if (methodMapping.VersionedMappings.Any(versionedMapping =>
                versionedMapping.GameVersion.Id == mapping.GameVersion))
                return Conflict();

            var inheritsFrom =
                (await Task.WhenAll(mapping.InheritsFrom.Select(async id =>
                    await ComponentWriter.GetVersionedMapping(id)))).ToList();

            if (inheritsFrom.Any(m => m == null))
                return BadRequest("Unknown inheriting method.");

            var versionedMethodMapping = new VersionedComponent
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                GameVersion = currentGameVersion,
                Mappings = new List<LiveMappingEntry>(),
                Proposals = new List<ProposalMappingEntry>()
            };

            versionedMethodMapping.Metadata = new MethodMetadata
            {
                Component = versionedMethodMapping,
                Outer=outer,
                Package = mapping.Package,
                InheritsFrom = inheritsFrom,
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
            var outerId = (versionedComponent.Metadata as MethodMetadata)?.Outer?.Id;

            return new MethodVersionedReadModel
            {
                Id = versionedComponent.Id,
                VersionedViewModelFor = versionedComponent.Component.Id,
                GameVersion = versionedComponent.GameVersion.Id,
                Outer = outerId,
                Package = (versionedComponent.Metadata as MethodMetadata)?.Package,
                InheritsFrom = (versionedComponent.Metadata as MethodMetadata)?.InheritsFrom.ToList().Select(parentMethod => parentMethod.Id),
                CurrentMappings = versionedComponent.Mappings.ToList().Select(ConvertLiveDbModelToMappingReadModel),
                Proposals = versionedComponent.Proposals.ToList().Select(ConvertProposalDbModelToProposalReadModel)
            };
        }
    }
}
