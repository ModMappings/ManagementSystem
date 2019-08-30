using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model.Creation.Class;
using API.Model.Creation.Core;
using API.Model.Read.Class;
using API.Model.Read.Core;
using API.Services.Core;
using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Readers.Core;
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
        /// The classReaderOrWriter and as such also the reader for class mappings.
        /// </summary>
        private readonly IClassMappingWriter _classReaderOrWriter;

        private readonly IGameVersionReader _gameVersionReader;

        /// <summary>
        /// The user resolving service.
        /// </summary>
        private readonly IUserResolvingService _userResolvingService;

        /// <summary>
        /// Creates a new controller.
        /// Called via DI.
        /// </summary>
        /// <param name="classReaderOrWriter">The classReaderOrWriter for class mappings.</param>
        /// <param name="gameVersionReader">The reader for game versions.</param>
        /// <param name="userResolvingService">The service used to resolve the user.</param>
        public ClassController(IClassMappingWriter classReaderOrWriter, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService)
        {
            _classReaderOrWriter = classReaderOrWriter;
            _userResolvingService = userResolvingService;
            _gameVersionReader = gameVersionReader;
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
            var dbModel = await _classReaderOrWriter.GetById(id);

            if (dbModel == null)
                return NotFound();

            var readModel = ConvertClassModelToReadModel(dbModel);

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
            var dbModels = (await _classReaderOrWriter.AsMappingQueryable()).Skip(pageSize * pageIndex).Take(pageSize);

            var readModels = dbModels.ToList().Select(ConvertClassModelToReadModel);

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
            return Ok(await (await _classReaderOrWriter.AsMappingQueryable()).CountAsync());
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
            var dbModels = await _classReaderOrWriter.GetByLatestRelease();

            var readModels = dbModels.ToList().Select(ConvertClassModelToReadModel).Skip(pageSize * pageIndex).Take(pageIndex);

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
            return Ok(await (await _classReaderOrWriter.GetByLatestRelease()).CountAsync());
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
            var dbModels = await _classReaderOrWriter.GetByRelease(releaseId);

            var readModels = dbModels.ToList().Select(ConvertClassModelToReadModel).Skip(pageSize * pageIndex).Take(pageIndex);

            return Json(readModels);
        }

        /// <summary>
        /// Counts the class models that are part of a given release.
        /// </summary>
        /// <param name="releaseId">The id of the release for which the classes are being pulled.</param>
        /// <returns>The amount of classes that are part of the given release.</returns>
        [HttpGet("release/count/{releaseId}", Name = "CountOfReleaseById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetByReleaseCount(Guid releaseId)
        {
            return Ok(await (await _classReaderOrWriter.GetByRelease(releaseId)).CountAsync());
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
            var dbModels = await _classReaderOrWriter.GetByRelease(releaseName);

            var readModels = dbModels.ToList().Select(ConvertClassModelToReadModel).Skip(pageSize * pageIndex).Take(pageIndex);

            return Json(readModels);
        }

        /// <summary>
        /// Calculates the amount of classes that are part of the release with the given name.
        /// </summary>
        /// <param name="releaseName">The name of the release in question.</param>
        /// <returns>The amount of classes that are part of the release.</returns>
        [HttpGet("release/count/{releaseName}", Name = "CountOfReleaseByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetByReleaseCount(string releaseName)
        {
            return Ok(await (await _classReaderOrWriter.GetByRelease(releaseName)).CountAsync());
        }

        /// <summary>
        /// Gets the class of the corresponding name in the latest mapping, for the latest MC version.
        /// Returns 404 when no class is found.
        /// </summary>
        /// <param name="name">The name of the class in the current mapping as output. In the form: net.package.com.OutClassName#InnerClassName#TargetClassNameAsMostInnerClass</param>
        /// <returns>The class model of the class with the name in question.</returns>
        [HttpGet("byMapping/latest/{name}", Name = "ByLatestMapping")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassReadModel>> GetByLatestMapping(string name)
        {
            var dbModels = await _classReaderOrWriter.GetByLatestMapping(name);

            if (dbModels == null)
                return NotFound();

            var readModels = ConvertClassModelToReadModel(dbModels);

            return Json(readModels);
        }

        /// <summary>
        /// Gets the class of the corresponding name in the latest mapping, for the given mc version.
        /// Returns 404 when no class in the target version is found.
        /// </summary>
        /// <param name="name">The name of the class in the current mapping as output. In the form: net.package.com.OutClassName#InnerClassName#TargetClassNameAsMostInnerClass</param>
        /// <param name="versionId">The id of the game version in which needs to be looked.</param>
        /// <returns>The class model of the class with the name in question, in the requested version.</returns>
        [HttpGet("mapping/version/{name}/{versionId}", Name = "ByMappingInVersionFromId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassReadModel>> GetByMappingInVersion(string name, Guid versionId)
        {
            var dbModels = await _classReaderOrWriter.GetByMappingInVersion(name, versionId);

            if (dbModels == null)
                return NotFound();

            var readModels = ConvertClassModelToReadModel(dbModels);

            return Json(readModels);
        }

        /// <summary>
        /// Gets the class of the corresponding name in the latest mapping, for the given release.
        /// Returns 404 when no class in the target version is found.
        /// </summary>
        /// <param name="name">The name of the class in the current mapping as output. In the form: net.package.com.OutClassName#InnerClassName#TargetClassNameAsMostInnerClass</param>
        /// <param name="releaseId">The id of the release in which needs to be looked.</param>
        /// <returns>The class model of the class with the name in question, in the requested version.</returns>
        [HttpGet("mapping/release/{name}/{releaseId}", Name = "ByMappingInReleaseFromId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClassReadModel>> GetByMappingInRelease(string name, Guid releaseId)
        {
            var dbModels = await _classReaderOrWriter.GetByMappingInRelease(name, releaseId);

            if (dbModels == null)
                return NotFound();

            var readModels = ConvertClassModelToReadModel(dbModels);

            return Json(readModels);
        }

        /// <summary>
        /// Method used to create a new proposal.
        /// </summary>
        /// <param name="proposalModel">The model for the proposal.</param>
        /// <returns>An http response code: 201-Created new proposal, 404-Unknown class, 401-Unauthorized user.</returns>
        [HttpPost("propose", Name = "Propose")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Propose([FromBody] CreateProposalModel proposalModel)
        {
            var classVersionedEntry = await _classReaderOrWriter.GetVersionedMapping(proposalModel.ProposedFor);
            if (classVersionedEntry == null)
                return NotFound(
                    $"Their is no class mapping with a version entry with id: {proposalModel.ProposedFor}");

            var user = await _userResolvingService.Get();
            if (user == null)
                return Unauthorized();

            var initialVotedFor = new List<User> {user};
            var initialVotedAgainst = new List<User>();

            var proposalEntry = new ClassProposalMappingEntry
            {
                VersionedMapping = classVersionedEntry,
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
                MergedWith = null
            };

            await this._classReaderOrWriter.AddProposal(proposalEntry);
            await this._classReaderOrWriter.SaveChanges();

            return CreatedAtAction("GetById", proposalEntry.VersionedMapping.Mapping.Id, proposalEntry);
        }

        /// <summary>
        /// Marks the current user as a person who voted for the proposal.
        /// </summary>
        /// <param name="proposalId">The id of the proposal for which is voted for.</param>
        /// <returns>An http response code: 200-Ok, 400-closed proposal, 401-Unauthorized user, 404-Unknown proposal.</returns>
        [HttpPost("proposal/vote/{proposalId}/for", Name = "VoteFor")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> VoteFor(Guid proposalId)
        {
            var currentProposal = await _classReaderOrWriter.GetProposal(proposalId);
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
            await _classReaderOrWriter.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Marks the current user as a person who voted against the proposal.
        /// </summary>
        /// <param name="proposalId">The id of the proposal for which is voted against.</param>
        /// <returns>An http response code: 200-Ok, 400-Closed proposal, 401-Unauthorized user, 404-Unknown proposal</returns>
        [HttpPost("proposal/vote/{proposalId}/against", Name = "VoteAgainst")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> VoteAgainst(Guid proposalId)
        {
            var currentProposal = await _classReaderOrWriter.GetProposal(proposalId);
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
            await _classReaderOrWriter.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Closes an open proposal.
        /// </summary>
        /// <param name="proposalId">The id of the proposal to close.</param>
        /// <param name="merge">True to merge a proposal as a committed mapping, false when not.</param>
        /// <returns>An http response code: 200-Ok proposal closed, 201-Created proposal merged, 400-Unknown or closed proposal, 401-Unauthorized user.</returns>
        [HttpPost("proposal/close/{proposalId}", Name = "MergeOrClose")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Close(Guid proposalId, bool merge)
        {
            var currentProposal = await _classReaderOrWriter.GetProposal(proposalId);
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
                var newCommittedMapping = new ClassCommittedMappingEntry
                {
                    InputMapping = currentProposal.InputMapping,
                    OutputMapping = currentProposal.OutputMapping,
                    Proposal = currentProposal,
                    Releases = new List<Release>(),
                    VersionedMapping = currentProposal.VersionedMapping
                };

                currentProposal.VersionedMapping.CommittedMappings.Add(newCommittedMapping);
                await _classReaderOrWriter.SaveChanges();

                return CreatedAtAction("GetById", newCommittedMapping.VersionedMapping.Mapping.Id, newCommittedMapping);
            }

            await _classReaderOrWriter.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Gets the details on a committed mapping.
        /// </summary>
        /// <param name="committedMappingId">The id of the committed mapping.</param>
        /// <returns>The committed mapping with the id or 404.</returns>
        [HttpGet("committed/detailed/{committedMappingId}", Name = "GetDetailedCommittedMapping")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ClassDetailedMappingReadModel>> Detailed(Guid committedMappingId)
        {
            var committedMapping = await _classReaderOrWriter.GetCommittedEntry(committedMappingId);
            if (committedMapping == null)
                return NotFound("Unknown committed mapping with requested id.");

            return Json(new ClassDetailedMappingReadModel
            {
                Id = committedMappingId,
                In = committedMapping.InputMapping,
                Out = committedMapping.OutputMapping,
                Proposal = committedMapping.Proposal.Id,
                Releases = committedMapping.Releases.Select(release => release.Id),
                VersionedMapping = committedMapping.VersionedMapping.Id
            });
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Add([FromBody] CreateClassModel mapping)
        {
            var currentRelease = await _gameVersionReader.GetLatest();
            if (currentRelease == null)
                return BadRequest();

            var user = await _userResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            //TODO: Add logic, creation model needs to be adapted.
            //TODO: Add logic to create/update a created class.
            return null;
        }

        [HttpPost("add/all")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status206PartialContent)]
        public async Task<ActionResult> AddAll([FromBody] IEnumerable<CreateClassModel> mapping)
        {
            //TODO: User auth and handling logic.
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
