using Flurl;
using Mcms.IO.Maven;

namespace Mcms.IO.MCP.Maven
{
    public class MCPStableMavenProject
        : MavenProject
    {
        public static MCPStableMavenProject Instance { get; } = new MCPStableMavenProject(
            Constants.MCP_MAVEN_URL,
            Constants.MCP_GROUP,
            Constants.MCP_STABLE_PROJECT_NAME);

        private MCPStableMavenProject(Url url, string @group, string name) : base(url, @group, name)
        {
        }

        protected override MavenArtifact CreateNewArtifact(
            MavenProject project,
            string version,
            string classifier = null,
            string extension = "jar")
        {
            return MCPStableMavenArtifact.Create(version);
        }
    }
}
