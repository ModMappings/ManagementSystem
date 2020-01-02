using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.IO.Api.Artifact;
using Mcms.IO.Core;
using Mcms.IO.Core.Artifacts;
using Mcms.IO.Core.Extensions;
using Mcms.IO.Core.Reading;
using Mcms.IO.Data;
using Microsoft.Extensions.Logging;

namespace Mcms.IO.Api
{
    public class McmsApiIOReader : IIOReader
    {
        private readonly ILogger<McmsApiIOReader> _logger;
        
        public async Task<IEnumerable<ReadResult>> ReadAllFrom(IArtifactHandler handler)
        {
            _logger.LogDebug($"Importing all releases from: {handler}.");

            var releases = new List<ReadResult>();
            var toImport = await handler.GetArtifactsAsync();

            _logger.LogDebug($"There are {toImport.Count} artifacts to import.");

            foreach (var artifact in toImport.Values)
            {
                releases.Add(
                    await ReadFrom(artifact)
                );
            }

            return releases;
        }

        public async Task<ReadResult> ReadFrom(IArtifact artifact)
        {
            _logger.LogDebug($"Importing {artifact}...");
            var releaseDataManager = ((McmsApiArtifact) artifact).ReleaseDataManager;
            if (releaseDataManager == null)
                throw new ArgumentException( "No valid mcms artifact is given.", nameof(artifact));

            var releasesQuery = await releaseDataManager.FindByName(artifact.Name);
            var release = releasesQuery.FirstOrDefault();
            if (release == null)
                throw new ArgumentException("The mcms artifact targets an unknown release.");

            var externalRelease = new ExternalRelease
            {
                Name = artifact.Name,
                GameVersion = artifact.GameVersion,
                IsSnapshot = release.IsSnapshot,
                MappingType = release.MappingType.Name
            };

            var packageMappingsToWrite = release.Components
                .Where(rc => rc.Mapping.VersionedComponent.Component.Type == ComponentType.PACKAGE)
                .Select(rc => rc.Mapping);
            
            packageMappingsToWrite.ForEachWithProgressCallback(packageMapping =>
            {
                
            },
                (count, current, percentage) =>
                {
                    _logger.LogInformation(
                        $"\t\t\t> {percentage}% ({current}/{count}): Exporting packages from: {release.Name} ...");
                });
        }
    }
}