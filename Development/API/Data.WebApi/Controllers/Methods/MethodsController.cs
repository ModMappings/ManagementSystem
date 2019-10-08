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
using Mcms.Api.Data.Poco.Writers.Mapping;
using Data.WebApi.Controllers.Base;
using Data.WebApi.Extensions;
using Data.WebApi.Model.Creation.Method;
using Data.WebApi.Model.Read.Core;
using Data.WebApi.Model.Read.Method;
using Data.WebApi.Services.Converters;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.WebApi.Controllers.Methods
{
    /// <summary>
    /// Controller that handles interactions on method levels.
    /// </summary>
    [Route("/methods")]
    [ApiController]
    public class MethodsController : ComponentControllerBase<MethodReadModel, MethodVersionedReadModel>
    {
        public MethodsController(IMethodComponentWriter methodComponentWriter, IClassComponentReader classComponentReader, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader) : base(methodComponentWriter, releaseReader, gameVersionReader, userResolvingService, mappingTypeReader)
        {
            this.MethodComponentWriter = methodComponentWriter;
            this.ClassComponentReader = classComponentReader;
        }

        private IMethodComponentWriter MethodComponentWriter { get; }

        private IClassComponentReader ClassComponentReader { get; }

        /// <summary>
        /// Gets the methods which are part of a given class.
        /// those methods also need to have at least one mapping (regardless of type) within the latest gameversion.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="classId">The id of the class the method needs to be part of.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The methods who's class match the id, and are part of the latest gameversion.</returns>
        [HttpGet("class/version/{classId}/latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<MethodReadModel>>> GetByClassInLatestGameVersion(Guid classId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await MethodComponentWriter.GetByClassInLatestGameVersion(classId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Counts the methods which are part of a given class
        /// those methods also need to have at least one mapping (regardless of type) within the latest gameversion.
        /// </summary>
        /// <param name="classId">The id of the class the method needs to be part of.</param>
        /// <returns>The count of methods who's class match the id, and are part of the latest gameversion.</returns>
        [HttpGet("class/version/{classId}/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountClassInLatestGameVersion(Guid classId)
        {
            var dbModels = await MethodComponentWriter.GetByClassInLatestGameVersion(classId);

            return Content((await dbModels.CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the methods which are part of a given class.
        /// those methods also need to have at least one mapping (regardless of type) within a gameversion that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="classId">The id of the class which the method has to be part of.</param>
        /// <param name="gameVersionId">The id of the gameversion that the component needs to be in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The methods who's class match the id, and are part of the given gameversion.</returns>
        [HttpGet("class/version/{classId}/{gameVersionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<MethodReadModel>>> GetByClassInGameVersion(Guid classId, Guid gameVersionId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await MethodComponentWriter.GetByClassInGameVersion(classId, gameVersionId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Gets the methods which are part of a given class
        /// those methods also need to have at least one mapping (regardless of type) within the latest release.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="classId">The id of the class the method has to be part of.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The methods who's class match the id, and are part of the latest release.</returns>
        [HttpGet("class/release/{classId}/latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<MethodReadModel>>> GetByClassInLatestRelease(Guid classId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await MethodComponentWriter.GetByClassInLatestRelease(classId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Gets the methods which are part of a given class
        /// those methods also need to have at least one mapping (regardless of type) within a release that has the given id.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="classId">The id of the class the method has to be part of.</param>
        /// <param name="releaseId">The id of the release that the component needs to be in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The methods who's class match the id, and are part of the given release.</returns>
        [HttpGet("class/release/{classId}/{releaseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<MethodReadModel>>> GetByClassInRelease(Guid classId, Guid releaseId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await MethodComponentWriter.GetByClassInRelease(classId, releaseId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
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
        [Authorize()]
        public async Task<ActionResult> AddToLatest([FromBody] CreateMethodModel mapping)
        {
            var currentLatestGameVersion = await GameVersionReader.GetLatest();
            if (currentLatestGameVersion == null)
                return BadRequest("No game version has been registered yet.");

            var user = await UserResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            VersionedComponent memberOf = await ClassComponentReader.GetVersionedComponent(mapping.MemberOf);
            if (memberOf == null)
                return BadRequest("Unknown memberOf class.");

            var versionedMethodMapping = new VersionedComponent
            {
                CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                GameVersion = currentLatestGameVersion,
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
                    MappingType = MappingTypeReader.GetByName(mappingData.MappingTypeName).Result,
                    ProposedMapping = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = versionedMethodMapping,
                    CreatedOn = DateTime.Now
                });

            versionedMethodMapping.Mappings.AddRange(initialLiveMappings);

            var methodMapping = new Component
            {
                Id = Guid.NewGuid(),
                Type = ComponentType.METHOD,
                VersionedComponents = new List<VersionedComponent>() {versionedMethodMapping}
            };

            await ComponentWriter.Add(methodMapping);
            await ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", methodMapping.Id, methodMapping);
        }



        protected override MethodReadModel ConvertDbModelToReadModel(Component component)
        {
            return new MethodReadModel
            {
                Id = component.Id,
                Versioned = component.VersionedComponents.ToList().Select(ConvertVersionedDbModelToReadModel)
            };
        }

        private MethodVersionedReadModel ConvertVersionedDbModelToReadModel(VersionedComponent versionedComponent)
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
