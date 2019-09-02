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
    public class ClassesController : Controller
    {
        /// <summary>
        /// The classReaderOrWriter and as such also the reader for class mappings.
        /// </summary>
        private readonly IClassMappingWriter _classReaderOrWriter;

        /// <summary>
        /// The reader for game versions.
        /// </summary>
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
        public ClassesController(IClassMappingWriter classReaderOrWriter, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService)
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
        [HttpGet("id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
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
        [HttpGet("all/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
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
        [HttpGet("all/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> Count()
        {
            return Content((await (await _classReaderOrWriter.AsMappingQueryable()).CountAsync()).ToString());
        }

        /// <summary>
        /// The class models that are part of the current version.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <param name="pageIndex">The 0-based index of the page to get.</param>
        /// <returns>The class models that are part of the current release and are on the requested page.</returns>
        [HttpGet("release/latest/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<ClassReadModel>>> GetByLatestRelease(int pageSize, int pageIndex)
        {
            var dbModels = await _classReaderOrWriter.GetByLatestRelease();

            var readModels = dbModels.Skip(pageSize * pageIndex).Take(pageSize).ToList().Select(ConvertClassModelToReadModel);

            return Json(readModels);
        }

        /// <summary>
        /// Counts the amount of classes that are contained in the latest release.
        /// </summary>
        /// <returns>The amount of classes that are contained in the latest release</returns>
        [HttpGet("release/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByLatestReleaseCount()
        {
            return Content((await (await _classReaderOrWriter.GetByLatestRelease()).CountAsync()).ToString());
        }

        /// <summary>
        /// The class models that are part of a given release based on the given id.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="releaseId">The id of the release for which the classes are being pulled.</param>
        /// <param name="pageSize">The size of the page that is being requested.</param>
        /// <param name="pageIndex">The 0-based index of the page that is being requested.</param>
        /// <returns>The classes that are part of the given release and are on the requested page.</returns>
        [HttpGet("release/{releaseId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<ClassReadModel>>> GetByRelease(Guid releaseId, int pageSize, int pageIndex)
        {
            var dbModels = await _classReaderOrWriter.GetByRelease(releaseId);

            var readModels = dbModels.Skip(pageSize * pageIndex).Take(pageSize).ToList().Select(ConvertClassModelToReadModel);

            return Json(readModels);
        }

        /// <summary>
        /// Counts the class models that are part of a given release.
        /// </summary>
        /// <param name="releaseId">The id of the release for which the classes are being pulled.</param>
        /// <returns>The amount of classes that are part of the given release.</returns>
        [HttpGet("release/count/{releaseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByReleaseCount(Guid releaseId)
        {
            return Content((await (await _classReaderOrWriter.GetByRelease(releaseId)).CountAsync()).ToString());
        }

        /// <summary>
        /// The class models that are part of a given release with the given name.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="releaseName">The name of the release for which the class models are retrieved.</param>
        /// <param name="pageSize">The size of the page for which the class models are retrieved. </param>
        /// <param name="pageIndex">The index of the page for which the class models are retrieved.</param>
        /// <returns>The classes that are part of the given release with the requested name as well as are on the requested page.</returns>
        [HttpGet("release/{releaseName}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<ClassReadModel>>> GetByRelease(string releaseName, int pageSize, int pageIndex)
        {
            var dbModels = await _classReaderOrWriter.GetByRelease(releaseName);

            var readModels = dbModels.Skip(pageSize * pageIndex).Take(pageSize).ToList().Select(ConvertClassModelToReadModel);

            return Json(readModels);
        }

        /// <summary>
        /// Calculates the amount of classes that are part of the release with the given name.
        /// </summary>
        /// <param name="releaseName">The name of the release in question.</param>
        /// <returns>The amount of classes that are part of the release.</returns>
        [HttpGet("release/count/{releaseName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByReleaseCount(string releaseName)
        {
            return Content((await (await _classReaderOrWriter.GetByRelease(releaseName)).CountAsync()).ToString());
        }

        /// <summary>
        /// The class models that are part of the current version.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <param name="pageIndex">The 0-based index of the page to get.</param>
        /// <returns>The class models that are part of the current version and are on the requested page.</returns>
        [HttpGet("version/latest/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<ClassReadModel>>> GetByLatestVersion(int pageSize, int pageIndex)
        {
            var dbModels = await _classReaderOrWriter.GetByLatestVersion();

            var readModels = dbModels.Skip(pageSize * pageIndex).Take(pageSize).ToList()
                .Select(ConvertClassModelToReadModel);

            return Json(readModels);
        }

        /// <summary>
        /// Counts the amount of classes that are contained in the latest version.
        /// </summary>
        /// <returns>The amount of classes that are contained in the latest version</returns>
        [HttpGet("version/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByLatestVersionCount()
        {
            return Content((await (await _classReaderOrWriter.GetByLatestVersion()).CountAsync()).ToString());
        }

        /// <summary>
        /// The class models that are part of a given version based on the given id.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="versionId">The id of the version for which the classes are being pulled.</param>
        /// <param name="pageSize">The size of the page that is being requested.</param>
        /// <param name="pageIndex">The 0-based index of the page that is being requested.</param>
        /// <returns>The classes that are part of the given version and are on the requested page.</returns>
        [HttpGet("version/{versionId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<ClassReadModel>>> GetByVersion(Guid versionId, int pageSize, int pageIndex)
        {
            var dbModels = await _classReaderOrWriter.GetByVersion(versionId);

            var readModels = dbModels.Skip(pageSize * pageIndex).Take(pageSize).ToList().Select(ConvertClassModelToReadModel);

            return Json(readModels);
        }

        /// <summary>
        /// Counts the class models that are part of a given version.
        /// </summary>
        /// <param name="versionId">The id of the version for which the classes are being pulled.</param>
        /// <returns>The amount of classes that are part of the given version.</returns>
        [HttpGet("version/count/{versionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByVersionCount(Guid versionId)
        {
            return Content((await (await _classReaderOrWriter.GetByVersion(versionId)).CountAsync()).ToString());
        }

        /// <summary>
        /// The class models that are part of a given version with the given name.
        /// Returns a paginated result.
        /// </summary>
        /// <param name="versionName">The name of the version for which the class models are retrieved.</param>
        /// <param name="pageSize">The size of the page for which the class models are retrieved. </param>
        /// <param name="pageIndex">The index of the page for which the class models are retrieved.</param>
        /// <returns>The classes that are part of the given version with the requested name as well as are on the requested page.</returns>
        [HttpGet("version/{versionName}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<ClassReadModel>>> GetByVersion(string versionName, int pageSize, int pageIndex)
        {
            var dbModels = await _classReaderOrWriter.GetByVersion(versionName);

            var readModels = dbModels.Skip(pageSize * pageIndex).Take(pageSize).ToList().Select(ConvertClassModelToReadModel);

            return Json(readModels);
        }

        /// <summary>
        /// Calculates the amount of classes that are part of the version with the given name.
        /// </summary>
        /// <param name="versionName">The name of the version in question.</param>
        /// <returns>The amount of classes that are part of the version.</returns>
        [HttpGet("version/count/{versionName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetByVersionCount(string versionName)
        {
            return Content((await (await _classReaderOrWriter.GetByVersion(versionName)).CountAsync()).ToString());
        }

        /// <summary>
        /// Gets the class of the corresponding name in the latest mapping, for the latest MC version.
        /// Returns 404 when no class is found.
        /// </summary>
        /// <param name="name">The name of the class in the current mapping as output. In the form: net.package.com.OutClassName#InnerClassName#TargetClassNameAsMostInnerClass</param>
        /// <returns>The class model of the class with the name in question.</returns>
        [HttpGet("mapping/latest/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
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
        [HttpGet("mapping/version/{name}/{versionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
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
        [HttpGet("mapping/release/{name}/{releaseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
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
        [HttpPost("propose")]
        [Consumes("application/json")]
        [Produces("application/json")]
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
                CreatedOn = DateTime.Now,
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
        [HttpPost("proposal/vote/{proposalId}/for")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
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
        [HttpPost("proposal/vote/{proposalId}/against")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
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
                    Releases = new List<ClassReleaseMember>(),
                    VersionedMapping = currentProposal.VersionedMapping,
                    CreatedOn = DateTime.Now
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
        [HttpGet("committed/detailed/{committedMappingId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
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

        /// <summary>
        /// Creates a new class and its central mapping entry.
        /// Creates a new core mapping class, a versioned mapping class for the latest version, as well a single committed mapping.
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
        public async Task<ActionResult> AddToLatest([FromBody] CreateClassModel mapping)
        {
            var currentLatestGameVersion = await _gameVersionReader.GetLatest();
            if (currentLatestGameVersion == null)
                return BadRequest("No game version has been registered yet.");

            var user = await _userResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            ClassVersionedMapping outer = null;
            if (mapping.Outer.HasValue)
            {
                outer = await _classReaderOrWriter.GetVersionedMapping(mapping.Outer.Value);
                if (outer == null)
                    return BadRequest("Unknown outer class");
            }

            var inheritsFrom =
                (await Task.WhenAll(mapping.InheritsFrom.Select(async id =>
                    await _classReaderOrWriter.GetVersionedMapping(id)))).ToList();

            if (inheritsFrom.Any(m => m == null))
                return BadRequest("Unknown inheriting class.");

            var versionedClassMapping = new ClassVersionedMapping
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                GameVersion = currentLatestGameVersion,
                Outer=outer,
                Package = mapping.Package,
                InheritsFrom = inheritsFrom,
                CommittedMappings = new List<ClassCommittedMappingEntry>(),
                ProposalMappings = new List<ClassProposalMappingEntry>()
            };

            var initialCommittedMappingEntry = new ClassCommittedMappingEntry
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<ClassReleaseMember>(),
                VersionedMapping = versionedClassMapping,
                CreatedOn = DateTime.Now
            };

            versionedClassMapping.CommittedMappings.Add(initialCommittedMappingEntry);

            var classMapping = new ClassMapping
            {
                Id = Guid.NewGuid(),
                VersionedMappings = new List<ClassVersionedMapping>() {versionedClassMapping}
            };

            await _classReaderOrWriter.Add(classMapping);
            await _classReaderOrWriter.SaveChanges();

            return CreatedAtAction("GetById", classMapping.Id, classMapping);
        }

        /// <summary>
        /// Creates a new versioned class entry for an already existing class mapping.
        /// Creates a versioned mapping class for the given version, as well a single committed mapping.
        /// </summary>
        /// <param name="mapping">The versioned mapping to create.</param>
        /// <returns>An http response code:201-New mapping created,400-The request was invalid,404-Certain data could not be found,401-Unauthorized,409-A versioned class for that version already exists with the class.</returns>
        [HttpPost("add/version/{versionId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> AddToVersion([FromBody] CreateVersionedClassModel mapping)
        {
            var currentGameVersion = await _gameVersionReader.GetById(mapping.GameVersion);
            if (currentGameVersion == null)
                return BadRequest("No game version with that id has been registered yet.");

            var user = await _userResolvingService.Get();
            if (user == null || !user.CanCommit)
                return Unauthorized();

            ClassVersionedMapping outer = null;
            if (mapping.Outer.HasValue)
            {
                outer = await _classReaderOrWriter.GetVersionedMapping(mapping.Outer.Value);
                if (outer == null)
                    return BadRequest("Unknown outer class");
            }

            var classMapping = await _classReaderOrWriter.GetById(mapping.VersionedMappingFor);
            if (classMapping == null)
                return BadRequest("Unknown class mapping to create the versioned mapping for.");

            if (classMapping.VersionedMappings.Any(versionedMapping =>
                versionedMapping.GameVersion.Id == mapping.GameVersion))
                return Conflict();

            var inheritsFrom =
                (await Task.WhenAll(mapping.InheritsFrom.Select(async id =>
                    await _classReaderOrWriter.GetVersionedMapping(id)))).ToList();

            if (inheritsFrom.Any(m => m == null))
                return BadRequest("Unknown inheriting class.");

            var versionedClassMapping = new ClassVersionedMapping
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                GameVersion = currentGameVersion,
                Outer=outer,
                Package = mapping.Package,
                InheritsFrom = inheritsFrom,
                CommittedMappings = new List<ClassCommittedMappingEntry>(),
                ProposalMappings = new List<ClassProposalMappingEntry>()
            };

            var initialCommittedMappingEntry = new ClassCommittedMappingEntry
            {
                Documentation = mapping.Documentation,
                InputMapping = mapping.In,
                OutputMapping = mapping.Out,
                Proposal = null,
                Releases = new List<ClassReleaseMember>(),
                VersionedMapping = versionedClassMapping,
                CreatedOn = DateTime.Now
            };

            versionedClassMapping.CommittedMappings.Add(initialCommittedMappingEntry);

            await _classReaderOrWriter.SaveChanges();

            return CreatedAtAction("GetById", classMapping.Id, classMapping);
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
            return new MappingReadModel {Id = committedMapping.Id, In = committedMapping.InputMapping, Out = committedMapping.OutputMapping, Documentation = committedMapping.Documentation};
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
                Out = proposalMapping.OutputMapping,
                Documentation = proposalMapping.Documentation
            };
        }
    }
}
