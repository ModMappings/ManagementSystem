using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model.Creation.Method;
using API.Model.Creation.Core;
using API.Model.Read.Core;
using API.Model.Read.Method;
using API.Services.Core;
using Data.Core.Models.Method;
using Data.Core.Models.Core;
using Data.Core.Readers.Class;
using Data.Core.Readers.Core;
using Data.Core.Writers.Class;
using Data.Core.Writers.Core;
using Data.Core.Writers.Method;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// Controller that handles interactions on method levels.
    /// </summary>
    [Route("/methods")]
    [ApiController]
    public class MethodsController : NoneUniqueMappingControllerBase<
        MethodMapping,
        MethodVersionedMapping,
        MethodCommittedMappingEntry,
        MethodProposalMappingEntry,
        MethodReleaseMember,
        MethodReadModel,
        MethodVersionedReadModel,
        CreateMethodModel,
        CreateVersionedMethodModel>
    {

        private readonly IClassMappingReader _classMappingReader;

        public MethodsController(INoneUniqueNamedMappingWriter<MethodMapping, MethodVersionedMapping, MethodCommittedMappingEntry, MethodProposalMappingEntry, MethodReleaseMember> readerWriter, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IClassMappingReader classMappingReader) : base(readerWriter, gameVersionReader, userResolvingService)
        {
            _classMappingReader = classMappingReader;
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
        public override async Task<ActionResult> AddToLatest([FromBody] CreateMethodModel mapping)
        {
            var currentLatestGameVersion = await _gameVersionReader.GetLatest();
            if (currentLatestGameVersion == null)
                return BadRequest("No game version has been registered yet.");

            var user = await _userResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            var memberOf = await _classMappingReader.GetVersionedMapping(mapping.MemberOf);
            if (memberOf == null)
                return BadRequest("Unknown versioned class mapping, this method would be part of.");

            var versionedMethodMapping = new MethodVersionedMapping
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                GameVersion = currentLatestGameVersion,
                IsStatic = mapping.IsStatic,
                Descriptor = mapping.Descriptor,
                CommittedMappings = new List<MethodCommittedMappingEntry>(),
                ProposalMappings = new List<MethodProposalMappingEntry>(),
                MemberOf = memberOf
            };

            var initialCommittedMappingEntry = new MethodCommittedMappingEntry
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<MethodReleaseMember>(),
                VersionedMapping = versionedMethodMapping,
                CreatedOn = DateTime.Now
            };

            versionedMethodMapping.CommittedMappings.Add(initialCommittedMappingEntry);

            var methodMapping = new MethodMapping
            {
                Id = Guid.NewGuid(),
                VersionedMappings = new List<MethodVersionedMapping>() {versionedMethodMapping}
            };

            await _readerWriter.Add(methodMapping);
            await _readerWriter.SaveChanges();

            return CreatedAtAction("GetById", methodMapping.Id, methodMapping);
        }

        public override async Task<ActionResult> AddToVersion([FromBody] CreateVersionedMethodModel mapping)
        {
            var currentGameVersion = await _gameVersionReader.GetById(mapping.GameVersion);
            if (currentGameVersion == null)
                return BadRequest("No game version with that id has been registered yet.");

            var user = await _userResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            var memberOf = await _classMappingReader.GetVersionedMapping(mapping.MemberOf);
            if (memberOf == null)
                return BadRequest("Unknown versioned class mapping, this method would be part of.");

            var methodMapping = await _readerWriter.GetById(mapping.VersionedMappingFor);
            if (methodMapping == null)
                return BadRequest("Unknown method mapping to create the versioned mapping for.");

            if (methodMapping.VersionedMappings.Any(versionedMapping =>
                versionedMapping.GameVersion.Id == mapping.GameVersion))
                return Conflict();

            var versionedMethodMapping = new MethodVersionedMapping
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                GameVersion = currentGameVersion,
                IsStatic = mapping.IsStatic,
                Descriptor = mapping.Descriptor,
                CommittedMappings = new List<MethodCommittedMappingEntry>(),
                ProposalMappings = new List<MethodProposalMappingEntry>(),
                MemberOf = memberOf
            };

            var initialCommittedMappingEntry = new MethodCommittedMappingEntry
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<MethodReleaseMember>(),
                VersionedMapping = versionedMethodMapping,
                CreatedOn = DateTime.Now
            };

            versionedMethodMapping.CommittedMappings.Add(initialCommittedMappingEntry);

            await _readerWriter.SaveChanges();

            return CreatedAtAction("GetById", methodMapping.Id, methodMapping);
        }



        protected override MethodReadModel ConvertDBModelToApiModel(MethodMapping methodModel)
        {
            return new MethodReadModel
            {
                Id = methodModel.Id,
                Versioned = methodModel.VersionedMappings.ToList().Select(ConvertVersionedDbModelToVersionedApiModel)
            };
        }

        protected override MethodVersionedReadModel ConvertVersionedDbModelToVersionedApiModel(MethodVersionedMapping versionedMapping)
        {
            return new MethodVersionedReadModel
            {
                Id = versionedMapping.Id,
                VersionedViewModelFor = versionedMapping.Mapping.Id,
                GameVersion = versionedMapping.GameVersion.Id,
                CurrentMappings = versionedMapping.CommittedMappings.ToList().Select(ConvertCommittedMappingToSimpleReadModel),
                Proposals = versionedMapping.ProposalMappings.ToList().Select(ConvertProposalMappingToReadModel),
                MemberOf = versionedMapping.MemberOf.Id,
                Descriptor = versionedMapping.Descriptor,
                IsStatic = versionedMapping.IsStatic
            };
        }

        protected override MappingReadModel ConvertCommittedMappingToSimpleReadModel(MethodCommittedMappingEntry committedMapping)
        {
            return new MappingReadModel {Id = committedMapping.Id, In = committedMapping.InputMapping, Out = committedMapping.OutputMapping, Documentation = committedMapping.Documentation};
        }

        protected override ProposalReadModel ConvertProposalMappingToReadModel(MethodProposalMappingEntry proposalMapping) =>
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
