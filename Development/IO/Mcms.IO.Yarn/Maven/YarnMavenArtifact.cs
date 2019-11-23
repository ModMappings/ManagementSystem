using Mcms.IO.Maven;

namespace Mcms.IO.Yarn.Maven
{
    public class YarnMavenArtifact
        : MavenArtifact
    {

        public static YarnMavenArtifact Create(string version)
        {
            return new YarnMavenArtifact(
                YarnMavenProject.Instance,
                version);
        }


        private YarnMavenArtifact(MavenProject project, string version, string classifier = null, string extension = "jar") : base(project, version, classifier, extension)
        {
            Name = version;
            GameVersion = version;
        }

        public sealed override string Name { get; set; }
        public sealed override string GameVersion { get; set; }
    }
}
