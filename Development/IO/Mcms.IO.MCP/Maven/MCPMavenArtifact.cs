using Mcms.IO.Maven;

namespace Mcms.IO.MCP.Maven
{
    public class MCPMavenArtifact
        : MavenArtifact
    {

        public static MCPMavenArtifact Create(string version)
        {
            return new MCPMavenArtifact(
                MCPStableMavenProject.Instance,
                version);
        }


        private MCPMavenArtifact(MavenProject project, string version, string classifier = null, string extension = "jar") : base(project, version, classifier, extension)
        {
            Name = version;
            GameVersion = version.Split("-")[0];
        }

        public sealed override string Name { get; set; }
        public sealed override string GameVersion { get; set; }
    }
}
