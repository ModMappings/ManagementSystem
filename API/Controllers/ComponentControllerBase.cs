using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model.Creation.Core;
using API.Model.Read.Core;
using API.Services.Core;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Readers.Core;
using Data.Core.Writers.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public abstract class ComponentControllerBase<TReadModel, TVersionedReadModel> : Controller
        where TReadModel : AbstractReadModel<TVersionedReadModel>
        where TVersionedReadModel : AbstractVersionedReadModel
    {

        protected readonly IComponentWriter ComponentWriter;

        protected readonly IReleaseReader ReleaseReader;

        protected readonly IGameVersionReader GameVersionReader;

        protected readonly IUserResolvingService UserResolvingService;

        protected ComponentControllerBase(IComponentWriter componentWriter, IReleaseReader releaseReader, IGameVersionReader gameVersionReader, IUserResolvingService userResolvingService)
        {
            ComponentWriter = componentWriter;
            ReleaseReader = releaseReader;
            GameVersionReader = gameVersionReader;
            UserResolvingService = userResolvingService;
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
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The components in the store on the requested page.</returns>
        [HttpGet("all/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TReadModel>>> AsQueryable(int pageSize, int pageIndex)
        {
            var dbModels = await ComponentWriter.AsQueryable();

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Returns the count of all components stored in the database.
        /// </summary>
        /// <returns>The total count of components stored in the database.</returns>
        [HttpGet("all/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<IQueryable<TReadModel>>> Count()
        {
            var dbModels = await ComponentWriter.AsQueryable();

            return Content(dbModels.Count().ToString());
        }

        /// <summary>
        /// Returns all components, stored in the database, that are part of the latest release.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>All components, stored in the database, that are part of the latest release.</returns>
        [HttpGet("release/latest/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TReadModel>>> GetByLatestRelease(int pageSize, int pageIndex)
        {
            var latestRelease = await ReleaseReader.GetLatest();

            return await GetByRelease(latestRelease.Id, pageSize, pageIndex);
        }

        /// <summary>
        /// Returns the count of all components, stored in the database, that are part of the latest release.
        /// </summary>
        /// <returns>The count of all components, stored in the database, that are part of the latest release.</returns>
        [HttpGet("release/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<IQueryable<TReadModel>>> CountByLatestRelease()
        {
            var latestRelease = await ReleaseReader.GetLatest();

            return await CountByRelease(latestRelease.Id);
        }

        /// <summary>
        /// Returns all components, stored in the database, that are part of the requested release.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="releaseId">The id of the release in question.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>All components, stored in the database, that are part of the latest release.</returns>
        [HttpGet("release/{releaseId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TReadModel>>> GetByRelease(Guid releaseId, int pageSize, int pageIndex)
        {
            var dbModels = await ComponentWriter.GetByRelease(releaseId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Returns the count of all components, stored in the database, that are part of the given release.
        /// </summary>
        /// <param name="releaseId">The id of the release in question</param>
        /// <returns>The count of all components, stored in the database, that are part of the given release.</returns>
        [HttpGet("release/{releaseId}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<IQueryable<TReadModel>>> CountByRelease(Guid releaseId)
        {
            var dbModels = await ComponentWriter.GetByRelease(releaseId);

            return Content(dbModels.Count().ToString());
        }

        /// <summary>
        /// Returns all components, stored in the database, that are part of the requested release.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="releaseName">The name of the release in question.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>All components, stored in the database, that are part of the latest release.</returns>
        [HttpGet("release/{releaseName}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TReadModel>>> GetByRelease(string releaseName, int pageSize, int pageIndex)
        {
            var latestRelease = await ReleaseReader.GetByName(releaseName);

            return await GetByRelease(latestRelease.Id, pageSize, pageIndex);
        }

        /// <summary>
        /// Returns the count of all components, stored in the database, that are part of the given release.
        /// </summary>
        /// <param name="releaseName">The name of the release in question</param>
        /// <returns>The count of all components, stored in the database, that are part of the given release.</returns>
        [HttpGet("release/{releaseName}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<IQueryable<TReadModel>>> CountByRelease(string releaseName)
        {
            var dbModels = await ComponentWriter.GetByRelease(releaseName);

            return Content(dbModels.Count().ToString());
        }

        /// <summary>
        /// Returns all components, stored in the database, that are part of the latest version.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>All components, stored in the database, that are part of the latest version.</returns>
        [HttpGet("version/latest/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TReadModel>>> GetByLatestVersion(int pageSize, int pageIndex)
        {
            var latestVersion = await GameVersionReader.GetLatest();

            return await GetByVersion(latestVersion.Id, pageSize, pageIndex);
        }

        /// <summary>
        /// Returns the count of all components, stored in the database, that are part of the latest version.
        /// </summary>
        /// <returns>The count of all components, stored in the database, that are part of the latest version.</returns>
        [HttpGet("version/latest/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<IQueryable<TReadModel>>> CountByLatestVersion()
        {
            var latestVersion = await GameVersionReader.GetLatest();

            return await CountByVersion(latestVersion.Id);
        }

        /// <summary>
        /// Returns all components, stored in the database, that are part of the requested version.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="versionId">The id of the version in question.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>All components, stored in the database, that are part of the latest version.</returns>
        [HttpGet("version/{versionId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TReadModel>>> GetByVersion(Guid versionId, int pageSize, int pageIndex)
        {
            var dbModels = await ComponentWriter.GetByVersion(versionId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Returns the count of all components, stored in the database, that are part of the given version.
        /// </summary>
        /// <param name="versionId">The id of the version in question</param>
        /// <returns>The count of all components, stored in the database, that are part of the given version.</returns>
        [HttpGet("version/{versionId}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<IQueryable<TReadModel>>> CountByVersion(Guid versionId)
        {
            var dbModels = await ComponentWriter.GetByVersion(versionId);

            return Content(dbModels.Count().ToString());
        }

        /// <summary>
        /// Returns all components, stored in the database, that are part of the requested version.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="versionName">The name of the version in question.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>All components, stored in the database, that are part of the latest version.</returns>
        [HttpGet("version/{versionName}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TReadModel>>> GetByVersion(string versionName, int pageSize, int pageIndex)
        {
            var latestVersion = await GameVersionReader.GetByName(versionName);

            return await GetByVersion(latestVersion.Id, pageSize, pageIndex);
        }

        /// <summary>
        /// Returns the count of all components, stored in the database, that are part of the given version.
        /// </summary>
        /// <param name="versionName">The name of the version in question</param>
        /// <returns>The count of all components, stored in the database, that are part of the given version.</returns>
        [HttpGet("version/{versionName}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<IQueryable<TReadModel>>> CountByVersion(string versionName)
        {
            var dbModels = await ComponentWriter.GetByVersion(versionName);

            return Content(dbModels.Count().ToString());
        }

        /// <summary>
        /// Gets all the components with a given name in its latest mappings.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="name">The name to look for.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The components with a matching mapping.</returns>
        [HttpGet("mapping/latest/{name}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TReadModel>>> GetByLatestMapping(string name, int pageSize, int pageIndex)
        {
            var dbModels = await ComponentWriter.GetByLatestMapping(name);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Calculates the count of all the components with a given name in the latest mapping.
        /// </summary>
        /// <param name="name">The name to look for.</param>
        /// <returns>The count of all components, stored in the database, with a given name in the mapping.</returns>
        [HttpGet("mapping/latest/{name}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<IQueryable<TReadModel>>> CountByLatestMapping(string name)
        {
            var dbModels = await ComponentWriter.GetByLatestMapping(name);

            return Content(dbModels.Count().ToString());
        }

        /// <summary>
        /// Gets all the components with a given name in its latest mappings within the given version.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="name">The name to look for.</param>
        /// <param name="versionId">The id of the version to look in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The components with a matching mapping in the given version.</returns>
        [HttpGet("mapping/version/{versionId}/{name}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TReadModel>>> GetByMappingInVersion(string name, Guid versionId, int pageSize, int pageIndex)
        {
            var dbModels = await ComponentWriter.GetByMappingInVersion(name, versionId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Calculates the count of components with a given name in its latest mappings within the given version.
        /// </summary>
        /// <param name="name">The name to look for.</param>
        /// <param name="versionId">The id of the version to look in.</param>
        /// <returns>The count of all components with a matching mapping in the given version.</returns>
        [HttpGet("mapping/version/{versionId}/{name}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<IQueryable<TReadModel>>> CountByMappingInVersion(string name, Guid versionId)
        {
            var dbModels = await ComponentWriter.GetByMappingInVersion(name, versionId);

            return Content(dbModels.Count().ToString());
        }

        /// <summary>
        /// Gets all the components with a given name in its latest mappings within the given release.
        /// </summary>
        /// <remarks>Has pagination support via request route parameters.</remarks>
        /// <param name="name">The name to look for.</param>
        /// <param name="releaseId">The id of the release to look in.</param>
        /// <param name="pageSize">The size of a single page in the pagination.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <returns>The components with a matching mapping in the given release.</returns>
        [HttpGet("mapping/release/{releaseId}/{name}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<TReadModel>>> GetByMappingInRelease(string name, Guid releaseId, int pageSize, int pageIndex)
        {
            var dbModels = await ComponentWriter.GetByMappingInRelease(name, releaseId);

            return Json(dbModels.Skip(pageSize * pageIndex).Take(pageSize).Select(ConvertDbModelToReadModel));
        }

        /// <summary>
        /// Calculates the count of components with a given name in its latest mappings within the given release.
        /// </summary>
        /// <param name="name">The name to look for.</param>
        /// <param name="releaseId">The id of the release to look in.</param>
        /// <returns>The count of all components with a matching mapping in the given release.</returns>
        [HttpGet("mapping/release/{releaseId}/{name}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<IQueryable<TReadModel>>> CountByMappingInRelease(string name, Guid releaseId)
        {
            var dbModels = await ComponentWriter.GetByMappingInRelease(name, releaseId);

            return Content(dbModels.Count().ToString());
        }

        /// <summary>
        /// Gets the versioned component with the given id.
        /// </summary>
        /// <param name="id">The id of the versioned component you are looking for.</param>
        /// <returns>200-The versioned component with the given id. 404-When no versioned component exists with the given id.</returns>
        [HttpGet("versioned/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<TVersionedReadModel>> GetVersionedMapping(Guid id)
        {
            var versionComponent = await ComponentWriter.GetVersionedMapping(id);

            if (versionComponent == null)
                return NotFound();

            return Json(ConvertVersionedDbModelToReadModel(versionComponent));
        }

        /// <summary>
        /// Gets the proposal with the given id.
        /// </summary>
        /// <param name="id">The id of the proposal you are looking for.</param>
        /// <returns>200-The proposal with the given id. 404-When no proposal exists with the given id.</returns>
        [HttpGet("mappings/proposal/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<ProposalMappingEntry>> GetProposalMapping(Guid id)
        {
            var proposalMappingEntry = await ComponentWriter.GetProposalMapping(id);

            if (proposalMappingEntry == null)
                return NotFound();

            return Json(ConvertProposalDbModelToProposalReadModel(proposalMappingEntry));
        }

        /// <summary>
        /// Gets the live mapping with the given id.
        /// </summary>
        /// <param name="id">The id of the live mapping you are looking for.</param>
        /// <returns>200-The live mapping with the given id. 404-When no live mapping exists with the given id.</returns>
        [HttpGet("mappings/live/detailed/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<DetailedMappingReadModel>> GetLiveMapping(Guid id)
        {
            var liveMappingEntry = await ComponentWriter.GetLiveMapping(id);

            if (liveMappingEntry == null)
                return NotFound();

            return Json(ConvertLiveDbModelToDetailedMappingReadModel(liveMappingEntry));
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
            var classVersionedEntry = await ComponentWriter.GetVersionedMapping(proposalModel.ProposedFor);
            if (classVersionedEntry == null)
                return NotFound(
                    $"Their is no component with a version component with id: {proposalModel.ProposedFor}");

            var user = await UserResolvingService.Get();
            if (user == null)
                return Unauthorized();

            var initialVotedFor = new List<Guid> {user.Id};
            var initialVotedAgainst = new List<Guid>();

            var proposalEntry = new ProposalMappingEntry()
            {
                Mapping = classVersionedEntry,
                InputMapping = proposalModel.NewInput,
                OutputMapping = proposalModel.NewOutput,
                ProposedBy = user.Id,
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
                WentLiveWith = null,
                WentLiveWithId = null
            };

            classVersionedEntry.Proposals.Add(proposalEntry);

            await this.ComponentWriter.Update(classVersionedEntry);
            await this.ComponentWriter.SaveChanges();

            return CreatedAtAction("GetById", proposalEntry.Mapping.Component.Id, proposalEntry);
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
            var currentProposal = await ComponentWriter.GetProposalMapping(proposalId);
            if (currentProposal == null)
                return NotFound("No proposal with the given id exists.");

            if (!currentProposal.IsOpen)
                return BadRequest("Proposal is not open");

            var user = await UserResolvingService.Get();
            if (user == null)
                return Unauthorized();

            if (!currentProposal.IsPublicVote && user.CanReview)
                return Unauthorized();

            if (currentProposal.VotedFor.Contains(user.Id))
                return Conflict();

            currentProposal.VotedAgainst.Remove(user.Id);
            currentProposal.VotedFor.Add(user.Id);
            await ComponentWriter.SaveChanges();

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
            var currentProposal = await ComponentWriter.GetProposalMapping(proposalId);
            if (currentProposal == null)
                return NotFound("No proposal with the given id exists.");

            if (!currentProposal.IsOpen)
                return BadRequest("Proposal is not open");

            var user = await UserResolvingService.Get();
            if (user == null)
                return Unauthorized();

            if (!currentProposal.IsPublicVote && user.CanReview)
                return Unauthorized();

            if (currentProposal.VotedAgainst.Contains(user.Id))
                return Conflict();

            currentProposal.VotedFor.Remove(user.Id);
            currentProposal.VotedAgainst.Add(user.Id);
            await ComponentWriter.SaveChanges();

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
            var currentProposal = await ComponentWriter.GetProposalMapping(proposalId);
            if (currentProposal == null)
                return NotFound("No proposal with the given id exists.");

            if (!currentProposal.IsOpen)
                return BadRequest("Proposal is not open");

            var user = await UserResolvingService.Get();
            if (user == null)
                return Unauthorized();

            if (!currentProposal.IsPublicVote && user.CanCommit)
                return Unauthorized();

            currentProposal.ClosedBy = user.Id;
            currentProposal.ClosedOn = DateTime.Now;
            currentProposal.Merged = merge;

            if (merge)
            {
                var newCommittedMapping = new LiveMappingEntry()
                {
                    InputMapping = currentProposal.InputMapping,
                    OutputMapping = currentProposal.OutputMapping,
                    Proposal = currentProposal,
                    Releases = new List<ReleaseComponent>(),
                    Mapping = currentProposal.Mapping,
                    CreatedOn = DateTime.Now
                };

                currentProposal.Mapping.Mappings.Add(newCommittedMapping);
                await ComponentWriter.SaveChanges();

                return CreatedAtAction("GetById", newCommittedMapping.Mapping.Component.Id, newCommittedMapping);
            }

            await ComponentWriter.SaveChanges();
            return Ok();
        }

        protected abstract TReadModel ConvertDbModelToReadModel(Component component);

        protected abstract TVersionedReadModel
            ConvertVersionedDbModelToReadModel(VersionedComponent versionedComponent);

        protected ProposalReadModel ConvertProposalDbModelToProposalReadModel(ProposalMappingEntry proposalMappingEntry)
        {
            return new ProposalReadModel()
            {
                Id = proposalMappingEntry.Id,
                ProposedFor = proposalMappingEntry.Mapping.Id,
                GameVersion = proposalMappingEntry.Mapping.GameVersion.Id,
                ProposedBy = proposalMappingEntry.ProposedBy,
                ProposedOn = proposalMappingEntry.ProposedOn,
                IsOpen = proposalMappingEntry.IsOpen,
                IsPublicVote = proposalMappingEntry.IsPublicVote,
                VotedFor = proposalMappingEntry.VotedFor.ToList(),
                VotedAgainst = proposalMappingEntry.VotedAgainst.ToList(),
                Comment = proposalMappingEntry.Comment,
                ClosedBy = proposalMappingEntry.ClosedBy,
                ClosedOn = proposalMappingEntry.ClosedOn,
                In = proposalMappingEntry.InputMapping,
                Out = proposalMappingEntry.OutputMapping,
                Documentation = proposalMappingEntry.Documentation,
                MappingName = proposalMappingEntry.MappingType.Name
            };
        }

        protected MappingReadModel ConvertProposalDbModelToMappingReadModel(ProposalMappingEntry proposalMappingEntry)
        {
            return new MappingReadModel()
            {
                Id = proposalMappingEntry.Id,
                In = proposalMappingEntry.InputMapping,
                Out = proposalMappingEntry.OutputMapping,
                Documentation = proposalMappingEntry.Documentation,
                MappingName = proposalMappingEntry.MappingType.Name
            };
        }

        protected DetailedMappingReadModel ConvertLiveDbModelToDetailedMappingReadModel(LiveMappingEntry liveMappingEntry)
        {
            return new DetailedMappingReadModel()
            {
                Id = liveMappingEntry.Id,
                In = liveMappingEntry.InputMapping,
                Out = liveMappingEntry.OutputMapping,
                Proposal = liveMappingEntry.Proposal.Id,
                Releases = liveMappingEntry.Releases.Select(release => release.Id),
                VersionedMapping = liveMappingEntry.Mapping.Id,
                Documentation = liveMappingEntry.Documentation,
                MappingName = liveMappingEntry.MappingType.Name
            };
        }

        protected MappingReadModel ConvertLiveDbModelToMappingReadModel(LiveMappingEntry liveMappingEntry)
        {
            return new MappingReadModel()
            {
                Id = liveMappingEntry.Id,
                In = liveMappingEntry.InputMapping,
                Out = liveMappingEntry.OutputMapping,
                Documentation = liveMappingEntry.Documentation,
                MappingName = liveMappingEntry.MappingType.Name
            };
        }
    }
}
