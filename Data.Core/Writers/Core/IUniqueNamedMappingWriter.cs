using System;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Readers.Core;

namespace Data.Core.Writers.Core
{
    public interface IUniqueNamedMappingWriter<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        : IUniqueNamedMappingReader<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>, IWriter<TMapping>
        where TMapping : AbstractMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TVersionedMapping : AbstractVersionedMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TCommittedEntry : AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TProposalEntry : AbstractProposalMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
    {

        Task AddProposal(TProposalEntry proposalEntry);
    }
}
