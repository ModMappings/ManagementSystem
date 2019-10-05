using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Flurl;
using Flurl.Http;

namespace Data.MCPTSRGImporter
{
    /// <summary>
    /// Represents a single maven project.
    /// Defined by its workspace url as well as the group the project belongs to as well as the name of the project.
    /// </summary>
    public class MavenProject
    {
        public static MavenProject Create(string url, string @group, string name)
        {
            return new MavenProject(new Url(url), @group, name);
        }

        public static MavenProject Create(Uri url, string @group, string name)
        {
            return new MavenProject(url, @group, name);
        }

        private MavenProject(Url url, string @group, string name)
        {
            URL = url ?? throw new ArgumentNullException(nameof(url));
            Group = @group ?? throw new ArgumentNullException(nameof(@group));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Url URL { get; private set; }

        public string Group { get; private set; }

        public string Name { get; private set; }

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

        public async Task<Dictionary<string, MavenArtifact>> GetArtifacts()
        {
            return (await GetPublishedVersions()).Select(v => MavenArtifact.Create(this, v))
                .ToDictionary(a => a.Version, a => a);
        }
    }
}
