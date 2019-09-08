using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.Model.Creation.Class;
using API.Model.Read.Class;
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
    /// Controller that handles interactions on class levels.
    /// </summary>
    [Route("/classes")]
    [ApiController]
    public class ClassesController : ComponentControllerBase<ClassReadModel, ClassVersionedReadModel>
    {

        public ClassesController(ComponentWriterFactory componentWriterFactory, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService) : base(componentWriterFactory.Build(ComponentType.CLASS), releaseReader, gameVersionReader, userResolvingService)
        {
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
                outer = await ComponentWriter.GetVersionedMapping(mapping.Outer.Value);
                if (outer == null)
                    return BadRequest("Unknown outer class");
            }

            var inheritsFrom =
                (await Task.WhenAll(
                    mapping.InheritsFrom.Select(async id => (await ComponentWriter.GetVersionedMapping(id)).Metadata as ClassMetadata))).ToList();

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
                Component = versionedClassMapping,
                Outer=outer?.Metadata as ClassMetadata,
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
                Mapping = versionedClassMapping,
                CreatedOn = DateTime.Now
            };

            versionedClassMapping.Mappings.Add(initialCommittedMappingEntry);

            var classMapping = new Component
            {
                Id = Guid.NewGuid(),
                Type = ComponentType.CLASS,
                VersionedMappings = new List<VersionedComponent>() {versionedClassMapping}
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
                outer = await ComponentWriter.GetVersionedMapping(mapping.Outer.Value);
                if (outer == null)
                    return BadRequest("Unknown outer class");
            }

            var inheritsFrom =
                (await Task.WhenAll(
                    mapping.InheritsFrom.Select(async id => (await ComponentWriter.GetVersionedMapping(id)).Metadata as ClassMetadata))).ToList();

            if (inheritsFrom.Any(m => m == null))
                return BadRequest("Unknown inheriting class.");

            var classMapping = await ComponentWriter.GetById(mapping.VersionedMappingFor);
            if (classMapping == null)
                return BadRequest("Unknown class mapping to create the versioned mapping for.");

            if (classMapping.VersionedMappings.Any(versionedMapping =>
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
                Component = versionedClassMapping,
                Outer=outer?.Metadata as ClassMetadata,
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
                Mapping = versionedClassMapping,
                CreatedOn = DateTime.Now
            };

            versionedClassMapping.Mappings.Add(initialCommittedMappingEntry);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", classMapping.Id, classMapping);
        }

        protected override ClassReadModel ConvertDbModelToReadModel(Component component)
        {
            return new ClassReadModel
            {
                Id = component.Id,
                Versioned = component.VersionedMappings.ToList().Select(ConvertVersionedDbModelToReadModel)
            };
        }

        protected override ClassVersionedReadModel ConvertVersionedDbModelToReadModel(VersionedComponent versionedComponent)
        {
            var outerId = (versionedComponent.Metadata as ClassMetadata)?.Outer?.Component.Id;

            return new ClassVersionedReadModel
            {
                Id = versionedComponent.Id,
                VersionedViewModelFor = versionedComponent.Component.Id,
                GameVersion = versionedComponent.GameVersion.Id,
                Outer = outerId,
                Package = (versionedComponent.Metadata as ClassMetadata)?.Package,
                InheritsFrom = (versionedComponent.Metadata as ClassMetadata)?.InheritsFrom.ToList().Select(parentClass => parentClass.Component.Id),
                CurrentMappings = versionedComponent.Mappings.ToList().Select(ConvertLiveDbModelToMappingReadModel),
                Proposals = versionedComponent.Proposals.ToList().Select(ConvertProposalDbModelToProposalReadModel)
            };
        }
    }
}
