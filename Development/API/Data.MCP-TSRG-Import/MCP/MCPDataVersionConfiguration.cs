using System;
using System.Collections.Generic;

namespace Data.MCP.TSRG.Importer
{
    public class MCPDataVersionConfiguration
    {
        public IEnumerable<string> Snapshot { get; set; }

        public IEnumerable<string> Stable { get; set; }
    }
}
