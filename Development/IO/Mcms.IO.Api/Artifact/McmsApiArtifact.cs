using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Mcms.Api.Data.Core.Manager.Core;
using Mcms.Api.Data.Core.Manager.Mapping.Component;
using Mcms.Api.Data.Core.Raw;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Mcms.IO.Core.Artifacts;

namespace Mcms.IO.Api.Artifact
{
    public class McmsApiArtifact : IArtifact
    {
        public string Name { get; }
        public string GameVersion { get; }

        public async Task<Stream> GetStreamAsync(CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("Mcms api artifacts give access to the raw DB directly!");
        }

        public async Task WriteStreamAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("Mcms api artifacts give access to the raw DB directly!");
        }
        
        public IRawDataAccessor RawDataAccessor { get; private set; }

        public IGameVersionDataManager GameVersionDataManager { get; private set; }

        public IMappingTypeDataManager MappingTypeDataManager { get; private set; }

        public IReleaseDataManager ReleaseDataManager { get; private set; }

        public IComponentDataManager ComponentDataManager { get; private set; }
    }
}
