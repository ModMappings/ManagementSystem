using Mcms.IO.Maven;

namespace Mcms.IO.TSRG.Maven
{
    public class MCPConfigMavenArtifact
        : MavenArtifact
    {

        public static MCPConfigMavenArtifact Create(string version)
        {
            return new MCPConfigMavenArtifact(
                MCPConfigMavenProject.Instance,
                version);
        }


        private MCPConfigMavenArtifact(MavenProject project, string version, string classifier = null, string extension = "jar") : base(project, version, classifier, extension)
        {
            Name = version;
            GameVersion = version.Split("-")[0];
        }

        public sealed override string Name { get; set; }
        public sealed override string GameVersion { get; set; }
    }
}
