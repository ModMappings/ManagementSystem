using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model.Read.Class;
using API.Model.Read.Core;
using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Writers.Class;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// Controller that handles interactions on class levels.
    /// </summary>
    [Route("/classes")]
    [ApiController]
    public class ClassController : Controller
    {
        /// <summary>
        /// The writer and as such also the reader for class mappings.
        /// </summary>
        private readonly IClassMappingWriter _writer;

        /// <summary>
        /// Creates a new controller.
        /// Called via DI.
        /// </summary>
        /// <param name="writer">The writer for class mappings.</param>
        public ClassController(IClassMappingWriter writer)
        {
            _writer = writer;
        }

        /// <summary>
        /// Allows for the lookup of a single class model by its id.
        /// Returns 404 if no model exists with the given id.
        /// </summary>
        /// <param name="id">The id for which a class model is retrieved.</param>
        /// <returns>The class model with a matching id if it exists.</returns>
        [HttpGet("id/{id}", Name = "ById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassReadModel>> GetById(Guid id)
        {
            var _dbModel = await _writer.GetById(id);

            if (_dbModel == null)
                return NotFound();

            var readModel = ConvertClassModelToReadModel(_dbModel);

            return Json(readModel);
        }

        /// <summary>
        /// Allows for the lookup of the entire class model table.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="pageSize">The size of a single page.</param>
        /// <param name="pageIndex">The 0-based index of the page to display.</param>
        /// <returns>A list of all class models on the requested page, possible empty if no models exist on the given page.</returns>
        [HttpGet("all/{pageSize}/{pageIndex}", Name = "All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IQueryable<ClassReadModel>>> AsMappingQueryable(int pageSize, int pageIndex)
        {
            var _dbModels = (await _writer.AsMappingQueryable()).Skip(pageSize * pageIndex).Take(pageSize);

            var readModels = _dbModels.ToList().Select(ConvertClassModelToReadModel);

            return Json(readModels);
        }

        /// <summary>
        /// Calculates the amount of class mappings that are currently in use.
        /// </summary>
        /// <returns>The amount of class mappings.</returns>
        [HttpGet("/all/count", Name = "Count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> Count()
        {
            return Ok(await (await _writer.AsMappingQueryable()).CountAsync());
        }

        /// <summary>
        /// The class models that are part of the current version.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <param name="pageIndex">The 0-based index of the page to get.</param>
        /// <returns>The class models that are part of the current release and are on the requested page.</returns>
        [HttpGet("release/latest/{pageSize}/{pageIndex}", Name = "ByLatestRelease")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IQueryable<ClassReadModel>>> GetByLatestRelease(int pageSize, int pageIndex)
        {
            var _dbModels = await _writer.GetByLatestRelease();

            var readModels = _dbModels.ToList().Select(ConvertClassModelToReadModel).Skip(pageSize * pageIndex).Take(pageIndex);

            return Json(readModels);
        }

        /// <summary>
        /// Counts the amount of classes that are contained in the latest release.
        /// </summary>
        /// <returns>The amount of classes that are contained in the latest release</returns>
        [HttpGet("release/latest/count", Name = "CountOfLatestRelease")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetByLatestReleaseCount()
        {
            return Ok(await (await _writer.GetByLatestRelease()).CountAsync());
        }

        /// <summary>
        /// The class models that are part of a given release based on the given id.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="releaseId">The id of the release for which the classes are being pulled.</param>
        /// <param name="pageSize">The size of the page that is being requested.</param>
        /// <param name="pageIndex">The 0-based index of the page that is being requested.</param>
        /// <returns>The classes that are part of the given release and are on the requested page.</returns>
        [HttpGet("release/{releaseId}/{pageSize}/{pageIndex}", Name = "ByReleaseById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IQueryable<ClassReadModel>>> GetByRelease(Guid releaseId, int pageSize, int pageIndex)
        {
            var _dbModels = await _writer.GetByRelease(releaseId);

            var readModels = _dbModels.ToList().Select(ConvertClassModelToReadModel).Skip(pageSize * pageIndex).Take(pageIndex);

            return Json(readModels);
        }

        /// <summary>
        /// Counts the class models that are part of a given release.
        /// </summary>
        /// <param name="releaseId">The id of the release for which the classes are being pulled.</param>
        /// <returns>The amount of classes that are part of the given release.</returns>
        [HttpGet("release/count/{releaseId}", Name = "ByReleaseById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetByReleaseCount(Guid releaseId)
        {
            return Ok(await (await _writer.GetByRelease(releaseId)).CountAsync());
        }

        /// <summary>
        /// The class models that are part of a given release with the given name.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="releaseName">The name of the release for which the class models are retrieved.</param>
        /// <param name="pageSize">The size of the page for which the class models are retrieved. </param>
        /// <param name="pageIndex">The index of the page for which the class models are retrieved.</param>
        /// <returns>The classes that are part of the given release with the requested name as well as are on the requested page.</returns>
        [HttpGet("release/{releaseName}/{pageSize}/{pageIndex}", Name = "ByReleaseByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IQueryable<ClassReadModel>>> GetByRelease(string releaseName, int pageSize, int pageIndex)
        {
            var _dbModels = await _writer.GetByRelease(releaseName);

            var readModels = _dbModels.ToList().Select(ConvertClassModelToReadModel).Skip(pageSize * pageIndex).Take(pageIndex);

            return Json(readModels);
        }

        [HttpGet("byMapping/latest/{name}", Name = "ByLatestMapping")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassReadModel>> GetByLatestMapping(string name)
        {
            var _dbModels = await _writer.GetByLatestMapping(name);

            if (_dbModels == null)
                return NotFound();

            var readModels = ConvertClassModelToReadModel(_dbModels);

            return Json(readModels);
        }

        [HttpGet("byMapping/version/{name}/{versionId}", Name = "ByMappingInVersionFromId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassReadModel>> GetByMappingInVersion(string name, Guid versionId)
        {
            var _dbModels = await _writer.GetByMappingInVersion(name, versionId);

            if (_dbModels == null)
                return NotFound();

            var readModels = ConvertClassModelToReadModel(_dbModels);

            return Json(readModels);
        }

        [HttpGet("byMapping/version/{name}", Name = "ByMappingInVersionFromReadModel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassReadModel>> GetByMappingInVersion(string name, [FromBody] GameVersionReadModel gameVersion)
        {
            return await GetByMappingInVersion(name, gameVersion.Id);
        }

        [HttpGet("byMapping/release/{name}/{releaseId}", Name = "ByMappingInReleaseFromId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassReadModel>> GetByMappingInRelease(string name, Guid releaseId)
        {
            var _dbModels = await _writer.GetByMappingInRelease(name, releaseId);

            if (_dbModels == null)
                return NotFound();

            var readModels = ConvertClassModelToReadModel(_dbModels);

            return Json(readModels);
        }

        [HttpGet("byMapping/release/{name}", Name = "ByMappingInReleaseFromViewModel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassReadModel>> GetByMappingInRelease(string name, [FromBody] ReleaseReadModel release)
        {
            return await GetByMappingInRelease(name, release.Id);
        }

        [HttpPost("/add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Add([FromBody] ClassReadModel mapping)
        {
            //TODO: User auth
            throw new NotImplementedException();
        }

        [HttpPost("/add/all")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status206PartialContent)]
        public async Task<ActionResult> AddAll([FromBody] IEnumerable<ClassReadModel> mapping)
        {
            //TODO: User auth and handling logic.
            throw new NotImplementedException();
        }

        [HttpPatch("/update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Update([FromBody] ClassReadModel mapping)
        {
            //TODO: User auth
            throw new NotImplementedException();
        }

        [HttpPatch("/update/all")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status206PartialContent)]
        public async Task<ActionResult> UpdateAll([FromBody] IEnumerable<ClassReadModel> mapping)
        {
            //TODO: User auth
            throw new NotImplementedException();
        }

        private ClassReadModel ConvertClassModelToReadModel(ClassMapping classModel)
        {
            return new ClassReadModel
            {
                Id = classModel.Id,
                Versioned = classModel.VersionedMappings.ToList().Select(ConvertVersionedModelToReadModel)
            };
        }

        private ClassVersionedReadModel ConvertVersionedModelToReadModel(ClassVersionedMapping versionedMapping)
        {
            var outerId = versionedMapping.Outer?.Id;

            return new ClassVersionedReadModel
            {
                Id = versionedMapping.Id,
                VersionedViewModelFor = versionedMapping.Mapping.Id,
                GameVersion = versionedMapping.GameVersion.Id,
                Outer = outerId,
                Package = versionedMapping.Package,
                InheritsFrom = versionedMapping.InheritsFrom.ToList().Select(parentClass => parentClass.Id),
                CurrentMappings = versionedMapping.CommittedMappings.ToList().Select(ConvertCommittedMappingToSimpleReadModel),
                Proposals = versionedMapping.ProposalMappings.ToList().Select(ConvertProposalMappingToReadModel)
            };
        }

        private MappingReadModel ConvertCommittedMappingToSimpleReadModel(ClassCommittedMappingEntry committedMapping)
        {
            return new MappingReadModel {Id = committedMapping.Id, In = committedMapping.InputMapping, Out = committedMapping.OutputMapping};
        }

        private static ProposalReadModel ConvertProposalMappingToReadModel(ClassProposalMappingEntry proposalMapping)
        {
            return new ProposalReadModel()
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
                Out = proposalMapping.OutputMapping
            };
        }
    }
}
