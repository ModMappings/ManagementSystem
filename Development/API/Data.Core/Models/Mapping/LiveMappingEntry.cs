using System.Collections.Generic;

namespace Data.Core.Models.Mapping
{
    public class LiveMappingEntry
            : MappingEntryBase
    {
        public virtual ProposalMappingEntry Proposal { get; set; }

        public virtual List<ReleaseComponent> Releases { get; set; }
    }
}
