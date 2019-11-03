using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.WebApi.Extensions;
using Data.WebApi.Model;
using Data.WebApi.Services.Core;
using Mcms.Api.Business.Poco.Api.REST.Mapping.Mappings.Voting;
using Mcms.Api.Data.Core.Manager.Mapping.Mappings.Voting;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings.Voting;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.REST
{

    [Route("rest/voting_record")]
    [ApiController]
    public class VotingRecordController : Controller
    {

        private readonly IVotingRecordDataManager _votingRecordDataManager;
        private readonly IMapper _mapper;

        private readonly IUserResolvingService _userResolvingService;

        public VotingRecordController(IVotingRecordDataManager votingRecordDataManager, IMapper mapper, IUserResolvingService userResolvingService)
        {
            _votingRecordDataManager = votingRecordDataManager;
            _mapper = mapper;
            _userResolvingService = userResolvingService;
        }

        /// <summary>
        /// Get method to get a given voting record with a given id.
        /// </summary>
        /// <param name="id">The id of a voting record to lookup.</param>
        /// <returns>200 - The voting record with the given id, 404 - If no voting record with the given id is found.</returns>
        [HttpGet()]
        [Route("get/{id}")]
        public async Task<ActionResult<VotingRecordDto>> Get(
            Guid id
        )
        {
            var rawResultQuery = await _votingRecordDataManager.FindById(id);
            if (rawResultQuery == null || !rawResultQuery.Any())
            {
                return NotFound($"No voting record exists with the given id: {id}");
            }

            var rawResult = rawResultQuery.First();

            return Ok(_mapper.Map<VotingRecordDto>(rawResult));
        }

        /// <summary>
        /// Method that looks up the votingRecords based on its properties.
        /// </summary>
        /// <param name="proposalId">The id of the proposal to find the voting records for.</param>
        /// <param name="userId">The user id to find the voting records for.</param>
        /// <param name="pageIndex">The 0-based page index to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paged list of elements that matches the given data.</returns>
        [HttpGet()]
        [Route("list")]
        public async Task<ActionResult<PagedList<VotingRecordDto>>> List(
            [FromQuery(Name = "proposalId")] Guid? proposalId = null,
            [FromQuery(Name = "userId")] Guid? userId = null,
            [FromQuery(Name = "pageIndex")] int pageIndex = 0,
            [FromQuery(Name = "pageSize")] int pageSize = 25
        )
        {
            var rawQueryable = await _votingRecordDataManager.FindUsingFilter(
                null,
                proposalId,
                userId
            );

            return rawQueryable.ProjectTo<VotingRecordDto>(_mapper.ConfigurationProvider).AsPagedList(pageIndex, pageSize);
        }

        /// <summary>
        /// Deletes a given voting record (and all of its related entities).
        /// </summary>
        /// <param name="id">The id of the voting record to delete.</param>
        /// <returns>200 - Including the data of the voting record that got deleted, 404 - If no voting record exists with the given id.</returns>
        [HttpDelete()]
        [Route("delete/{id}")]
        public async Task<ActionResult<VotingRecordDto>> Delete(
            Guid id
        )
        {
            var rawData = await _votingRecordDataManager.FindById(id);
            if (rawData == null || !rawData.Any())
            {
                return NotFound($"No voting record can be found with a given id: {id}");
            }

            var target = rawData.First();

            await _votingRecordDataManager.DeleteVotingRecord(target);
            await _votingRecordDataManager.SaveChanges();

            return Ok(_mapper.Map<VotingRecordDto>(target));
        }

        /// <summary>
        /// Creates a new voting record from the given data.
        /// The api will create a new Id.
        ///
        /// The system returns the data after it has been saved in the database.
        /// </summary>
        /// <param name="votingRecordDto">The data to create the new voting record from.</param>
        /// <returns>200 - The updated voting record data (should be identical to the input), with the id set.</returns>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<VotingRecordDto>> Create(
            [FromBody()] VotingRecordDto votingRecordDto
        )
        {
            var newId = Guid.NewGuid();
            var votingRecord = _mapper.Map<VotingRecord>(votingRecordDto);

            votingRecord.Id = newId;
            votingRecord.CreatedOn = DateTime.Now;
            votingRecord.VotedBy = Guid.Parse(ClaimsPrincipal.Current.FindFirstValue(ClaimTypes.Sid));

            await _votingRecordDataManager.CreateVotingRecord(votingRecord);
            var rawNewData = _votingRecordDataManager.FindById(newId);

            return Ok(_mapper.Map<VotingRecordDto>(rawNewData));
        }

        /// <summary>
        /// Updates an existing voting record with the data given by the dto.
        /// </summary>
        /// <param name="id">The id of the voting record to update the data with.</param>
        /// <param name="votingRecordDto">The data to update the voting record with.</param>
        /// <returns>200 - The updated voting record data (should be identical to the input), 404 - No voting record with the given id exists.</returns>
        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult<VotingRecordDto>> Patch(
            Guid id,
            [FromBody()] VotingRecordDto votingRecordDto
        )
        {
            var rawDataQuery = await _votingRecordDataManager.FindById(id);
            if (rawDataQuery == null || !rawDataQuery.Any())
            {
                return NotFound("No voting record with the given dto's id exists.");
            }

            var rawData = rawDataQuery.First();

            _mapper.Map(votingRecordDto, rawData);

            await _votingRecordDataManager.UpdateVotingRecord(rawData);
            await _votingRecordDataManager.SaveChanges();

            return Ok(rawData);
        }
    }
}
