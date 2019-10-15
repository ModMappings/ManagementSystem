using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data.EFCore.QueryFilters.Factory;
using Mcms.Api.Business.Core.Manager.Mapping.Mappings.Voting;
using Mcms.Api.Business.Core.Stores;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings.Voting;
using Microsoft.Extensions.Logging;

namespace Data.EFCore.Manager.Mapping.Mappings.Voting
{
    public class VotingRecordManager
        : IVotingRecordManager
    {

        private readonly ICallbackBasedQueryFilterFactory<VotingRecord> _queryFilterFactory;
        private readonly IStore<VotingRecord> _store;
        private readonly ILogger<VotingRecordManager> _logger;

        public async Task<IQueryable<VotingRecord>> FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<VotingRecord>> FindByProposal(Guid proposalId)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<VotingRecord>> FindByUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<VotingRecord>> FindUsingFilter(Guid? id = null, Guid? proposalId = null, Guid? userId = null)
        {
            throw new NotImplementedException();
        }

        public async Task CreateVotingRecord(VotingRecord votingRecord)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateVotingRecord(VotingRecord votingRecord)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteVotingRecord(VotingRecord votingRecord)
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
