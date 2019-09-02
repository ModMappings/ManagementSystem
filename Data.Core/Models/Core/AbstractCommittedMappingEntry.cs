using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Data.Core.Models.Core
{
    public abstract class
        AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry,
            TProposalEntry, TReleaseEntry> : AbstractMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TMapping : AbstractMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TVersionedMapping : AbstractVersionedMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TTypedMapping : AbstractTypedMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TCommittedEntry :
        AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TProposalEntry : AbstractProposalMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TReleaseEntry : AbstractReleaseMember<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>

    {
        public virtual TProposalEntry Proposal { get; set; }
        public virtual List<TReleaseEntry> Releases { get; set; }
    }
}
