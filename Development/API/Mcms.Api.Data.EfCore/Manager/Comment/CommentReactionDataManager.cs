using System;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Core.Manager.Comments;
using Mcms.Api.Data.Core.Stores;
using Mcms.Api.Data.EfCore.QueryFilters.Factory;
using Mcms.Api.Data.Poco.Models.Comments;
using Microsoft.Extensions.Logging;

namespace Mcms.Api.Data.EfCore.Manager.Comment
{
    public class CommentReactionDataManager
        : ICommentReactionDataManager
    {

        private readonly ICallbackBasedQueryFilterFactory<CommentReaction> _queryFilterFactory;
        private readonly IStore<CommentReaction> _store;
        private readonly ILogger<CommentReactionDataManager> _logger;

        public CommentReactionDataManager(ICallbackBasedQueryFilterFactory<CommentReaction> queryFilterFactory, IStore<CommentReaction> store, ILogger<CommentReactionDataManager> logger)
        {
            _queryFilterFactory = queryFilterFactory;
            _store = store;
            _logger = logger;
        }

        public async Task<IQueryable<CommentReaction>> FindById(Guid id)
        {
            _logger.LogDebug($"Attempting to find comment reaction by id: '{id}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(r => r.Id == id)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<CommentReaction>> FindByType(CommentReactionType type)
        {
            _logger.LogDebug($"Attempting to find comment reaction by type: '{type}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(r => r.Type == type)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<CommentReaction>> FindByComment(Guid commentId)
        {
            _logger.LogDebug($"Attempting to find comment reaction by comment id: '{commentId}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(r => r.Comment.Id == commentId)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<CommentReaction>> FindUsingFilter(Guid? id = null, CommentReactionType? type = null, Guid? commentId = null)
        {
            _logger.LogDebug("Attempting to find comment reaction by filter data.");
            if (id != null)
            {
                _logger.LogTrace($" > Id: '{id}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(r => r.Id == id)
                );
            }

            if (type != null)
            {
                _logger.LogTrace($" > Type: '{type}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(r => r.Type == type)
                );
            }

            if (commentId != null)
            {
                _logger.LogTrace($" > Comment id: '{commentId}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(r => r.Comment.Id == commentId)
                );
            }

            var filter = _queryFilterFactory.Build();
            return await _store.ReadAsync(filter);
        }

        public async Task CreateComment(CommentReaction commentReaction)
        {
            _logger.LogDebug($"Creating new comment reaction: '{commentReaction.Id}'");
            await _store.Create(commentReaction);
        }

        public async Task UpdateComment(CommentReaction commentReaction)
        {
            _logger.LogDebug($"Updating comment reaction: '{commentReaction.Id}'");
            await _store.Update(commentReaction);
        }

        public async Task DeleteComment(CommentReaction commentReaction)
        {
            _logger.LogDebug($"Deleting comment reaction: '{commentReaction.Id}'");
            await _store.Delete(commentReaction);
        }

        public bool HasPendingChanges => _store.HasPendingChanges;

        public async Task SaveChanges()
        {
            _logger.LogDebug("Saving changes made to comment reaction.");
            await _store.CommitChanges();
        }
    }
}
