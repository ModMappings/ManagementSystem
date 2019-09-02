using System;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Readers.Core;

namespace Data.Core.Writers.Core
{
    public interface IUniqueNamedMappingWriter<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        : IUniqueNamedMappingReader<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>, IWriter<TMapping>
        where TMapping : AbstractMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TVersionedMapping : AbstractVersionedMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TCommittedEntry : AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TProposalEntry : AbstractProposalMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TReleaseEntry : AbstractReleaseMember<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
    {

        Task AddProposal(TProposalEntry proposalEntry);
    }
}
