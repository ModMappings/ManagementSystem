using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Core.Manager.Core;
using Mcms.Api.Data.Core.Manager.Mapping.Component;
using Mcms.IO.Core.Artifacts;
using Mcms.IO.Core.Extensions;
using Mcms.IO.Core.Protocol.Reading;
using Mcms.IO.Core.Protocol.Writing;
using Mcms.IO.Data;
using Microsoft.Extensions.Logging;

namespace Mcms.IO.Api.Artifact
{
    public class McmsApiArtifactHandler : IArtifactHandler
    {
        private readonly ILogger<McmsApiArtifactHandler> _logger;
        
        public McmsApiArtifactHandler(IGameVersionDataManager gameVersionDataManager, IMappingTypeDataManager mappingTypeDataManager, IReleaseDataManager releaseDataManager, IComponentDataManager componentDataManager, McmsApiIOWriter writer, ILogger<McmsApiArtifactHandler> logger)
        {
            GameVersionDataManager = gameVersionDataManager;
            MappingTypeDataManager = mappingTypeDataManager;
            ReleaseDataManager = releaseDataManager;
            ComponentDataManager = componentDataManager;
            Writer = writer;
            _logger = logger;
        }

        public async Task<Dictionary<string, IArtifact>> GetArtifactsAsync()
        {
            var releasesQuery = await ReleaseDataManager.FindByName("*");
            return releasesQuery.ToList().ToDictionary(r => r.Name, r => CreateArtifact(r.Name, r.GameVersion.Name));
        }

        public Task<IArtifact> CreateNewArtifactWithName(string name)
        {
            throw new NotSupportedException("Can not create a release from just a name, use the ExternalRelease version to create an artifact from name and gameversion.");
        }

        public async Task PutArtifactsAsync(IEnumerable<ReadResult> releasesToPut)
        {
            await releasesToPut.ForEachWithProgressCallbackAsync(async release =>
                {
                    await Writer.WriteTo(release.Release, await CreateNewArtifactForRelease(release.Release),
                        new WriteContext(release.Strategies));
                },
                (count, current, percentage) =>
                {
                    _logger.LogInformation(
                        $"\t> {percentage}% ({current}/{count}): Writing artifacts ...");
                });
        }

        public Task<IArtifact> CreateNewArtifactForRelease(ExternalRelease externalRelease)
        {
            return Task.FromResult(CreateArtifact(externalRelease.Name, externalRelease.GameVersion));
        }

        private IArtifact CreateArtifact(string name, string gameVersion)
        {
            return new McmsApiArtifact(
                name,
                gameVersion,
                GameVersionDataManager,
                MappingTypeDataManager,
                ReleaseDataManager,
                ComponentDataManager
            );
        }

        private IGameVersionDataManager GameVersionDataManager { get; }

        private IMappingTypeDataManager MappingTypeDataManager { get; }

        private IReleaseDataManager ReleaseDataManager { get; }

        private IComponentDataManager ComponentDataManager { get; }
        
        private McmsApiIOWriter Writer { get; }
    }
}