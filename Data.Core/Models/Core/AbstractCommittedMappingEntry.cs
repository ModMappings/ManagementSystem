using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Data.Core.Models.Core
{
    public abstract class
        AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TCommittedEntry,
            TProposalEntry> : AbstractMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TMapping : AbstractMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TVersionedMapping : AbstractVersionedMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TCommittedEntry :
        AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TProposalEntry : AbstractProposalMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
    {
        [Required]
        public TProposalEntry Proposal { get; set; }
        public List<Release> Releases { get; set; }
    }
}
