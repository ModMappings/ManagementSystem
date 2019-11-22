using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Flurl;
using Flurl.Http;
using Mcms.IO.Core.Artifacts;

namespace Mcms.IO.Maven
{
    /// <summary>
    /// Represents a single maven project.
    /// Defined by its workspace url as well as the group the project belongs to as well as the name of the project.
    /// </summary>
    public abstract class MavenProject : IArtifactHandler
    {
        protected MavenProject(Url url, string @group, string name)
        {
            URL = url ?? throw new ArgumentNullException(nameof(url));
            Group = @group ?? throw new ArgumentNullException(nameof(@group));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Url URL { get; }

        public string Group { get; }

        public string Name { get; }

        public Url Path => new Url(URL).AppendPathSegments(Group.Split(".").ToList()).AppendPathSegment(Name);

        public Url MetadataAddress => Path.AppendPathSegment("maven-metadata.xml");

        public async Task<IEnumerable<string>> GetPublishedVersions()
        {
            var mavenMetaDownloadResult = await MetadataAddress.GetAsync();
            if (!mavenMetaDownloadResult.IsSuccessStatusCode)
                return new List<string>();

            var xDoc = XDocument.Parse(
                Encoding.UTF8.GetString(mavenMetaDownloadResult.Content.ReadAsByteArrayAsync().Result));


            return xDoc.Root?.Element("versioning")
                ?.Element("versions")?.Elements()
                .Select(element => element.Value).OrderBy(s => s).ToList() ?? new List<string>();
        }

        public async Task<Dictionary<string, IArtifact>> GetArtifactsAsync()
        {
            return (await GetPublishedVersions()).Select(v => CreateNewArtifact(this, v))
                .ToDictionary(a => a.Version, a => (IArtifact) a);
        }

        public async Task<IArtifact> CreateNewArtifactWithName(string name)
        {
            return await Task.FromResult(CreateNewArtifact(this, name));
        }

        public Task PutArtifactsAsync(IReadOnlyDictionary<string, IArtifact> artifactsToPut)
        {
            throw new NotSupportedException("A MavenProject can not be used to write artifacts too.");
        }

        protected abstract MavenArtifact CreateNewArtifact(MavenProject project, string version,
            string classifier = null, string extension = "jar");
    }
}
