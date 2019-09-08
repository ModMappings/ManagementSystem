using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.Model.Creation.Field;
using API.Model.Read.Field;
using API.Services.Core;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.MetaData;
using Data.Core.Readers.Core;
using Data.Core.Writers.Core;
using Data.EFCore.Writer.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    /// <summary>
    /// Controller that handles interactions on field levels.
    /// </summary>
    [Route("/fields")]
    [ApiController]
    public class FieldsController : ComponentControllerBase<FieldReadModel, FieldVersionedReadModel>
    {

        private readonly IComponentWriter _classWriter;

        public FieldsController(ComponentWriterFactory componentWriterFactory, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService) : base(componentWriterFactory.Build(ComponentType.FIELD), releaseReader, gameVersionReader, userResolvingService)
        {
            this._classWriter = componentWriterFactory.Build(ComponentType.CLASS);
        }

        /// <summary>
        /// Creates a new field and its central mapping entry.
        /// Creates a new core mapping field, a versioned mapping field for the latest version, as well a single committed mapping.
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
        public async Task<ActionResult> AddToLatest([FromBody] CreateFieldModel mapping)
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

            var versionedFieldMapping = new VersionedComponent
            {
                CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                GameVersion = currentLatestGameVersion,
                Mappings = new List<LiveMappingEntry>(),
                Proposals = new List<ProposalMappingEntry>()
            };

            versionedFieldMapping.Metadata = new FieldMetadata
            {
                Component = versionedFieldMapping,
                MemberOf = memberOf.Metadata as ClassMetadata,
                IsStatic = mapping.IsStatic
            };

            var initialCommittedMappingEntry = new LiveMappingEntry()
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<ReleaseComponent>(),
                Mapping = versionedFieldMapping,
                CreatedOn = DateTime.Now
            };

            versionedFieldMapping.Mappings.Add(initialCommittedMappingEntry);

            var fieldMapping = new Component
            {
                Id = Guid.NewGuid(),
                Type = ComponentType.FIELD,
                VersionedMappings = new List<VersionedComponent>() {versionedFieldMapping}
            };

            await ComponentWriter.Add(fieldMapping);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", fieldMapping.Id, fieldMapping);
        }

        /// <summary>
        /// Creates a new versioned field entry for an already existing field mapping.
        /// Creates a versioned mapping field for the given version, as well a single committed mapping.
        /// </summary>
        /// <param name="mapping">The versioned mapping to create.</param>
        /// <returns>An http response code:201-New mapping created,400-The request was invalid,404-Certain data could not be found,401-Unauthorized,409-A versioned field for that version already exists with the field.</returns>
        [HttpPost("add/version/{versionId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> AddToVersion([FromBody] CreateVersionedFieldModel mapping)
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

            var fieldMapping = await ComponentWriter.GetById(mapping.VersionedMappingFor);
            if (fieldMapping == null)
                return BadRequest("Unknown field mapping to create the versioned mapping for.");

            if (fieldMapping.VersionedMappings.Any(versionedMapping =>
                versionedMapping.GameVersion.Id == mapping.GameVersion))
                return Conflict();

            var versionedFieldMapping = new VersionedComponent
            {
                CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                GameVersion = currentGameVersion,
                Mappings = new List<LiveMappingEntry>(),
                Proposals = new List<ProposalMappingEntry>()
            };

            versionedFieldMapping.Metadata = new FieldMetadata
            {
                Component = versionedFieldMapping,
                MemberOf = memberOf.Metadata as ClassMetadata,
                IsStatic = mapping.IsStatic
            };

            var initialCommittedMappingEntry = new LiveMappingEntry()
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<ReleaseComponent>(),
                Mapping = versionedFieldMapping,
                CreatedOn = DateTime.Now
            };

            versionedFieldMapping.Mappings.Add(initialCommittedMappingEntry);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", fieldMapping.Id, fieldMapping);
        }

        protected override FieldReadModel ConvertDbModelToReadModel(Component component)
        {
            return new FieldReadModel
            {
                Id = component.Id,
                Versioned = component.VersionedMappings.ToList().Select(ConvertVersionedDbModelToReadModel)
            };
        }

        protected override FieldVersionedReadModel ConvertVersionedDbModelToReadModel(VersionedComponent versionedComponent)
        {
            var fieldMetaData = versionedComponent.Metadata as FieldMetadata;

            if (fieldMetaData == null)
                throw new ArgumentException("The given versioned component does not contain a valid metadata", nameof(versionedComponent));

            return new FieldVersionedReadModel
            {
                Id = versionedComponent.Id,
                VersionedViewModelFor = versionedComponent.Component.Id,
                GameVersion = versionedComponent.GameVersion.Id,
                CurrentMappings = versionedComponent.Mappings.ToList().Select(ConvertLiveDbModelToMappingReadModel),
                Proposals = versionedComponent.Proposals.ToList().Select(ConvertProposalDbModelToProposalReadModel),
                MemberOf = fieldMetaData.MemberOf.Component.Id,
                IsStatic = fieldMetaData.IsStatic,
            };
        }
    }
}
