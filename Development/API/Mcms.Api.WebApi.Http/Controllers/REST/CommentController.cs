using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Mcms.Api.Business.Poco.Api.REST.Mapping.Comment;
using Mcms.Api.Data.Core.Manager.Comments;
using Mcms.Api.Data.Poco.Models.Comments;
using Mcms.Api.WebApi.Http.Extensions;
using Mcms.Api.WebApi.Http.Model;
using Mcms.Api.WebApi.Http.Services.Core;
using Microsoft.AspNetCore.Mvc;

namespace Mcms.Api.WebApi.Http.Controllers.REST
{

    [Route("rest/comment")]
    [ApiController]
    public class CommentController : Controller
    {

        private readonly ICommentDataManager _commentDataManager;
        private readonly IMapper _mapper;

        private readonly IUserResolvingService _userResolvingService;

        public CommentController(ICommentDataManager commentDataManager, IMapper mapper, IUserResolvingService userResolvingService)
        {
            _commentDataManager = commentDataManager;
            _mapper = mapper;
            _userResolvingService = userResolvingService;
        }

        /// <summary>
        /// Get method to get a given comment with a given id.
        /// </summary>
        /// <param name="id">The id of a comment to lookup.</param>
        /// <returns>200 - The comment with the given id, 404 - If no comment with the given id is found.</returns>
        [HttpGet()]
        [Route("{id}")]
        public async Task<ActionResult<CommentDto>> Get(
            Guid id
        )
        {
            var rawResultQuery = await _commentDataManager.FindById(id);
            if (rawResultQuery == null || !rawResultQuery.Any())
            {
                return NotFound($"No comment exists with the given id: {id}");
            }

            var rawResult = rawResultQuery.First();

            return Ok(_mapper.Map<CommentDto>(rawResult));
        }

        /// <summary>
        /// Method that looks up the comments based on its properties.
        /// </summary>
        /// <param name="contentRegex">The regex to match the content of comments against.</param>
        /// <param name="releaseNameRegex">The regex to match release names against.</param>
        /// <param name="proposedMappingId">The id of the proposed mapping to get the comments for.</param>
        /// <param name="parentCommentId">The id of the parent comment to look comments up for.</param>
        /// <param name="pageIndex">The 0-based page index to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paged list of elements that matches the given data.</returns>
        [HttpGet()]
        [Route("")]
        public async Task<ActionResult<PagedList<CommentDto>>> List(
            [FromQuery(Name = "contentRegex")] string contentRegex = null,
            [FromQuery(Name = "releaseNameRegex")] string releaseNameRegex = null,
            [FromQuery(Name = "proposedMappingId")] Guid? proposedMappingId = null,
            [FromQuery(Name = "parentCommentId")] Guid? parentCommentId = null,
            [FromQuery(Name = "pageIndex")] int pageIndex = 0,
            [FromQuery(Name = "pageSize")] int pageSize = 25
        )
        {
            var rawQueryable = await _commentDataManager.FindUsingFilter(
                null,
                contentRegex,
                releaseNameRegex,
                proposedMappingId,
                parentCommentId
            );

            return rawQueryable.ProjectTo<CommentDto>(_mapper.ConfigurationProvider).AsPagedList(pageIndex, pageSize);
        }

        /// <summary>
        /// Deletes a given comment (and all of its related entities).
        /// </summary>
        /// <param name="id">The id of the comment to delete.</param>
        /// <returns>200 - Including the data of the comment that got deleted, 404 - If no comment exists with the given id.</returns>
        [HttpDelete()]
        [Route("{id}")]
        public async Task<ActionResult<CommentDto>> Delete(
            Guid id
        )
        {
            var rawData = await _commentDataManager.FindById(id);
            if (rawData == null || !rawData.Any())
            {
                return NotFound($"No comment can be found with a given id: {id}");
            }

            var target = rawData.First();

            await _commentDataManager.DeleteComment(target);
            await _commentDataManager.SaveChanges();

            return Ok(_mapper.Map<CommentDto>(target));
        }

        /// <summary>
        /// Creates a new comment from the given data.
        /// The api will create a new Id.
        ///
        /// The system returns the data after it has been saved in the database.
        /// </summary>
        /// <param name="commentDto">The data to create the new comment from.</param>
        /// <returns>200 - The updated comment data (should be identical to the input), with the id set.</returns>
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<CommentDto>> Create(
            [FromBody()] CommentDto commentDto
        )
        {
            var newId = Guid.NewGuid();
            var comment = _mapper.Map<Comment>(commentDto);

            comment.Id = newId;
            comment.CreatedOn = DateTime.Now;
            comment.CreatedBy = Guid.Parse(ClaimsPrincipal.Current.FindFirstValue(ClaimTypes.Sid));

            await _commentDataManager.CreateComment(comment);
            var rawNewData = _commentDataManager.FindById(newId);

            return Ok(_mapper.Map<CommentDto>(rawNewData));
        }

        /// <summary>
        /// Updates an existing comment with the data given by the dto.
        /// </summary>
        /// <param name="id">The id of the comment to update the data with.</param>
        /// <param name="commentDto">The data to update the comment with.</param>
        /// <returns>200 - The updated comment data (should be identical to the input), 404 - No comment with the given id exists.</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<CommentDto>> Patch(
            Guid id,
            [FromBody()] CommentDto commentDto
        )
        {
            var rawDataQuery = await _commentDataManager.FindById(id);
            if (rawDataQuery == null || !rawDataQuery.Any())
            {
                return NotFound("No comment with the given dto's id exists.");
            }

            var rawData = rawDataQuery.First();

            _mapper.Map(commentDto, rawData);

            await _commentDataManager.UpdateComment(rawData);
            await _commentDataManager.SaveChanges();

            return Ok(rawData);
        }
    }
}
