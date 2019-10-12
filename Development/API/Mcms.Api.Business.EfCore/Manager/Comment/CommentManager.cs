using System;
using System.Linq;
using System.Threading.Tasks;
using Data.EFCore.QueryFilters.Factory;
using Mcms.Api.Business.Core.Manager.Comments;
using Mcms.Api.Business.Core.Stores;
using Microsoft.Extensions.Logging;

namespace Data.EFCore.Manager.Comment
{
    public class CommentManager
        : ICommentManager
    {

        private readonly ICallbackBasedQueryFilterFactory<Mcms.Api.Data.Poco.Models.Comments.Comment>
            _queryFilterFactory;

        private readonly IStore<Mcms.Api.Data.Poco.Models.Comments.Comment> _store;
        private readonly ILogger<Mcms.Api.Data.Poco.Models.Comments.Comment> _logger;

        public CommentManager(ICallbackBasedQueryFilterFactory<Mcms.Api.Data.Poco.Models.Comments.Comment> queryFilterFactory, IStore<Mcms.Api.Data.Poco.Models.Comments.Comment> store, ILogger<Mcms.Api.Data.Poco.Models.Comments.Comment> logger)
        {
            _queryFilterFactory = queryFilterFactory;
            _store = store;
            _logger = logger;
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Comments.Comment>> FindById(Guid id)
        {
            _logger.LogDebug("");
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Comments.Comment>> FindByContent(string contentRegex)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Comments.Comment>> FindByRelease(string releaseNameRegex)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Comments.Comment>> FindByProposedMapping(Guid proposedMappingId)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Comments.Comment>> FindUsingFilter(Guid? id = null, string contentRegex = null, string releaseNameRegex = null,
            Guid? proposedMappingId = null)
        {
            throw new NotImplementedException();
        }

        public async Task CreateComment(Mcms.Api.Data.Poco.Models.Comments.Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateComment(Mcms.Api.Data.Poco.Models.Comments.Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteComment(Mcms.Api.Data.Poco.Models.Comments.Comment comment)
        {
            throw new NotImplementedException();
        }

        public bool HasPendingChanges { get; }
        public async Task SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
