using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Data.Core.Models.Core
{
    public abstract class AbstractVersionedMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TMapping : AbstractMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TVersionedMapping : AbstractVersionedMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TCommittedEntry : AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TProposalEntry : AbstractProposalMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public GameVersion GameVersion { get; set; }

        [Required]
        public User CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public TMapping Mapping { get; set; }

        public List<TCommittedEntry> CommittedMappings { get; set; }

        public List<TProposalEntry> ProposalMappings { get; set; }
    }
}
