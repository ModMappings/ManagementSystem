using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Mcms.IO.Core.Artifacts;

namespace Mcms.IO.Maven
{
    public abstract class MavenArtifact : IArtifact
    {
        protected MavenArtifact(MavenProject project, string version, string classifier = null, string extension = "jar")
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
            Version = version ?? throw new ArgumentNullException(nameof(version));
            Classifier = classifier;
            Extension = extension ?? throw new ArgumentException(nameof(extension));
        }

        public MavenProject Project { get; private set; }

        public string Version { get; private set; }

        public string Classifier { get; private set; }

        public string Extension { get; private set; }

        public string FileName =>
            $"{(new List<string> {Project.Name, Version, Classifier}).Where(s => !string.IsNullOrWhiteSpace(s)).AsEnumerable().Aggregate((s, s1) => $"{s}-{s1}")}.{Extension}";

        public Url Path => new Url(Project.Path).AppendPathSegment(Version).AppendPathSegment(FileName);

        public abstract string Name { get; set; }

        public abstract string GameVersion { get; set; }

        public async Task<Stream> GetStreamAsync(
            CancellationToken cancellationToken = default)
        {
            return await Path.GetStreamAsync(cancellationToken);
        }

        public Task WriteStreamAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }
    }
}
