using Flurl;
using Mcms.IO.Maven;

namespace Mcms.IO.Yarn.Maven
{
    public class YarnMavenProject
        : MavenProject
    {
        public static YarnMavenProject Instance { get; } = new YarnMavenProject(
            Constants.YARN_MAVEN_URL,
            Constants.FABRIC_GROUP,
            Constants.YARN_PROJECT_NAME);

        private YarnMavenProject(Url url, string @group, string name) : base(url, @group, name)
        {
        }

        protected override MavenArtifact CreateNewArtifact(
            MavenProject project,
            string version,
            string classifier = null,
            string extension = "jar")
        {
            return YarnMavenArtifact.Create(version);
        }
    }
}
