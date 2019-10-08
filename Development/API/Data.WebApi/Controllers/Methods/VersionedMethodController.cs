using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Mcms.Api.Data.Poco.Models.Mapping;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Mapping.Metadata;
using Mcms.Api.Data.Poco.Readers.Core;
using Mcms.Api.Data.Poco.Readers.Mapping;
using Mcms.Api.Data.Poco.Writers.Core;
using Mcms.Api.Data.Poco.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Model.Creation.Method;
using Data.WebApi.Model.Read.Method;
using Data.WebApi.Services.Converters;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Methods
{
    /// <summary>
    /// Controller that handles interactions on versioned method levels.
    /// </summary>
    [Route("/methods/versioned")]
    [ApiController]
    public class VersionedMethodController
        : VersionedComponentControllerBase<MethodVersionedReadModel>
    {
        private readonly IGameVersionReader _gameVersionReader;

        private readonly IUserResolvingService _userResolvingService;

        private readonly IMappingTypeReader _mappingTypeReader;

        private readonly IClassComponentReader _classComponentReader;

        public VersionedMethodController(IMethodComponentWriter componentWriter, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader, IClassComponentReader classComponentReader) : base(componentWriter)
        {
            _gameVersionReader = gameVersionReader;
            _userResolvingService = userResolvingService;
            _mappingTypeReader = mappingTypeReader;
            _classComponentReader = classComponentReader;
        }

        /// <summary>
        /// Creates a new versioned method entry for an already existing method mapping.
        /// Creates a versioned mapping method for the given version, as well a single committed mapping.
        /// </summary>
        /// <param name="mapping">The versioned mapping to create.</param>
        /// <returns>An http response code:201-New mapping created,400-The request was invalid,404-Certain data could not be found,401-Unauthorized,409-A versioned method for that version already exists with the method.</returns>
        [HttpPost("create/{versionId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> Create([FromBody] CreateVersionedMethodModel mapping)
        {
            var currentGameVersion = await _gameVersionReader.GetById(mapping.GameVersion);
            if (currentGameVersion == null)
                return BadRequest("No game version with that id has been registered yet.");

            var user = await _userResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent memberOf = await _classComponentReader.GetVersionedComponent(mapping.MemberOf);
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
                Mappings = new List<CommittedMapping>(),
                Proposals = new List<ProposedMapping>()
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
                .Select(mappingData => new CommittedMapping()
                {
                    Documentation = mappingData.Documentation,
                    Distribution = mappingData.Distribution,
                    InputMapping = mappingData.In,
                    OutputMapping = mappingData.Out,
                    MappingType = _mappingTypeReader.GetByName(mappingData.MappingTypeName).Result,
                    ProposedMapping = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = versionedMethodMapping,
                    CreatedOn = DateTime.Now
                });

            versionedMethodMapping.Mappings.AddRange(initialLiveMappings);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", methodMapping.Id, methodMapping);
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
                CurrentMappings = versionedComponent.Mappings.ToList().Select(ConverterUtils.ConvertLiveDbModelToMappingReadModel),
                Proposals = versionedComponent.Proposals.ToList().Select(ConverterUtils.ConvertProposalDbModelToProposalReadModel),
                MemberOf = methodMetaData.MemberOf.VersionedComponent.Id,
                Descriptor = methodMetaData.Descriptor,
                IsStatic = methodMetaData.IsStatic,
                LockedMappingNames = versionedComponent.LockedMappingTypes.ToList().Select(lm => lm.MappingType.Name),
                Parameters = methodMetaData.Parameters.Select(p => p.VersionedComponent.Id)
            };
        }
    }
}
