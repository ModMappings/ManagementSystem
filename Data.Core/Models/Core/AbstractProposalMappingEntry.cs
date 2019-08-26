using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Data.Core.Models.Core
{
    public abstract class AbstractProposalMappingEntry<TMapping, TVersionedMapping, TCommittedEntry,
        TProposalEntry> : AbstractMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TMapping : AbstractMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TVersionedMapping : AbstractVersionedMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TCommittedEntry :
        AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TProposalEntry : AbstractProposalMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
    {
        [Required]
        public User ProposedBy { get; set; }

        [Required]
        public DateTime ProposedOn { get; set; }

        public bool IsPublicVote { get; set; }

        public IQueryable<User> VotedFor { get; set; }

        public IQueryable<User> VotedAgainst { get; set; }

        [Required]
        public string Comment { get; set; }

        public User CommittedBy { get; set; }

        public DateTime CommittedOn { get; set; }
    }
}
