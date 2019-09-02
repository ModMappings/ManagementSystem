using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Core.Models.Core
{
    public abstract class AbstractProposalMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry,
        TProposalEntry, TReleaseEntry> : AbstractMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TMapping : AbstractMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TVersionedMapping : AbstractVersionedMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TTypedMapping : AbstractTypedMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TCommittedEntry :
        AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TProposalEntry : AbstractProposalMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TReleaseEntry : AbstractReleaseMember<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
    {
        [Required]
        public virtual User ProposedBy { get; set; }

        [Required]
        public DateTime ProposedOn { get; set; }

        [Required]
        public bool IsOpen { get; set; }

        [Required]
        public bool IsPublicVote { get; set; }

        public virtual List<User> VotedFor { get; set; }

        public virtual List<User> VotedAgainst { get; set; }

        [Required]
        public string Comment { get; set; }

        public virtual User ClosedBy { get; set; }

        public DateTime? ClosedOn { get; set; }

        public bool? Merged { get; set; }

        public Guid? MergedWithId { get; set; }

        [ForeignKey("MergedWithId")]
        public virtual TCommittedEntry MergedWith { get; set; }
    }
}
