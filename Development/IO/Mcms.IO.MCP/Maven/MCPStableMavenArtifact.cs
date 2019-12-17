using Mcms.IO.Maven;

namespace Mcms.IO.MCP.Maven
{
    public class MCPStableMavenArtifact
        : MavenArtifact
    {

        public static MCPStableMavenArtifact Create(string version)
        {
            return new MCPStableMavenArtifact(
                MCPStableMavenProject.Instance,
                version);
        }


        private MCPStableMavenArtifact(MavenProject project, string version, string classifier = null, string extension = "jar") : base(project, version, classifier, extension)
        {
            Name = version;
            GameVersion = version.Split("-")[0];
        }

        public sealed override string Name { get; set; }
        public sealed override string GameVersion { get; set; }
    }
}
