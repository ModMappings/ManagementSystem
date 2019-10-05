using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Readers.Core;
using Data.Core.Writers.Core;
using Data.WebApi.Extensions;
using Data.WebApi.Model.Read.Core;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Base
{
    public abstract class ComponentControllerBase<TReadModel, TVersionedReadModel> : Controller
        where TReadModel : AbstractReadModel<TVersionedReadModel>
        where TVersionedReadModel : AbstractVersionedReadModel
    {

        protected readonly IComponentWriter ComponentWriter;

        protected readonly IReleaseReader ReleaseReader;

        protected readonly IGameVersionReader GameVersionReader;

        protected readonly IUserResolvingService UserResolvingService;

        protected readonly IMappingTypeReader MappingTypeReader;

        protected ComponentControllerBase(IComponentWriter componentWriter, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader)
        {
            ComponentWriter = componentWriter;
            ReleaseReader = releaseReader;
            GameVersionReader = gameVersionReader;
            UserResolvingService = userResolvingService;
            MappingTypeReader = mappingTypeReader;
        }

        /// <summary>
        /// Returns the component with the given id.
        /// </summary>
        /// <param name="id">The id of the component to lookup.</param>
        /// <returns>200-The component with the requested id. 404-Not found.</returns>
        [HttpGet("id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<TReadModel>> GetById(Guid id)
        {
            var byId = await ComponentWriter.GetById(id);

            if (byId == null)
                return NotFound();

            return Json(ConvertDbModelToReadModel(byId));
        }

        /// <summary>
        /// Returns all of the components stored in the database.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The components in the store on the requested page.</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<TReadModel>>> AsQueryable([FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await ComponentWriter.AsQueryable();

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Returns all components, stored in the database, that are part of the latest release.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>All components, stored in the database, that are part of the latest release.</returns>
        [HttpGet("release/latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<TReadModel>>> GetByLatestRelease([FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var latestRelease = await ReleaseReader.GetLatest();

            return await GetByReleaseById(latestRelease.Id, pageSize, pageIndex);
        }


        /// <summary>
        /// Returns all components, stored in the database, that are part of the requested release.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="releaseId">The id of the release in question.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>All components, stored in the database, that are part of the latest release.</returns>
        [HttpGet("release/{releaseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<TReadModel>>> GetByReleaseById(Guid releaseId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await ComponentWriter.GetByRelease(releaseId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Returns all components, stored in the database, that are part of the requested release.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="releaseName">The name of the release in question.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>All components, stored in the database, that are part of the latest release.</returns>
        [HttpGet("release/{releaseName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<TReadModel>>> GetByReleaseByName(string releaseName, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var latestRelease = await ReleaseReader.GetByName(releaseName);

            return await GetByReleaseById(latestRelease.Id, pageSize, pageIndex);
        }

        /// <summary>
        /// Returns all components, stored in the database, that are part of the latest version.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>All components, stored in the database, that are part of the latest version.</returns>
        [HttpGet("version/latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<TReadModel>>> GetByLatestVersion([FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var latestVersion = await GameVersionReader.GetLatest();

            return await GetByVersionById(latestVersion.Id, pageSize, pageIndex);
        }

        /// <summary>
        /// Returns all components, stored in the database, that are part of the requested version.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="versionId">The id of the version in question.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>All components, stored in the database, that are part of the latest version.</returns>
        [HttpGet("version/{versionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<TReadModel>>> GetByVersionById(Guid versionId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await ComponentWriter.GetByVersion(versionId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Returns all components, stored in the database, that are part of the requested version.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="versionName">The name of the version in question.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>All components, stored in the database, that are part of the latest version.</returns>
        [HttpGet("version/{versionName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<TReadModel>>> GetByVersionByName(string versionName, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var latestVersion = await GameVersionReader.GetByName(versionName);

            return await GetByVersionById(latestVersion.Id, pageSize, pageIndex);
        }

        /// <summary>
        /// Gets all the components with a given name in its latest mappings.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="name">The name to look for.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The components with a matching mapping.</returns>
        [HttpGet("mapping/latest/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<TReadModel>>> GetByLatestMapping(string name, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await ComponentWriter.GetByLatestMapping(name);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Gets all the components with a given name in its latest mappings within the given version.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="name">The name to look for.</param>
        /// <param name="versionId">The id of the version to look in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The components with a matching mapping in the given version.</returns>
        [HttpGet("mapping/version/{versionId}/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<TReadModel>>> GetByMappingInVersion(string name, Guid versionId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await ComponentWriter.GetByMappingInVersion(name, versionId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        /// <summary>
        /// Gets all the components with a given name in its latest mappings within the given release.
        /// </summary>
        /// <remarks>Has pagination support via request query parameters.</remarks>
        /// <param name="name">The name to look for.</param>
        /// <param name="releaseId">The id of the release to look in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The components with a matching mapping in the given release.</returns>
        [HttpGet("mapping/release/{releaseId}/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<PagedList<TReadModel>>> GetByMappingInRelease(string name, Guid releaseId, [FromQuery] int pageSize = 25, [FromQuery] int pageIndex = 0)
        {
            var dbModels = await ComponentWriter.GetByMappingInRelease(name, releaseId);

            return Json(dbModels.AsPagedListWithSelect(ConvertDbModelToReadModel, pageIndex, pageSize));
        }

        protected abstract TReadModel ConvertDbModelToReadModel(Component component);
    }
}
