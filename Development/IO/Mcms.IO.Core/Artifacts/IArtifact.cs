using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mcms.IO.Core.Artifacts
{
    public interface IArtifact
    {
        string Name { get; }

        string GameVersion { get; }

        Task<Stream> GetStreamAsync(
            CancellationToken cancellationToken = default);

        Task WriteStreamAsync(
            byte[] data, CancellationToken cancellationToken = default);
    }
}
