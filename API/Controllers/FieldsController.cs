using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model.Creation.Core;
using API.Model.Creation.Field;
using API.Model.Read.Core;
using API.Model.Read.Field;
using API.Services.Core;
using Data.Core.Models.Field;
using Data.Core.Models.Core;
using Data.Core.Readers.Class;
using Data.Core.Readers.Core;
using Data.Core.Writers.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller that handles interactions on field levels.
    /// </summary>
    [Route("/fields")]
    [ApiController]
    public class FieldsController : NoneUniqueMappingControllerBase<
        FieldMapping,
        FieldVersionedMapping,
        FieldCommittedMappingEntry,
        FieldProposalMappingEntry,
        FieldReleaseMember,
        FieldReadModel,
        FieldVersionedReadModel,
        CreateFieldModel,
        CreateVersionedFieldModel>
    {

        private readonly IClassMappingReader _classMappingReader;

        public FieldsController(INoneUniqueNamedMappingWriter<FieldMapping, FieldVersionedMapping, FieldCommittedMappingEntry, FieldProposalMappingEntry, FieldReleaseMember> readerWriter, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IClassMappingReader classMappingReader) : base(readerWriter, gameVersionReader, userResolvingService)
        {
            _classMappingReader = classMappingReader;
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
        public override async Task<ActionResult> AddToLatest([FromBody] CreateFieldModel mapping)
        {
            var currentLatestGameVersion = await _gameVersionReader.GetLatest();
            if (currentLatestGameVersion == null)
                return BadRequest("No game version has been registered yet.");

            var user = await _userResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            var memberOf = await _classMappingReader.GetVersionedMapping(mapping.MemberOf);
            if (memberOf == null)
                return BadRequest("Unknown versioned class mapping, this field would be part of.");

            var versionedFieldMapping = new FieldVersionedMapping
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                GameVersion = currentLatestGameVersion,
                IsStatic = mapping.IsStatic,
                CommittedMappings = new List<FieldCommittedMappingEntry>(),
                ProposalMappings = new List<FieldProposalMappingEntry>(),
                MemberOf = memberOf
            };

            var initialCommittedMappingEntry = new FieldCommittedMappingEntry
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<FieldReleaseMember>(),
                VersionedMapping = versionedFieldMapping,
                CreatedOn = DateTime.Now
            };

            versionedFieldMapping.CommittedMappings.Add(initialCommittedMappingEntry);

            var fieldMapping = new FieldMapping
            {
                Id = Guid.NewGuid(),
                VersionedMappings = new List<FieldVersionedMapping>() {versionedFieldMapping}
            };

            await _readerWriter.Add(fieldMapping);
            await _readerWriter.SaveChanges();

            return CreatedAtAction("GetById", fieldMapping.Id, fieldMapping);
        }

        /// <summary>
        /// Creates a new versioned model entry for an already existing method mapping.
        /// Creates a versioned mapping model for the given version, as well a single committed mapping.
        /// </summary>
        /// <param name="mapping">The versioned mapping to create.</param>
        /// <returns>An http response code:201-New mapping created,400-The request was invalid,404-Certain data could not be found,401-Unauthorized,409-A versioned model for that version already exists with the core model.</returns>
        [HttpPost("add/version/{versionId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public override async Task<ActionResult> AddToVersion([FromBody] CreateVersionedFieldModel mapping)
        {
            var currentGameVersion = await _gameVersionReader.GetById(mapping.GameVersion);
            if (currentGameVersion == null)
                return BadRequest("No game version with that id has been registered yet.");

            var user = await _userResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            var memberOf = await _classMappingReader.GetVersionedMapping(mapping.MemberOf);
            if (memberOf == null)
                return BadRequest("Unknown versioned class mapping, this field would be part of.");

            var fieldMapping = await _readerWriter.GetById(mapping.VersionedMappingFor);
            if (fieldMapping == null)
                return BadRequest("Unknown field mapping to create the versioned mapping for.");

            if (fieldMapping.VersionedMappings.Any(versionedMapping =>
                versionedMapping.GameVersion.Id == mapping.GameVersion))
                return Conflict();

            var versionedFieldMapping = new FieldVersionedMapping
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                GameVersion = currentGameVersion,
                IsStatic = mapping.IsStatic,
                CommittedMappings = new List<FieldCommittedMappingEntry>(),
                ProposalMappings = new List<FieldProposalMappingEntry>(),
                MemberOf = memberOf
            };

            var initialCommittedMappingEntry = new FieldCommittedMappingEntry
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<FieldReleaseMember>(),
                VersionedMapping = versionedFieldMapping,
                CreatedOn = DateTime.Now
            };

            versionedFieldMapping.CommittedMappings.Add(initialCommittedMappingEntry);

            await _readerWriter.SaveChanges();

            return CreatedAtAction("GetById", fieldMapping.Id, fieldMapping);
        }

        protected override FieldReadModel ConvertDBModelToApiModel(FieldMapping fieldModel)
        {
            return new FieldReadModel
            {
                Id = fieldModel.Id,
                Versioned = fieldModel.VersionedMappings.ToList().Select(ConvertVersionedDbModelToVersionedApiModel)
            };
        }

        protected override FieldVersionedReadModel ConvertVersionedDbModelToVersionedApiModel(FieldVersionedMapping versionedMapping)
        {
            return new FieldVersionedReadModel
            {
                Id = versionedMapping.Id,
                VersionedViewModelFor = versionedMapping.Mapping.Id,
                GameVersion = versionedMapping.GameVersion.Id,
                CurrentMappings = versionedMapping.CommittedMappings.ToList().Select(ConvertCommittedMappingToSimpleReadModel),
                Proposals = versionedMapping.ProposalMappings.ToList().Select(ConvertProposalMappingToReadModel),
                MemberOf = versionedMapping.MemberOf.Id,
                IsStatic = versionedMapping.IsStatic
            };
        }

        protected override MappingReadModel ConvertCommittedMappingToSimpleReadModel(FieldCommittedMappingEntry committedMapping)
        {
            return new MappingReadModel {Id = committedMapping.Id, In = committedMapping.InputMapping, Out = committedMapping.OutputMapping, Documentation = committedMapping.Documentation};
        }

        protected override ProposalReadModel ConvertProposalMappingToReadModel(FieldProposalMappingEntry proposalMapping) =>
            new ProposalReadModel()
            {
                Id = proposalMapping.Id,
                ProposedFor = proposalMapping.VersionedMapping.Id,
                GameVersion = proposalMapping.VersionedMapping.GameVersion.Id,
                ProposedBy = proposalMapping.ProposedBy.Id,
                ProposedOn = proposalMapping.ProposedOn,
                IsOpen = proposalMapping.IsOpen,
                IsPublicVote = proposalMapping.IsPublicVote,
                VotedFor = proposalMapping.VotedFor.ToList().Select(user => user.Id),
                VotedAgainst = proposalMapping.VotedAgainst.ToList().Select(user => user.Id),
                Comment = proposalMapping.Comment,
                ClosedBy = proposalMapping.ClosedBy.Id,
                ClosedOn = proposalMapping.ClosedOn,
                In = proposalMapping.InputMapping,
                Out = proposalMapping.OutputMapping,
                Documentation = proposalMapping.Documentation
            };
    }
}
