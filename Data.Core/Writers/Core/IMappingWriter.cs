using System;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Readers.Core;

namespace Data.Core.Writers.Core
{
    public interface IMappingWriter<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        : IMappingReader<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>, IWriter<TMapping>
        where TMapping : IMapping<TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TVersionedMapping : IVersionedMapping<TCommittedEntry, TProposalEntry>
        where TCommittedEntry : ICommittedMappingEntry<TProposalEntry>
        where TProposalEntry : IProposalMappingEntry
    {
    }
}
