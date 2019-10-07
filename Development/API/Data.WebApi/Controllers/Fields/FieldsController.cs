using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.Core.Models.Mapping.Proposals;
using Data.Core.Readers.Core;
using Data.Core.Readers.Mapping;
using Data.Core.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Extensions;
using Data.WebApi.Model.Creation.Field;
using Data.WebApi.Model.Read.Core;
using Data.WebApi.Model.Read.Field;
using Data.WebApi.Services.Converters;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.WebApi.Controllers.Fields
{
    /// <summary>
    /// Controller that handles interactions on field levels.
    /// </summary>
    [Route("/fields")]
    [ApiController]
    public class FieldsController : ComponentControllerBase<FieldReadModel, FieldVersionedReadModel>
    {
        private readonly IFieldComponentWriter _fieldComponentWriter;

        private readonly IClassComponentReader _classComponentReader;

        public FieldsController(IFieldComponentWriter fieldComponentWriter, IClassComponentReader classComponentReader, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(fieldComponentWriter, releaseReader, gameVersionReader, userResolvingService, mappingTypeReader)
        {
            _fieldComponentWriter = fieldComponentWriter;
            _classComponentReader = classComponentReader;
        }

        /// <summary>
        /// Gets the fields which are part of a given class.
        /// those fields also need to have at least one mapping (regardless of type) within the latest gameversion.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="classId">The id of the class the field needs to be part of.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The fields who's class match the id, and are part of the latest gameversion.</returns>
        [HttpGet("class/version/{classId}/latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<FieldReadModel>>> GetByClassInLatestGameVersion(Guid classId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await _fieldComponentWriter.GetByClassInLatestGameVersion(classId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Counts the fields which are part of a given class
        /// those fields also need to have at least one mapping (regardless of type) within the latest gameversion.
        /// </summary>
        /// <param name="classId">The id of the class the field needs to be part of.</param>
        /// <returns>The count of fields who's class match the id, and are part of the latest gameversion.</returns>
        [HttpGet("class/version/{classId}/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountClassInLatestGameVersion(Guid classId)
        {
            var dbModels = await _fieldComponentWriter.GetByClassInLatestGameVersion(classId);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the fields which are part of a given class.
        /// those fields also need to have at least one mapping (regardless of type) within a gameversion that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="classId">The id of the class which the field has to be part of.</param>
        /// <param name="gameVersionId">The id of the gameversion that the component needs to be in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The fields who's class match the id, and are part of the given gameversion.</returns>
        [HttpGet("class/version/{classId}/{gameVersionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<FieldReadModel>>> GetByClassInGameVersion(Guid classId, Guid gameVersionId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await _fieldComponentWriter.GetByClassInGameVersion(classId, gameVersionId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Gets the fields which are part of a given class
        /// those fields also need to have at least one mapping (regardless of type) within the latest release.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="classId">The id of the class the field has to be part of.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The fields who's class match the id, and are part of the latest release.</returns>
        [HttpGet("class/release/{classId}/latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<FieldReadModel>>> GetByClassInLatestRelease(Guid classId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await _fieldComponentWriter.GetByClassInLatestRelease(classId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Gets the fields which are part of a given class
        /// those fields also need to have at least one mapping (regardless of type) within a release that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="classId">The id of the class the field has to be part of.</param>
        /// <param name="releaseId">The id of the release that the component needs to be in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The fields who's class match the id, and are part of the given release.</returns>
        [HttpGet("class/release/{classId}/{releaseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<FieldReadModel>>> GetByClassInRelease(Guid classId, Guid releaseId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await _fieldComponentWriter.GetByClassInRelease(classId, releaseId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Creates a new field and its central mapping entry.
        /// Creates a new core mapping field, a versioned mapping field for the latest version, as well a single committed mapping.
        /// </summary>
        /// <param name="mapping">The mapping to create.</param>
        /// <returns>An http response code:201-New mapping created,400-The request was invalid,404-Certain data could not be found,401-Unauthorized.</returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> Create([FromBody] CreateFieldModel mapping)
        {
            var currentLatestGameVersion = await GameVersionReader.GetLatest();
            if (currentLatestGameVersion == null)
                return BadRequest("No game version has been registered yet.");

            var user = await UserResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent memberOf = await _classComponentReader.GetVersionedComponent(mapping.MemberOf);
            if (memberOf == null)
                return BadRequest("Unknown memberOf class.");

            var versionedFieldMapping = new VersionedComponent
            {
                CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                GameVersion = currentLatestGameVersion,
                Mappings = new List<LiveMappingEntry>(),
                Proposals = new List<ProposedMapping>()
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
                    MappingType = MappingTypeReader.GetByName(mappingData.MappingTypeName).Result,
                    ProposedMapping = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = versionedFieldMapping,
                    CreatedOn = DateTime.Now
                });

            versionedFieldMapping.Mappings.AddRange(initialLiveMappings);

            var fieldMapping = new Component
            {
                Id = Guid.NewGuid(),
                Type = ComponentType.FIELD,
                VersionedComponents = new List<VersionedComponent>() {versionedFieldMapping}
            };

            await ComponentWriter.Add(fieldMapping);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", fieldMapping.Id, fieldMapping);
        }



        protected override FieldReadModel ConvertDbModelToReadModel(Component component)
        {
            return new FieldReadModel
            {
                Id = component.Id,
                Versioned = component.VersionedComponents.ToList().Select(ConvertVersionedDbModelToReadModel)
            };
        }

        private FieldVersionedReadModel ConvertVersionedDbModelToReadModel(VersionedComponent versionedComponent)
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
