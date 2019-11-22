using System.Dynamic;
using Flurl;
using Mcms.IO.Maven;

namespace Mcms.IO.Intermediary.Maven
{
    public class IntermediaryMavenProject
        : MavenProject
    {
        public static IntermediaryMavenProject Instance { get; } = new IntermediaryMavenProject(
            Constants.FABRIC_MAVEN_URL,
            Constants.FABRIC_GROUP,
            Constants.INTERMEDIARY_PROJECT_NAME);

        private IntermediaryMavenProject(Url url, string @group, string name) : base(url, @group, name)
        {
        }

        protected override MavenArtifact CreateNewArtifact(
            MavenProject project,
            string version,
            string classifier = null,
            string extension = "jar")
        {
            return IntermediaryMavenArtifact.Create(version);
        }
    }
}
