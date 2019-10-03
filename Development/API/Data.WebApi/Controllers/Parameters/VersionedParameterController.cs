using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.Core.Readers.Core;
using Data.Core.Readers.Mapping;
using Data.Core.Writers.Core;
using Data.Core.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Model.Creation.Parameter;
using Data.WebApi.Model.Read.Parameter;
using Data.WebApi.Services.Converters;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Parameters
{
    public class VersionedParameterController
        : VersionedComponentControllerBase<ParameterVersionedReadModel>
    {
        private readonly IGameVersionReader _gameVersionReader;

        private readonly IUserResolvingService _userResolvingService;

        private readonly IMappingTypeReader _mappingTypeReader;

        private readonly IMethodComponentReader _methodComponentReader;

        public VersionedParameterController(IParameterComponentWriter componentWriter, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader, IMethodComponentReader methodComponentReader) : base(componentWriter)
        {
            _gameVersionReader = gameVersionReader;
            _userResolvingService = userResolvingService;
            _mappingTypeReader = mappingTypeReader;
            _methodComponentReader = methodComponentReader;
        }

        /// <summary>
        /// Creates a new versioned parameter entry for an already existing parameter mapping.
        /// Creates a versioned mapping parameter for the given version, as well a single committed mapping.
        /// </summary>
        /// <param name="mapping">The versioned mapping to create.</param>
        /// <returns>An http response code:201-New mapping created,400-The request was invalid,404-Certain data could not be found,401-Unauthorized,409-A versioned parameter for that version already exists with the parameter.</returns>
        [HttpPost("create/{versionId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> Create([FromBody] CreateVersionedParameterModel mapping)
        {
            var currentGameVersion = await _gameVersionReader.GetById(mapping.GameVersion);
            if (currentGameVersion == null)
                return BadRequest("No game version with that id has been registered yet.");

            var user = await _userResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent memberOf = await _methodComponentReader.GetVersionedComponent(mapping.ParameterOf);
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
                    MappingType = _mappingTypeReader.GetByName(mappingData.MappingTypeName).Result,
                    Proposal = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = versionedParameterMapping,
                    CreatedOn = DateTime.Now
                });

            versionedParameterMapping.Mappings.AddRange(initialLiveMappings);

            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", parameterMapping.Id, parameterMapping);
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
                CurrentMappings = versionedComponent.Mappings.ToList().Select(ConverterUtils.ConvertLiveDbModelToMappingReadModel),
                Proposals = versionedComponent.Proposals.ToList().Select(ConverterUtils.ConvertProposalDbModelToProposalReadModel),
                ParameterOf = parameterMetaData.ParameterOf.VersionedComponent.Id,
                Index = parameterMetaData.Index,
                LockedMappingNames = versionedComponent.LockedMappingTypes.ToList().Select(lm => lm.MappingType.Name)
            };
        }
    }
}
