using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.Core.Readers.Core;
using Data.Core.Readers.Mapping;
using Data.Core.Writers.Core;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Model.Creation.Field;
using Data.WebApi.Model.Read.Field;
using Data.WebApi.Services.Converters;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Fields
{
    /// <summary>
    /// Controller that handles interactions on versioned field levels.
    /// </summary>
    [Route("/fields/versioned")]
    public class VersionedFieldsController
        : VersionedComponentControllerBase<FieldVersionedReadModel>
    {
        private readonly IGameVersionReader _gameVersionReader;

        private readonly IUserResolvingService _userResolvingService;

        private readonly IMappingTypeReader _mappingTypeReader;

        private readonly IClassComponentReader _classComponentReader;

        public VersionedFieldsController(IComponentWriter componentWriter, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader, IClassComponentReader classComponentReader) : base(componentWriter)
        {
            _gameVersionReader = gameVersionReader;
            _userResolvingService = userResolvingService;
            _mappingTypeReader = mappingTypeReader;
            _classComponentReader = classComponentReader;
        }

        /// <summary>
        /// Creates a new versioned field entry for an already existing field mapping.
        /// Creates a versioned mapping field for the given version, as well a single committed mapping.
        /// </summary>
        /// <param name="mapping">The versioned mapping to create.</param>
        /// <returns>An http response code:201-New mapping created,400-The request was invalid,404-Certain data could not be found,401-Unauthorized,409-A versioned field for that version already exists with the field.</returns>
        [HttpPost("create/{versionId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> Create([FromBody] CreateVersionedFieldModel mapping)
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

            var fieldMapping = await ComponentWriter.GetById(mapping.VersionedMappingFor);
            if (fieldMapping == null)
                return BadRequest("Unknown field mapping to create the versioned mapping for.");

            if (fieldMapping.VersionedComponents.Any(versionedMapping =>
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
                VersionedComponent = versionedFieldMapping,
                VersionedComponentForeignKey = versionedFieldMapping.Id,
                MemberOf = memberOf.Metadata as ClassMetadata,
                IsStatic = mapping.IsStatic
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
                    VersionedComponent = versionedFieldMapping,
                    CreatedOn = DateTime.Now
                });

            versionedFieldMapping.Mappings.AddRange(initialLiveMappings);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", fieldMapping.Id, fieldMapping);
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
                CurrentMappings = versionedComponent.Mappings.ToList().Select(ConverterUtils.ConvertLiveDbModelToMappingReadModel),
                Proposals = versionedComponent.Proposals.ToList().Select(ConverterUtils.ConvertProposalDbModelToProposalReadModel),
                MemberOf = fieldMetaData.MemberOf.VersionedComponent.Id,
                IsStatic = fieldMetaData.IsStatic,
                LockedMappingNames = versionedComponent.LockedMappingTypes.ToList().Select(lm => lm.MappingType.Name)
            };
        }
    }
}
