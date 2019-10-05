using Flurl;

namespace Data.MCPTSRGImporter
{
    public static class Constants
    {

        public const string FORGE_MAVEN_URL = "https://files.minecraftforge.net/maven";

        public const string MCP_GROUP = "de.oceanlabs.mcp";

        public const string MCP_VERSION_INFO_URL = "http://export.mcpbot.bspk.rs/versions.json";
        public const string MCP_CONFIG_NAME = "mcp_config";
        public const string MCP_STABLE_NAME = "mcp_stable";
        public const string MCP_SNAPSHOT_NAME = "mcp_snapshot";

        public const string OBF_TO_TSRG_MAPPING_NAME = "OBF <-> TSRG";
        public const string TSRG_TO_MCP_MAPPING_NAME = "TSRG <-> MCP";

        public const string TSRG_JOINED_DATA = "config/joined.tsrg";
        public const string TSRG_STATIC_METHOD_DATA = "config/static_methods.txt";

        public const string MCP_FIELDS_DATA = "fields.csv";
        public const string MCP_METHODS_DATA = "methods.csv";
        public const string MCP_PARAMETERS_DATA = "parmeters.csv";
    }
}
