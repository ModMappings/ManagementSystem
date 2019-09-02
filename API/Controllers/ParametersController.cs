using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model.Creation.Parameter;
using API.Model.Read.Core;
using API.Model.Read.Parameter;
using API.Services.Core;
using Data.Core.Models.Parameter;
using Data.Core.Models.Core;
using Data.Core.Readers.Class;
using Data.Core.Readers.Core;
using Data.Core.Readers.Method;
using Data.Core.Writers.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller that handles interactions on parameter levels.
    /// </summary>
    [Route("/parameters")]
    [ApiController]
    public class ParametersController : NoneUniqueMappingControllerBase<
        ParameterMapping,
        ParameterVersionedMapping,
        ParameterCommittedMappingEntry,
        ParameterProposalMappingEntry,
        ParameterReleaseMember,
        ParameterReadModel,
        ParameterVersionedReadModel,
        CreateParameterModel,
        CreateVersionedParameterModel>
    {

        private readonly IMethodMappingReader _methodMappingReader;

        public ParametersController(INoneUniqueNamedMappingWriter<ParameterMapping, ParameterVersionedMapping, ParameterCommittedMappingEntry, ParameterProposalMappingEntry, ParameterReleaseMember> readerWriter, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IMethodMappingReader methodMappingReader) : base(readerWriter, gameVersionReader, userResolvingService)
        {
            _methodMappingReader = methodMappingReader;
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
        public override async Task<ActionResult> AddToLatest([FromBody] CreateParameterModel mapping)
        {
            var currentLatestGameVersion = await _gameVersionReader.GetLatest();
            if (currentLatestGameVersion == null)
                return BadRequest("No game version has been registered yet.");

            var user = await _userResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            var parameterOf = await _methodMappingReader.GetVersionedMapping(mapping.ParameterOf);
            if (parameterOf == null)
                return BadRequest("Unknown versioned method mapping, this parameter would be part of.");

            if (parameterOf.Parameters.Any(parameter => parameter.Index == mapping.Index))
                return BadRequest("There is already a parameter at the given index.");

            var versionedParameterMapping = new ParameterVersionedMapping
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                GameVersion = currentLatestGameVersion,
                CommittedMappings = new List<ParameterCommittedMappingEntry>(),
                ProposalMappings = new List<ParameterProposalMappingEntry>(),
                ParameterOf = parameterOf,
                Index = mapping.Index
            };

            var initialCommittedMappingEntry = new ParameterCommittedMappingEntry
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<ParameterReleaseMember>(),
                VersionedMapping = versionedParameterMapping,
                CreatedOn = DateTime.Now
            };

            versionedParameterMapping.CommittedMappings.Add(initialCommittedMappingEntry);

            var parameterMapping = new ParameterMapping
            {
                Id = Guid.NewGuid(),
                VersionedMappings = new List<ParameterVersionedMapping>() {versionedParameterMapping}
            };

            await _readerWriter.Add(parameterMapping);
            await _readerWriter.SaveChanges();

            return CreatedAtAction("GetById", parameterMapping.Id, parameterMapping);
        }

        public override async Task<ActionResult> AddToVersion([FromBody] CreateVersionedParameterModel mapping)
        {
            var currentGameVersion = await _gameVersionReader.GetById(mapping.GameVersion);
            if (currentGameVersion == null)
                return BadRequest("No game version with that id has been registered yet.");

            var user = await _userResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            var parameterOf = await _methodMappingReader.GetVersionedMapping(mapping.ParameterOf);
            if (parameterOf == null)
                return BadRequest("Unknown versioned method mapping, this parameter would be part of.");

            var parameterMapping = await _readerWriter.GetById(mapping.VersionedMappingFor);
            if (parameterMapping == null)
                return BadRequest("Unknown parameter mapping to create the versioned mapping for.");

            if (parameterMapping.VersionedMappings.Any(versionedMapping =>
                versionedMapping.GameVersion.Id == mapping.GameVersion))
                return Conflict();

            var versionedParameterMapping = new ParameterVersionedMapping
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                GameVersion = currentGameVersion,
                CommittedMappings = new List<ParameterCommittedMappingEntry>(),
                ProposalMappings = new List<ParameterProposalMappingEntry>(),
                ParameterOf = parameterOf,
                Index = mapping.Index
            };

            var initialCommittedMappingEntry = new ParameterCommittedMappingEntry
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<ParameterReleaseMember>(),
                VersionedMapping = versionedParameterMapping,
                CreatedOn = DateTime.Now
            };

            versionedParameterMapping.CommittedMappings.Add(initialCommittedMappingEntry);

            await _readerWriter.SaveChanges();

            return CreatedAtAction("GetById", parameterMapping.Id, parameterMapping);
        }



        protected override ParameterReadModel ConvertDBModelToApiModel(ParameterMapping parameterModel)
        {
            return new ParameterReadModel
            {
                Id = parameterModel.Id,
                Versioned = parameterModel.VersionedMappings.ToList().Select(ConvertVersionedDbModelToVersionedApiModel)
            };
        }

        protected override ParameterVersionedReadModel ConvertVersionedDbModelToVersionedApiModel(ParameterVersionedMapping versionedMapping)
        {
            return new ParameterVersionedReadModel
            {
                Id = versionedMapping.Id,
                VersionedViewModelFor = versionedMapping.Mapping.Id,
                GameVersion = versionedMapping.GameVersion.Id,
                CurrentMappings = versionedMapping.CommittedMappings.ToList().Select(ConvertCommittedMappingToSimpleReadModel),
                Proposals = versionedMapping.ProposalMappings.ToList().Select(ConvertProposalMappingToReadModel),
                ParameterOf = versionedMapping.ParameterOf.Id,
                Index = versionedMapping.Index
            };
        }

        protected override MappingReadModel ConvertCommittedMappingToSimpleReadModel(ParameterCommittedMappingEntry committedMapping)
        {
            return new MappingReadModel {Id = committedMapping.Id, In = committedMapping.InputMapping, Out = committedMapping.OutputMapping, Documentation = committedMapping.Documentation};
        }

        protected override ProposalReadModel ConvertProposalMappingToReadModel(ParameterProposalMappingEntry proposalMapping) =>
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
