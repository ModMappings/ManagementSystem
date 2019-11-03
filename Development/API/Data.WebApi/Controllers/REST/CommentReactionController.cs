using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.WebApi.Extensions;
using Data.WebApi.Model;
using Data.WebApi.Services.Core;
using Mcms.Api.Business.Poco.Api.REST.Mapping.Comment;
using Mcms.Api.Data.Core.Manager.Comments;
using Mcms.Api.Data.Poco.Models.Comments;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.REST
{

    [Route("rest/comment_reaction")]
    [ApiController]
    public class CommentReactionController : Controller
    {

        private readonly ICommentReactionDataManager _commentReactionDataManager;
        private readonly IMapper _mapper;

        private readonly IUserResolvingService _userResolvingService;

        public CommentReactionController(ICommentReactionDataManager commentReactionDataManager, IMapper mapper, IUserResolvingService userResolvingService)
        {
            _commentReactionDataManager = commentReactionDataManager;
            _mapper = mapper;
            _userResolvingService = userResolvingService;
        }

        /// <summary>
        /// Get method to get a given comment reaction with a given id.
        /// </summary>
        /// <param name="id">The id of a comment reaction to lookup.</param>
        /// <returns>200 - The comment reaction with the given id, 404 - If no comment reaction with the given id is found.</returns>
        [HttpGet()]
        [Route("get/{id}")]
        public async Task<ActionResult<CommentReactionDto>> Get(
            Guid id
        )
        {
            var rawResultQuery = await _commentReactionDataManager.FindById(id);
            if (rawResultQuery == null || !rawResultQuery.Any())
            {
                return NotFound($"No comment reaction exists with the given id: {id}");
            }

            var rawResult = rawResultQuery.First();

            return Ok(_mapper.Map<CommentReactionDto>(rawResult));
        }

        /// <summary>
        /// Method that looks up the commentReactions based on its properties.
        /// </summary>
        /// <param name="type">The type to match the comments reaction type against.</param>
        /// <param name="commentId">The id of the comment to get the comment reactions for.</param>
        /// <param name="pageIndex">The 0-based page index to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paged list of elements that matches the given data.</returns>
        [HttpGet()]
        [Route("list")]
        public async Task<ActionResult<PagedList<CommentReactionDto>>> List(
            [FromQuery(Name = "type")] CommentReactionType? type = null,
            [FromQuery(Name = "commentId")] Guid? commentId = null,
            [FromQuery(Name = "pageIndex")] int pageIndex = 0,
            [FromQuery(Name = "pageSize")] int pageSize = 25
        )
        {
            var rawQueryable = await _commentReactionDataManager.FindUsingFilter(
                null,
                type,
                commentId
            );

            return rawQueryable.ProjectTo<CommentReactionDto>(_mapper.ConfigurationProvider).AsPagedList(pageIndex, pageSize);
        }

        /// <summary>
        /// Deletes a given comment reaction (and all of its related entities).
        /// </summary>
        /// <param name="id">The id of the comment reaction to delete.</param>
        /// <returns>200 - Including the data of the comment reaction that got deleted, 404 - If no comment reaction exists with the given id.</returns>
        [HttpDelete()]
        [Route("delete/{id}")]
        public async Task<ActionResult<CommentReactionDto>> Delete(
            Guid id
        )
        {
            var rawData = await _commentReactionDataManager.FindById(id);
            if (rawData == null || !rawData.Any())
            {
                return NotFound($"No comment reaction can be found with a given id: {id}");
            }

            var target = rawData.First();

            await _commentReactionDataManager.DeleteCommentReaction(target);
            await _commentReactionDataManager.SaveChanges();

            return Ok(_mapper.Map<CommentReactionDto>(target));
        }

        /// <summary>
        /// Creates a new comment reaction from the given data.
        /// The api will create a new Id.
        ///
        /// The system returns the data after it has been saved in the database.
        /// </summary>
        /// <param name="commentReactionDto">The data to create the new comment reaction from.</param>
        /// <returns>200 - The updated comment reaction data (should be identical to the input), with the id set.</returns>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<CommentReactionDto>> Create(
            [FromBody()] CommentReactionDto commentReactionDto
        )
        {
            var newId = Guid.NewGuid();
            var commentReaction = _mapper.Map<CommentReaction>(commentReactionDto);

            commentReaction.Id = newId;
            commentReaction.CreatedOn = DateTime.Now;
            commentReaction.CreatedBy = Guid.Parse(ClaimsPrincipal.Current.FindFirstValue(ClaimTypes.Sid));

            await _commentReactionDataManager.CreateCommentReaction(commentReaction);
            var rawNewData = _commentReactionDataManager.FindById(newId);

            return Ok(_mapper.Map<CommentReactionDto>(rawNewData));
        }

        /// <summary>
        /// Updates an existing comment reaction with the data given by the dto.
        /// </summary>
        /// <param name="id">The id of the comment reaction to update the data with.</param>
        /// <param name="commentReactionDto">The data to update the comment reaction with.</param>
        /// <returns>200 - The updated comment reaction data (should be identical to the input), 404 - No comment reaction with the given id exists.</returns>
        [HttpPatch]
        [Route("patch/{id}")]
        public async Task<ActionResult<CommentReactionDto>> Patch(
            Guid id,
            [FromBody()] CommentReactionDto commentReactionDto
        )
        {
            var rawDataQuery = await _commentReactionDataManager.FindById(id);
            if (rawDataQuery == null || !rawDataQuery.Any())
            {
                return NotFound("No comment reaction with the given dto's id exists.");
            }

            var rawData = rawDataQuery.First();

            _mapper.Map(commentReactionDto, rawData);

            await _commentReactionDataManager.UpdateCommentReaction(rawData);
            await _commentReactionDataManager.SaveChanges();

            return Ok(rawData);
        }
    }
}
