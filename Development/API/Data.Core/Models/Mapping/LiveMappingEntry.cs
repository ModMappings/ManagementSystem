using System.Collections.Generic;
using Data.Core.Models.Mapping.Proposals;

namespace Data.Core.Models.Mapping
{
    public class LiveMappingEntry
            : MappingBase
    {
        public virtual ProposedMapping ProposedMapping { get; set; }

        public virtual List<ReleaseComponent> Releases { get; set; }
    }
}
