using System;
using System.Collections.Generic;

namespace Data.MCPTSRGImporter
{
    public class MCPDataVersionConfiguration
    {
        public IEnumerable<string> Snapshot { get; set; }

        public IEnumerable<string> Stable { get; set; }
    }
}
