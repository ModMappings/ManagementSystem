using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required]
        public bool IsOpen { get; set; }

        [Required]
        public bool IsPublicVote { get; set; }

        public IQueryable<User> VotedFor { get; set; }

        public IQueryable<User> VotedAgainst { get; set; }

        [Required]
        public string Comment { get; set; }

        public User ClosedBy { get; set; }

        public DateTime? ClosedOn { get; set; }

        public bool? Merged { get; set; }

        public Guid? MergedWithId { get; set; }

        [ForeignKey("MergedWithId")]
        public TCommittedEntry MergedWith { get; set; }
    }
}
