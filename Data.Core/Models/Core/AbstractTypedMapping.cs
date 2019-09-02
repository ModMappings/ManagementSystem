using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Core.Models.Core
{
    public class AbstractTypedMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TMapping : AbstractMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TVersionedMapping : AbstractVersionedMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TTypedMapping : AbstractTypedMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TCommittedEntry : AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TProposalEntry : AbstractProposalMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TReleaseEntry : AbstractReleaseMember<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public virtual TVersionedMapping VersionedMapping { get; set; }

        [Required]
        public virtual MappingType MappingType { get; set;}

        [Required]
        public virtual User CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public virtual List<TCommittedEntry> CommittedMappings { get; set; }

        public virtual List<TProposalEntry> ProposalMappings { get; set; }
    }
}
