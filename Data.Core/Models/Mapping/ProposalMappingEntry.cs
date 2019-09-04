using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Core.Models.Core;

namespace Data.Core.Models.Mapping
{
    public class ProposalMappingEntry
        : MappingEntryBase
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

        public Guid? WentLiveWithId { get; set; }

        [ForeignKey("WentLiveWithId")]
        public virtual LiveMappingEntry WentLiveWith { get; set; }
    }
}
