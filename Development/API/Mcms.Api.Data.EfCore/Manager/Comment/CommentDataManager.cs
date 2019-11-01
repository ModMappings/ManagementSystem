using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mcms.Api.Data.Core.Manager.Comments;
using Mcms.Api.Data.Core.Stores;
using Mcms.Api.Data.EfCore.QueryFilters.Factory;
using Microsoft.Extensions.Logging;

namespace Mcms.Api.Data.EfCore.Manager.Comment
{
    public class CommentDataManager
        : ICommentDataManager
    {

        private readonly ICallbackBasedQueryFilterFactory<Mcms.Api.Data.Poco.Models.Comments.Comment>
            _queryFilterFactory;

        private readonly IStore<Mcms.Api.Data.Poco.Models.Comments.Comment> _store;
        private readonly ILogger<Mcms.Api.Data.Poco.Models.Comments.Comment> _logger;

        public CommentDataManager(ICallbackBasedQueryFilterFactory<Mcms.Api.Data.Poco.Models.Comments.Comment> queryFilterFactory, IStore<Mcms.Api.Data.Poco.Models.Comments.Comment> store, ILogger<Mcms.Api.Data.Poco.Models.Comments.Comment> logger)
        {
            _queryFilterFactory = queryFilterFactory;
            _store = store;
            _logger = logger;
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Comments.Comment>> FindById(Guid id)
        {
            _logger.LogDebug($"Attempting to find comment by id: '{id}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(c => c.Id == id)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Comments.Comment>> FindByContent(string contentRegex)
        {
            _logger.LogDebug($"Attempting to find comment by content regex: '{contentRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(c => Regex.IsMatch(c.Content, contentRegex))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Comments.Comment>> FindByRelease(string releaseNameRegex)
        {
            _logger.LogDebug($"Attempting to find comment by release name regex: '{releaseNameRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(c => Regex.IsMatch(c.Release.Name, releaseNameRegex))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Comments.Comment>> FindByProposedMapping(Guid proposedMappingId)
        {
            _logger.LogDebug($"Attempting to find comment by proposed mapping id: '{proposedMappingId}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(c => c.ProposedMapping.Id == proposedMappingId)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Comments.Comment>> FindUsingFilter(Guid? id = null, string contentRegex = null, string releaseNameRegex = null,
            Guid? proposedMappingId = null,
            Guid? parentCommentId = null)
        {
            _logger.LogDebug("Attempting to find comment by filter data.");
            if (id != null)
            {
                _logger.LogTrace($" > Id: '{id}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(c => c.Id == id)
                );
            }

            if (contentRegex != null)
            {
                _logger.LogTrace($" > Content regex: '{contentRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(c => Regex.IsMatch(c.Content, contentRegex))
                );
            }

            if (releaseNameRegex != null)
            {
                _logger.LogTrace($" > Release name regex: '{releaseNameRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(c => Regex.IsMatch(c.Release.Name, releaseNameRegex))
                );
            }

            if (proposedMappingId != null)
            {
                _logger.LogTrace($" > Proposed mapping id: '{proposedMappingId}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(c => c.ProposedMapping.Id == proposedMappingId)
                );
            }

            if (proposedMappingId != null)
            {
                _logger.LogTrace($" > Parent comment id: '{parentCommentId}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(c => c.Parent.Id == parentCommentId)
                );
            }

            var filter = _queryFilterFactory.Build();
            return await _store.ReadAsync(filter);
        }

        public async Task CreateComment(Mcms.Api.Data.Poco.Models.Comments.Comment comment)
        {
            _logger.LogDebug($"Creating new comment: '{comment.Id}'");
            await _store.Create(comment);
        }

        public async Task UpdateComment(Mcms.Api.Data.Poco.Models.Comments.Comment comment)
        {
            _logger.LogDebug($"Updating comment: '{comment.Id}'");
            await _store.Update(comment);
        }

        public async Task DeleteComment(Mcms.Api.Data.Poco.Models.Comments.Comment comment)
        {
            _logger.LogDebug($"Delete comment: '{comment.Id}'");
            await _store.Delete(comment);
        }

        public bool HasPendingChanges => _store.HasPendingChanges;

        public async Task SaveChanges()
        {
            _logger.LogDebug("Attempting to save comment changes.");
            await _store.CommitChanges();
        }
    }
}
