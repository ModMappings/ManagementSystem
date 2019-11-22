using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mcms.IO.Core.Artifacts
{
    public interface IArtifact
    {
        string Name { get; set; }

        string GameVersion { get; set; }

        Task<Stream> GetStreamAsync(
            CancellationToken cancellationToken = default);
    }
}
