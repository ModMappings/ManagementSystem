using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Core;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Metadata;
using Mcms.IO.Api.Artifact;
using Mcms.IO.Core;
using Mcms.IO.Core.Artifacts;
using Mcms.IO.Core.Deduplication;
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
                MappingType = release.MappingType.Name,
                Packages = new List<ExternalPackage>()
            };

            var packageMappingsToWrite = release.Components
                .Where(rc => rc.Mapping.VersionedComponent.Component.Type == ComponentType.PACKAGE)
                .Select(rc => rc.Mapping);
            
            packageMappingsToWrite.ForEachWithProgressCallback(packageMapping =>
            {
                var externalPackage = new ExternalPackage()
                {
                    Classes = new List<ExternalClass>(),
                    Distribution = ConvertInternalDistributionToExternal(packageMapping.Distribution),
                    Documentation = packageMapping.Documentation,
                    Input = packageMapping.InputMapping,
                    Output = packageMapping.OutputMapping
                };

                externalRelease.Packages.Add(externalPackage);

                var classMappingsToWrite =
                    ((PackageMetadata) packageMapping.VersionedComponent.Metadata).Classes.SelectMany(cm =>
                        cm.VersionedComponent.Mappings.Where(m => m.Releases.Any(rc => rc.Release == release)));
                
                classMappingsToWrite.ForEachWithProgressCallback(classMapping =>
                    {
                        var externalClass = new ExternalClass()
                        {
                            Distribution = ConvertInternalDistributionToExternal(classMapping.Distribution),
                            Documentation = classMapping.Documentation,
                            Input = classMapping.InputMapping,
                            Output = classMapping.OutputMapping,
                            Fields = new List<ExternalField>(),
                            Methods = new List<ExternalMethod>()
                        };

                        externalPackage.Classes.Add(externalClass);
                        
                        var methodMappingsToWrite =
                            ((ClassMetadata) classMapping.VersionedComponent.Metadata).Methods.SelectMany(cm =>
                                cm.VersionedComponent.Mappings.Where(m => m.Releases.Any(rc => rc.Release == release)));
                        
                        var fieldMappingsToWrite =
                            ((ClassMetadata) classMapping.VersionedComponent.Metadata).Methods.SelectMany(cm =>
                                cm.VersionedComponent.Mappings.Where(m => m.Releases.Any(rc => rc.Release == release)));
                        
                        methodMappingsToWrite.ForEachWithProgressCallback(methodMapping =>
                            {
                                var externalMethod = new ExternalMethod()
                                {
                                    Distribution = ConvertInternalDistributionToExternal(methodMapping.Distribution),
                                    Documentation = methodMapping.Documentation,
                                    Input = methodMapping.InputMapping,
                                    Output = methodMapping.OutputMapping,
                                    Parameters = new List<ExternalParameter>(),
                                    Descriptor = ((MethodMetadata) methodMapping.VersionedComponent.Metadata).Descriptor,
                                    IsStatic = ((MethodMetadata) methodMapping.VersionedComponent.Metadata).IsStatic
                                };

                                externalClass.Methods.Add(externalMethod);
                        
                                var parameterMappingsToWrite =
                                    ((MethodMetadata) methodMapping.VersionedComponent.Metadata).Parameters.SelectMany(cm =>
                                        cm.VersionedComponent.Mappings.Where(m => m.Releases.Any(rc => rc.Release == release)));
                        
                                parameterMappingsToWrite.ForEachWithProgressCallback(parameterMapping =>
                                    {
                                        var externalParameter = new ExternalParameter()
                                        {
                                            Distribution = ConvertInternalDistributionToExternal(parameterMapping.Distribution),
                                            Documentation = parameterMapping.Documentation,
                                            Input = parameterMapping.InputMapping,
                                            Output = parameterMapping.OutputMapping,
                                        };

                                        externalMethod.Parameters.Add(externalParameter);
                                    },
                                    (count, current, percentage) =>
                                    {
                                        _logger.LogInformation(
                                            $"\t\t\t\t> {percentage}% ({current}/{count}): Exporting parameters from: {externalClass.Input}<->{externalClass.Output} in {externalPackage.Input}<->{externalPackage.Output}, {release.Name} ...");
                                    });
                                
                            },
                            (count, current, percentage) =>
                            {
                                _logger.LogInformation(
                                    $"\t\t\t> {percentage}% ({current}/{count}): Exporting methods from: {externalClass.Input}<->{externalClass.Output} in {externalPackage.Input}<->{externalPackage.Output}, {release.Name} ...");
                            });
                        
                        fieldMappingsToWrite.ForEachWithProgressCallback(fieldMapping =>
                            {
                                var externalField = new ExternalField()
                                {
                                    Distribution = ConvertInternalDistributionToExternal(fieldMapping.Distribution),
                                    Documentation = fieldMapping.Documentation,
                                    Input = fieldMapping.InputMapping,
                                    Output = fieldMapping.OutputMapping,
                                    Type = ((FieldMetadata) fieldMapping.VersionedComponent.Metadata).Type,
                                    IsStatic = ((FieldMetadata) fieldMapping.VersionedComponent.Metadata).IsStatic
                                };

                                externalClass.Fields.Add(externalField);
                            },
                            (count, current, percentage) =>
                            {
                                _logger.LogInformation(
                                    $"\t\t\t> {percentage}% ({current}/{count}): Exporting fields from: {externalClass.Input}<->{externalClass.Output} in {externalPackage.Input}<->{externalPackage.Output}, {release.Name} ...");
                            });
                    },
                    (count, current, percentage) =>
                    {
                        _logger.LogInformation(
                            $"\t\t> {percentage}% ({current}/{count}): Exporting classes from: {externalPackage.Input}<->{externalPackage.Output} in {release.Name} ...");
                    });
            },
                (count, current, percentage) =>
                {
                    _logger.LogInformation(
                        $"\t> {percentage}% ({current}/{count}): Exporting packages from: {release.Name} ...");
                });
            
            return new ReadResult(
                externalRelease,
                new DeduplicationStrategies(
                    DeduplicationStrategy.ALWAYS_CREATE,
                    DeduplicationStrategy.ALWAYS_CREATE,
                    DeduplicationStrategy.ALWAYS_CREATE,
                    DeduplicationStrategy.ALWAYS_CREATE,
                    DeduplicationStrategy.ALWAYS_CREATE));
        }

        private ExternalDistribution ConvertInternalDistributionToExternal(Distribution distribution)
        {
            switch (distribution)
            {
                case Distribution.BOTH:
                    return ExternalDistribution.BOTH;
                case Distribution.SERVER_ONLY:
                    return ExternalDistribution.SERVER_ONLY;
                case Distribution.CLIENT_ONLY:
                    return ExternalDistribution.CLIENT_ONLY;
                case Distribution.UNKNOWN:
                    return ExternalDistribution.UNKNOWN;
                default:
                    return ExternalDistribution.UNKNOWN;
            }
        }
    }
}