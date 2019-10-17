using System;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Core.Manager.Mapping.Mappings.Voting;
using Mcms.Api.Data.Core.Stores;
using Mcms.Api.Data.EfCore.QueryFilters.Factory;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings.Voting;
using Microsoft.Extensions.Logging;

namespace Mcms.Api.Data.EfCore.Manager.Mapping.Mappings.Voting
{
    public class VotingRecordDataManager
        : IVotingRecordDataManager
    {

        private readonly ICallbackBasedQueryFilterFactory<VotingRecord> _queryFilterFactory;
        private readonly IStore<VotingRecord> _store;
        private readonly ILogger<VotingRecordDataManager> _logger;

        public VotingRecordDataManager(ICallbackBasedQueryFilterFactory<VotingRecord> queryFilterFactory, IStore<VotingRecord> store, ILogger<VotingRecordDataManager> logger)
        {
            _queryFilterFactory = queryFilterFactory;
            _store = store;
            _logger = logger;
        }

        public async Task<IQueryable<VotingRecord>> FindById(Guid id)
        {
            _logger.LogDebug($"Attempting to find voting record by id: '{id}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(v => v.Id == id)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<VotingRecord>> FindByProposal(Guid proposalId)
        {
            _logger.LogDebug($"Attempting to find voting record by proposal: '{proposalId}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(v => v.Proposal.Id == proposalId)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<VotingRecord>> FindByUser(Guid userId)
        {
            _logger.LogDebug($"Attempting to find voting record by user: '{userId}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(v => v.VotedBy == userId)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<VotingRecord>> FindUsingFilter(Guid? id = null, Guid? proposalId = null, Guid? userId = null)
        {
            _logger.LogDebug("Attempting to find voting record by filter data");
            if (id != null)
            {
                _logger.LogTrace($" > Id: '{id}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(v => v.Id == id)
                );
            }

            if (proposalId != null)
            {
                _logger.LogTrace($" > Proposal: '{proposalId}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(v => v.Proposal.Id == proposalId)
                );
            }

            if (userId != null)
            {
                _logger.LogTrace($" > User: '{userId}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(v => v.VotedBy == userId)
                );
            }

            var filter = _queryFilterFactory.Build();
            return await _store.ReadAsync(filter);
        }

        public async Task CreateVotingRecord(VotingRecord votingRecord)
        {
            _logger.LogDebug($"Creating new voting record: '{votingRecord.Id}'");
            await _store.Create(votingRecord);
        }

        public async Task UpdateVotingRecord(VotingRecord votingRecord)
        {
            _logger.LogDebug($"Updating voting record: '{votingRecord.Id}'");
            await _store.Update(votingRecord);
        }

        public async Task DeleteVotingRecord(VotingRecord votingRecord)
        {
            _logger.LogDebug($"Deleting voting record: '{votingRecord.Id}'");
            await _store.Delete(votingRecord);
        }

        public bool HasPendingChanges => _store.HasPendingChanges;

        public async Task SaveChanges()
        {
            _logger.LogDebug("Attempting to save voting record changes");
            await _store.CommitChanges();
        }
    }
}
