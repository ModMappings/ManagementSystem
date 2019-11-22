using System;
using Mcms.IO.Maven;

namespace Mcms.IO.Intermediary.Maven
{
    public class IntermediaryMavenArtifact
        : MavenArtifact
    {

        public static IntermediaryMavenArtifact Create(string version)
        {
            return new IntermediaryMavenArtifact(
                IntermediaryMavenProject.Instance,
                version);
        }


        private IntermediaryMavenArtifact(MavenProject project, string version, string classifier = null, string extension = "jar") : base(project, version, classifier, extension)
        {
            Name = version;
            GameVersion = version;
        }

        public sealed override string Name { get; set; }
        public sealed override string GameVersion { get; set; }
    }
}
