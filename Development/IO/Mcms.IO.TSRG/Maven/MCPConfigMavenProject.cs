using Flurl;
using Mcms.IO.Maven;

namespace Mcms.IO.TSRG.Maven
{
    public class MCPConfigMavenProject
        : MavenProject
    {
        public static MCPConfigMavenProject Instance { get; } = new MCPConfigMavenProject(
            Constants.MCP_MAVEN_URL,
            Constants.MCP_CONFIG_GROUP,
            Constants.MCP_CONFIG_PROJECT_NAME);

        private MCPConfigMavenProject(Url url, string @group, string name) : base(url, @group, name)
        {
        }

        protected override MavenArtifact CreateNewArtifact(
            MavenProject project,
            string version,
            string classifier = null,
            string extension = "jar")
        {
            return MCPConfigMavenArtifact.Create(version);
        }
    }
}
