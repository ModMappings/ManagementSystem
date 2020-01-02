using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mcms.IO.Core;
using Mcms.IO.Core.Artifacts;
using Mcms.IO.Core.Deduplication;
using Mcms.IO.Core.Extensions;
using Mcms.IO.Core.Reading;
using Mcms.IO.Data;
using Microsoft.Extensions.Logging;

namespace Mcms.IO.Intermediary
{
    /// <summary>
    /// The IOReader for the intermediary file format.
    /// </summary>
    public class IntermediaryIOReader
        : IIOReader
    {
        private readonly ILogger<IntermediaryIOReader> _logger;

        public IntermediaryIOReader(ILogger<IntermediaryIOReader> logger)
        {
            _logger = logger;
        }

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
            var release = new ExternalRelease
            {
                Name = artifact.Name,
                GameVersion = artifact.GameVersion,
                MappingType = Constants.INTERMEDIARY_MAPPING_NAME
            };

            var packages = new Dictionary<string, ExternalPackage>();
            ZipArchive zip;
            try
            {
                zip = new ZipArchive(await artifact.GetStreamAsync());
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to download the Intermediary Config artifact data.");
                return new ReadResult(
                    release,
                    new DeduplicationStrategies(
                        DeduplicationStrategy.OUTPUT_UNIQUE,
                        DeduplicationStrategy.BOTH_UNIQUE,
                        DeduplicationStrategy.OUTPUT_UNIQUE,
                        DeduplicationStrategy.OUTPUT_UNIQUE,
                        DeduplicationStrategy.OUTPUT_UNIQUE));
            }

            var intermediaryJoinedFileContents =
                zip.ReadAllLines(Constants.INTERMEDIARY_JOINED_DATA, Encoding.UTF8).ToList();

            var analysisHelper = new IntermediaryAnalysisHelper(ref packages);

            intermediaryJoinedFileContents.ForEachWithProgressCallback((intermediaryLine) =>
                {
                    _logger.LogDebug($"Processing intermediary line: {intermediaryLine}");
                    if (intermediaryLine.StartsWith("CLASS"))
                    {
                        //New class
                        var intermediaryClassData = intermediaryLine.Split('\t');
                        var inputMapping = intermediaryClassData[1].Trim();
                        var outputMappingIncludingPackage = intermediaryClassData[2].Trim();

                        var outputMapping =
                            outputMappingIncludingPackage.Substring(outputMappingIncludingPackage.LastIndexOf('/'));
                        var package = outputMappingIncludingPackage.Replace(outputMapping, "").Replace("/", ".");
                        outputMapping = outputMapping.Substring(1);

                        _logger.LogDebug(
                            $"Processing entry as class, with mapping: {inputMapping} ->{outputMapping} in package: {package}");

                        analysisHelper.AddClass(inputMapping, outputMapping, package);
                    }
                    else if (intermediaryLine.StartsWith("METHOD"))
                    {
                        //New method
                        var intermediaryMethodData = intermediaryLine.Trim().Split('\t');
                        var inputMapping = intermediaryMethodData[3].Trim();
                        var descriptor = intermediaryMethodData[2].Trim();
                        var outputMapping = intermediaryMethodData[4].Trim();

                        _logger.LogDebug(
                            $"Processing entry as method, with mapping: {inputMapping} -> {outputMapping} and descriptor: {descriptor}");

                        analysisHelper.AddMethod(inputMapping, outputMapping, descriptor, false);
                    }
                    else if (intermediaryLine.StartsWith("FIELD"))
                    {
                        var intermediaryFieldData = intermediaryLine.Split('\t');
                        var type = intermediaryFieldData[2].Trim();
                        var inputMapping = intermediaryFieldData[3].Trim();
                        var outputMapping = intermediaryFieldData[4].Trim();

                        _logger.LogDebug(
                            $"Processing entry as field, with mapping: {inputMapping} -> {outputMapping}");

                        analysisHelper.AddField(inputMapping, outputMapping, false, type);
                    }
                },
                (count, current, percentage) =>
                {
                    _logger.LogInformation(
                        $"  > {percentage}% ({current}/{count}): Processing the {artifact.Name} intermediary file ...");
                }
            );

            foreach (var externalPackage in packages.Values)
            {
                release.Packages.Add(externalPackage);
            }

            return new ReadResult(
                release,
                new DeduplicationStrategies(
                    DeduplicationStrategy.OUTPUT_UNIQUE,
                    DeduplicationStrategy.BOTH_UNIQUE,
                    DeduplicationStrategy.OUTPUT_UNIQUE,
                    DeduplicationStrategy.OUTPUT_UNIQUE,
                    DeduplicationStrategy.OUTPUT_UNIQUE
                    ));
        }


        /// <summary>
        /// This class is a wrapper and utility handler that handles the conversion of a line based approach
        /// of the intermediary file format into the tree based structure that Mcms external data supports.
        ///
        /// It is specifically designed to handle intermediary data.
        /// </summary>
        private class IntermediaryAnalysisHelper
        {
            private readonly Dictionary<string, ExternalPackage> _packages;
            private ExternalClass _currentClass;

            public IntermediaryAnalysisHelper(ref Dictionary<string, ExternalPackage> packages)
            {
                _packages = packages;
            }

            public void AddClass(string inputMapping, string outputMapping, string packageName)
            {
                var package = _packages.GetOrAdd(packageName, () => new ExternalPackage
                {
                    Input = "",
                    Output = packageName
                });

                _currentClass = new ExternalClass
                {
                    Input = inputMapping,
                    Output = outputMapping
                };

                package.Classes.Add(_currentClass);
            }

            public void AddMethod(string inputMapping, string outputMapping, string descriptor, bool isStatic)
            {
                _currentClass.Methods.Add(new ExternalMethod
                {
                    Input = inputMapping,
                    Output = outputMapping,
                    Descriptor = descriptor,
                    IsStatic = isStatic
                });
            }

            public void AddField(string inputMapping, string outputMapping, bool isStatic, string type)
            {
                _currentClass.Fields.Add(new ExternalField
                {
                    Input = inputMapping,
                    Output = outputMapping,
                    IsStatic = isStatic,
                    Type = type
                });
            }
        }
    }
}
