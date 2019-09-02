using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model.Creation.Core;
using API.Model.Read.Core;
using API.Services.Core;
using Data.Core.Models.Core;
using Data.Core.Readers.Core;
using Data.Core.Writers.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public abstract class
        NoneUniqueMappingControllerBase<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry, TAPIModel, TVersionedReadModel, TCreateReadModel, TCreateVersionedModel> : Controller
        where TMapping : AbstractMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TVersionedMapping : AbstractVersionedMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TCommittedEntry :
        AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>, new()
        where TProposalEntry :
        AbstractProposalMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>, new()
        where TReleaseEntry : AbstractReleaseMember<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
    {
        /// <summary>
        /// The readerWriter and as such also the reader for mappings.
        /// </summary>
        protected INoneUniqueNamedMappingWriter<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
            _readerWriter;

        /// <summary>
        /// The reader for game versions.
        /// </summary>
        protected IGameVersionReader _gameVersionReader;

        /// <summary>
        /// The user resolving service.
        /// </summary>
        protected IUserResolvingService _userResolvingService;

        /// <summary>
        /// Creates a new controller.
        /// Called via DI.
        /// </summary>
        /// <param name="readerWriter">The readerWriter for mappings.</param>
        /// <param name="gameVersionReader">The reader for game versions.</param>
        /// <param name="userResolvingService">The service used to resolve the user.</param>
        protected NoneUniqueMappingControllerBase(
            INoneUniqueNamedMappingWriter<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry> readerWriter,
            IGameVersionReader gameVersionReader,
            IUserResolvingService userResolvingService)
        {
            _readerWriter = readerWriter;
            _userResolvingService = userResolvingService;
            _gameVersionReader = gameVersionReader;
        }

        /// <summary>
        /// Allows for the lookup of a single model by its id.
        /// Returns 404 if no model exists with the given id.
        /// </summary>
        /// <param name="id">The id for which a model is retrieved.</param>
        /// <returns>The model with a matching id if it exists.</returns>
        [HttpGet("id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<TAPIModel>> GetById(Guid id)
        {
            var dbModel = await _readerWriter.GetById(id);

            if (dbModel == null)
                return NotFound();

            var readModel = ConvertDBModelToApiModel(dbModel);

            return Json(readModel);
        }

        /// <summary>
        /// Allows for the lookup of the entire model table.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="pageSize">The size of a single page.</param>
        /// <param name="pageIndex">The 0-based index of the page to display.</param>
        /// <returns>A list of all models on the requested page, possible empty if no models exist on the given page.</returns>
        [HttpGet("all/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TAPIModel>>> AsMappingQueryable(int pageSize, int pageIndex)
        {
            var dbModels = (await _readerWriter.AsMappingQueryable()).Skip(pageSize * pageIndex).Take(pageSize);

            var readModels = dbModels.ToList().Select(ConvertDBModelToApiModel);

            return Json(readModels);
        }

        /// <summary>
        /// Calculates the amount of mappings that are currently in use.
        /// </summary>
        /// <returns>The amount of mappings.</returns>
        [HttpGet("all/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> Count()
        {
            return Content((await (await _readerWriter.AsMappingQueryable()).CountAsync()).ToString());
        }

        /// <summary>
        /// The models that are part of the current version.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <param name="pageIndex">The 0-based index of the page to get.</param>
        /// <returns>The models that are part of the current release and  are on the requested page.</returns>
        [HttpGet("release/latest/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TAPIModel>>> GetByLatestRelease(int pageSize, int pageIndex)
        {
            var dbModels = await _readerWriter.GetByLatestRelease();

            var readModels = dbModels.Skip(pageSize * pageIndex)
                                                     .Take(pageSize).ToList().Select(ConvertDBModelToApiModel);

            return Json(readModels);
        }

        /// <summary>
        /// Counts the amount of that are contained in the latest release.
        /// </summary>
        /// <returns>The amount of that are contained in the latest release</returns>
        [HttpGet("release/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByLatestReleaseCount()
        {
            return Content((await (await _readerWriter.GetByLatestRelease()).CountAsync()).ToString());
        }

        /// <summary>
        /// The models that are part of a given release based on the given id.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="releaseId">The id of the release for which the models are being pulled.</param>
        /// <param name="pageSize">The size of the page that is being requested.</param>
        /// <param name="pageIndex">The 0-based index of the page that is being requested.</param>
        /// <returns>The models that are part of the given release and are on the requested page.</returns>
        [HttpGet("release/{releaseId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TAPIModel>>> GetByRelease(Guid releaseId, int pageSize,
            int pageIndex)
        {
            var dbModels = await _readerWriter.GetByRelease(releaseId);

            var readModels = dbModels.Skip(pageSize * pageIndex)
                                                     .Take(pageSize).ToList().Select(ConvertDBModelToApiModel);

            return Json(readModels);
        }

        /// <summary>
        /// Counts the models that are part of a given release.
        /// </summary>
        /// <param name="releaseId">The id of the release for which the models are being pulled.</param>
        /// <returns>The amount of models that are part of the given release.</returns>
        [HttpGet("release/count/{releaseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByReleaseCount(Guid releaseId)
        {
            return Content((await (await _readerWriter.GetByRelease(releaseId)).CountAsync()).ToString());
        }

        /// <summary>
        /// The models that are part of a given release with the given name.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="releaseName">The name of the release for which the models are retrieved.</param>
        /// <param name="pageSize">The size of the page for which the models are retrieved. </param>
        /// <param name="pageIndex">The index of the page for which the models are retrieved.</param>
        /// <returns>The models that are part of the given release with the requested name as well as are on the requested page.</returns>
        [HttpGet("release/{releaseName}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TAPIModel>>> GetByRelease(string releaseName, int pageSize,
            int pageIndex)
        {
            var dbModels = await _readerWriter.GetByRelease(releaseName);

            var readModels = dbModels.Skip(pageSize * pageIndex)
                                                     .Take(pageSize).ToList().Select(ConvertDBModelToApiModel);

            return Json(readModels);
        }

        /// <summary>
        /// Calculates the amount of models that are part of the release with the given name.
        /// </summary>
        /// <param name="releaseName">The name of the release in question.</param>
        /// <returns>The amount of models that are part of the release.</returns>
        [HttpGet("release/count/{releaseName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByReleaseCount(string releaseName)
        {
            return Content((await (await _readerWriter.GetByRelease(releaseName)).CountAsync()).ToString());
        }

        /// <summary>
        /// The models that are part of the current version.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <param name="pageIndex">The 0-based index of the page to get.</param>
        /// <returns>The models that are part of the current version and  are on the requested page.</returns>
        [HttpGet("version/latest/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TAPIModel>>> GetByLatestVersion(int pageSize, int pageIndex)
        {
            var dbModels = await _readerWriter.GetByLatestVersion();

            var readModels = dbModels.Skip(pageSize * pageIndex)
                                                     .Take(pageSize).ToList().Select(ConvertDBModelToApiModel);

            return Json(readModels);
        }

        /// <summary>
        /// Counts the amount of that are contained in the latest version.
        /// </summary>
        /// <returns>The amount of that are contained in the latest version</returns>
        [HttpGet("version/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByLatestVersionCount()
        {
            return Content((await (await _readerWriter.GetByLatestVersion()).CountAsync()).ToString());
        }

        /// <summary>
        /// The models that are part of a given version based on the given id.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="versionId">The id of the version for which the models are being pulled.</param>
        /// <param name="pageSize">The size of the page that is being requested.</param>
        /// <param name="pageIndex">The 0-based index of the page that is being requested.</param>
        /// <returns>The models that are part of the given version and are on the requested page.</returns>
        [HttpGet("version/{versionId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TAPIModel>>> GetByVersion(Guid versionId, int pageSize,
            int pageIndex)
        {
            var dbModels = await _readerWriter.GetByVersion(versionId);

            var readModels = dbModels.Skip(pageSize * pageIndex)
                                                     .Take(pageSize).ToList().Select(ConvertDBModelToApiModel);

            return Json(readModels);
        }

        /// <summary>
        /// Counts the models that are part of a given version.
        /// </summary>
        /// <param name="versionId">The id of the version for which the models are being pulled.</param>
        /// <returns>The amount of models that are part of the given version.</returns>
        [HttpGet("version/count/{versionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByVersionCount(Guid versionId)
        {
            return Content((await (await _readerWriter.GetByVersion(versionId)).CountAsync()).ToString());
        }

        /// <summary>
        /// The models that are part of a given version with the given name.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="versionName">The name of the version for which the models are retrieved.</param>
        /// <param name="pageSize">The size of the page for which the models are retrieved. </param>
        /// <param name="pageIndex">The index of the page for which the models are retrieved.</param>
        /// <returns>The models that are part of the given version with the requested name as well as are on the requested page.</returns>
        [HttpGet("version/{versionName}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TAPIModel>>> GetByVersion(string versionName, int pageSize,
            int pageIndex)
        {
            var dbModels = await _readerWriter.GetByVersion(versionName);

            var readModels = dbModels.Skip(pageSize * pageIndex)
                                                     .Take(pageSize).ToList().Select(ConvertDBModelToApiModel);

            return Json(readModels);
        }

        /// <summary>
        /// Calculates the amount of models that are part of the version with the given name.
        /// </summary>
        /// <param name="versionName">The name of the version in question.</param>
        /// <returns>The amount of models that are part of the version.</returns>
        [HttpGet("version/count/{versionName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByVersionCount(string versionName)
        {
            return Content((await (await _readerWriter.GetByVersion(versionName)).CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the model of the corresponding name in the latest mapping, for the latest MC version.
        /// Returns 404 when no model is found.
        /// </summary>
        /// <param name="name">The name of the model in the current mapping as output.</param>
        /// <returns>The model with the name in question.</returns>
        [HttpGet("mapping/latest/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<TAPIModel>> GetByLatestMapping(string name)
        {
            var dbModels = await _readerWriter.GetByLatestMapping(name);

            if (dbModels == null)
                return NotFound();

            var readModels = ConvertDBModelToApiModel(dbModels);

            return Json(readModels);
        }

        /// <summary>
        /// Gets the model of the corresponding name in the latest mapping, for the given mc version.
        /// Returns 404 when no model in the target version is found.
        /// </summary>
        /// <param name="name">The name of the model in the current mapping as output.</param>
        /// <param name="versionId">The id of the game version in which needs to be looked.</param>
        /// <returns>The model with the name in question, in the requested version.</returns>
        [HttpGet("mapping/version/{name}/{versionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<TAPIModel>> GetByMappingInVersion(string name, Guid versionId)
        {
            var dbModels = await _readerWriter.GetByMappingInVersion(name, versionId);

            if (dbModels == null)
                return NotFound();

            var readModels = ConvertDBModelToApiModel(dbModels);

            return Json(readModels);
        }

        /// <summary>
        /// Gets the model of the corresponding name in the latest mapping, for the given release.
        /// Returns 404 when no model in the target version is found.
        /// </summary>
        /// <param name="name">The name of the model in the current mapping as output.</param>
        /// <param name="releaseId">The id of the release in which needs to be looked.</param>
        /// <returns>The model with the name in question, in the requested version.</returns>
        [HttpGet("mapping/release/{name}/{releaseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<TAPIModel>> GetByMappingInRelease(string name, Guid releaseId)
        {
            var dbModels = await _readerWriter.GetByMappingInRelease(name, releaseId);

            if (dbModels == null)
                return NotFound();

            var readModels = ConvertDBModelToApiModel(dbModels);

            return Json(readModels);
        }

        /// <summary>
        /// Method used to create a new proposal.
        /// </summary>
        /// <param name="proposalModel">The model for the proposal.</param>
        /// <returns>An http response code: 201-Created new proposal, 404-Unknown model, 401-Unauthorized user.</returns>
        [HttpPost("propose")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Propose([FromBody] CreateProposalModel proposalModel)
        {
            var versionedEntry = await _readerWriter.GetVersionedMapping(proposalModel.ProposedFor);
            if (versionedEntry == null)
                return NotFound(
                    $"Their is no mapping with a version entry with id: {proposalModel.ProposedFor}");

            var user = await _userResolvingService.Get();
            if (user == null)
                return Unauthorized();

            var initialVotedFor = new List<User> {user};
            var initialVotedAgainst = new List<User>();

            var proposalEntry = new TProposalEntry
            {
                VersionedMapping = versionedEntry,
                InputMapping = proposalModel.NewInput,
                OutputMapping = proposalModel.NewOutput,
                ProposedBy = user,
                ProposedOn = DateTime.Now,
                IsOpen = true,
                IsPublicVote = proposalModel.IsPublicVote,
                VotedFor = initialVotedFor,
                VotedAgainst = initialVotedAgainst,
                Comment = proposalModel.Comment,
                ClosedBy = null,
                ClosedOn = null,
                Merged = null,
                MergedWithId = null,
                MergedWith = null,
                CreatedOn = DateTime.Now
            };

            await this._readerWriter.AddProposal(proposalEntry);
            await this._readerWriter.SaveChanges();

            return CreatedAtAction("GetById", proposalEntry.VersionedMapping.Mapping.Id, proposalEntry);
        }

        /// <summary>
        /// Marks the current user as a person who voted for the proposal.
        /// </summary>
        /// <param name="proposalId">The id of the proposal for which is voted for.</param>
        /// <returns>An http response code: 200-Ok, 400-closed proposal, 401-Unauthorized user, 404-Unknown proposal.</returns>
        [HttpPost("proposal/vote/{proposalId}/for")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> VoteFor(Guid proposalId)
        {
            var currentProposal = await _readerWriter.GetProposal(proposalId);
            if (currentProposal == null)
                return NotFound("No proposal with the given id exists.");

            if (!currentProposal.IsOpen)
                return BadRequest("Proposal is not open");

            var user = await _userResolvingService.Get();
            if (user == null)
                return Unauthorized();

            if (!currentProposal.IsPublicVote && user.CanReview)
                return Unauthorized();

            if (currentProposal.VotedFor.Contains(user))
                return Conflict();

            currentProposal.VotedAgainst.Remove(user);
            currentProposal.VotedFor.Add(user);
            await _readerWriter.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Marks the current user as a person who voted against the proposal.
        /// </summary>
        /// <param name="proposalId">The id of the proposal for which is voted against.</param>
        /// <returns>An http response code: 200-Ok, 400-Closed proposal, 401-Unauthorized user, 404-Unknown proposal</returns>
        [HttpPost("proposal/vote/{proposalId}/against")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> VoteAgainst(Guid proposalId)
        {
            var currentProposal = await _readerWriter.GetProposal(proposalId);
            if (currentProposal == null)
                return NotFound("No proposal with the given id exists.");

            if (!currentProposal.IsOpen)
                return BadRequest("Proposal is not open");

            var user = await _userResolvingService.Get();
            if (user == null)
                return Unauthorized();

            if (!currentProposal.IsPublicVote && user.CanReview)
                return Unauthorized();

            if (currentProposal.VotedAgainst.Contains(user))
                return Conflict();

            currentProposal.VotedFor.Remove(user);
            currentProposal.VotedAgainst.Add(user);
            await _readerWriter.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Closes an open proposal.
        /// </summary>
        /// <param name="proposalId">The id of the proposal to close.</param>
        /// <param name="merge">True to merge a proposal as a committed mapping, false when not.</param>
        /// <returns>An http response code: 200-Ok proposal closed, 201-Created proposal merged, 400-Unknown or closed proposal, 401-Unauthorized user.</returns>
        [HttpPost("proposal/close/{proposalId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> Close(Guid proposalId, bool merge)
        {
            var currentProposal = await _readerWriter.GetProposal(proposalId);
            if (currentProposal == null)
                return NotFound("No proposal with the given id exists.");

            if (!currentProposal.IsOpen)
                return BadRequest("Proposal is not open");

            var user = await _userResolvingService.Get();
            if (user == null)
                return Unauthorized();

            if (!currentProposal.IsPublicVote && user.CanCommit)
                return Unauthorized();

            currentProposal.ClosedBy = user;
            currentProposal.ClosedOn = DateTime.Now;
            currentProposal.Merged = merge;

            if (merge)
            {
                var newCommittedMapping = new TCommittedEntry
                {
                    InputMapping = currentProposal.InputMapping,
                    OutputMapping = currentProposal.OutputMapping,
                    Proposal = currentProposal,
                    Releases = new List<TReleaseEntry>(),
                    VersionedMapping = currentProposal.VersionedMapping,
                    CreatedOn = DateTime.Now
                };

                currentProposal.VersionedMapping.CommittedMappings.Add(newCommittedMapping);
                await _readerWriter.SaveChanges();

                return CreatedAtAction("GetById", newCommittedMapping.VersionedMapping.Mapping.Id, newCommittedMapping);
            }

            await _readerWriter.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Gets the details on a committed mapping.
        /// </summary>
        /// <param name="committedMappingId">The id of the committed mapping.</param>
        /// <returns>The committed mapping with the id or 404.</returns>
        [HttpGet("committed/detailed/{committedMappingId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<DetailedMappingReadModel>> Detailed(Guid committedMappingId)
        {
            var committedMapping = await _readerWriter.GetCommittedEntry(committedMappingId);
            if (committedMapping == null)
                return NotFound("Unknown committed mapping with requested id.");

            return Json(new DetailedMappingReadModel()
            {
                Id = committedMappingId,
                In = committedMapping.InputMapping,
                Out = committedMapping.OutputMapping,
                Proposal = committedMapping.Proposal.Id,
                Releases = committedMapping.Releases.Select(release => release.Id),
                VersionedMapping = committedMapping.VersionedMapping.Id
            });
        }

        /// <summary>
        /// Creates a new model and its central mapping entry.
        /// Creates a new core mapping model, a versioned mapping model for the latest version, as well a single committed mapping.
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
        public abstract Task<ActionResult> AddToLatest([FromBody] TCreateReadModel mapping);

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
        public abstract Task<ActionResult> AddToVersion([FromBody] TCreateVersionedModel mapping);

        private IEnumerable<TAPIModel> ConvertDBModelToApiModel(IEnumerable<TMapping> mappings)
        {
            return mappings.Select(ConvertDBModelToApiModel);
        }

        protected abstract TAPIModel ConvertDBModelToApiModel(TMapping methodModel);

        protected abstract TVersionedReadModel ConvertVersionedDbModelToVersionedApiModel(
            TVersionedMapping versionedMapping);

        protected abstract MappingReadModel ConvertCommittedMappingToSimpleReadModel(
            TCommittedEntry committedMapping);

        protected abstract ProposalReadModel ConvertProposalMappingToReadModel(
            TProposalEntry proposalMapping);
    }
}
