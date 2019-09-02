using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Core.Models.Core
{
    public abstract class AbstractMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
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
        public virtual TTypedMapping Mapping { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string InputMapping { get; set; }

        [Required]
        public string OutputMapping { get; set; }

        public string Documentation { get; set; }

        [Required]
        public Distribution Distribution { get; set; }
    }
}
