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
using Mcms.IO.Core.Protocol.Reading;
using Mcms.IO.Data;
using Microsoft.Extensions.Logging;

namespace Mcms.IO.TSRG
{
    /// <summary>
    /// The IOReader for the mcp config file format.
    /// </summary>
    public class MCPConfigIOReader
        : IIOReader
    {
        private readonly ILogger<MCPConfigIOReader> _logger;

        public MCPConfigIOReader(ILogger<MCPConfigIOReader> logger)
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
                MappingType = Constants.MCP_CONFIG_MAPPING_NAME
            };

            var packages = new Dictionary<string, ExternalPackage>();
            ZipArchive zip;
            try
            {
                zip = new ZipArchive(await artifact.GetStreamAsync());
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to download the MCPConfig Config artifact data.");
                return new ReadResult(
                    release,
                    new DeduplicationStrategies(
                        DeduplicationStrategy.OUTPUT_UNIQUE,
                        DeduplicationStrategy.BOTH_UNIQUE,
                        DeduplicationStrategy.OUTPUT_UNIQUE,
                        DeduplicationStrategy.OUTPUT_UNIQUE,
                        DeduplicationStrategy.OUTPUT_UNIQUE));
            }

            var mcpConfigJoinedFileContents =
                zip.ReadAllLines(Constants.TSRG_JOINED_DATA, Encoding.UTF8).ToList();
            var staticMethodsFileContents =
                zip.ReadAllLines(Constants.TSRG_STATIC_METHOD_DATA, Encoding.UTF8).ToDictionary(e => e, e => true);

            var analysisHelper = new MCPConfigAnalysisHelper(ref packages);

            mcpConfigJoinedFileContents.ForEachWithProgressCallback((mcpConfigLine) =>
                {
                    _logger.LogDebug($"Processing mCPConfig line: {mcpConfigLine}");
                    if (!mcpConfigLine.StartsWith('\t'))
                    {
                        //New class
                        var mcpConfigClassData = mcpConfigLine.Split(' ');
                        var inputMapping = mcpConfigClassData[0].Trim();
                        var outputMappingIncludingPackage = mcpConfigClassData[1].Trim();

                        var outputMapping =
                            outputMappingIncludingPackage.Substring(outputMappingIncludingPackage.LastIndexOf('/'));
                        var package = outputMappingIncludingPackage.Replace(outputMapping, "").Replace("/", ".");
                        outputMapping = outputMapping.Substring(1);

                        _logger.LogDebug(
                            $"Processing entry as class, with mapping: {inputMapping} ->{outputMapping} in package: {package}");

                        analysisHelper.AddClass(inputMapping, outputMapping, package);
                    }
                    else if (mcpConfigLine.StartsWith('('))
                    {
                        //New method
                        var mcpConfigMethodData = mcpConfigLine.Trim().Split(' ');
                        var inputMapping = mcpConfigMethodData[0].Trim();
                        var descriptor = mcpConfigMethodData[1].Trim();
                        var outputMapping = mcpConfigMethodData[2].Trim();
                        var isStatic = staticMethodsFileContents.GetValueOrDefault(outputMapping, false);

                        _logger.LogDebug(
                            $"Processing entry as method, with mapping: {inputMapping} -> {outputMapping} and descriptor: {descriptor}");

                        analysisHelper.AddMethod(inputMapping, outputMapping, descriptor, isStatic);
                    }
                    else
                    {
                        var mcpConfigFieldData = mcpConfigLine.Split(' ');
                        var inputMapping = mcpConfigFieldData[0].Trim();
                        var outputMapping = mcpConfigFieldData[1].Trim();

                        _logger.LogDebug(
                            $"Processing entry as field, with mapping: {inputMapping} -> {outputMapping}");

                        analysisHelper.AddField(inputMapping, outputMapping, false, "");
                    }
                },
                (count, current, percentage) =>
                {
                    _logger.LogInformation(
                        $"  > {percentage}% ({current}/{count}): Processing the {artifact.Name} MCPConfig file ...");
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
                    DeduplicationStrategy.OUTPUT_UNIQUE));
        }


        /// <summary>
        /// This class is a wrapper and utility handler that handles the conversion of a line based approach
        /// of the mCPConfig file format into the tree based structure that Mcms external data supports.
        ///
        /// It is specifically designed to handle mCPConfig data.
        /// </summary>
        private class MCPConfigAnalysisHelper
        {
            private readonly Dictionary<string, ExternalPackage> _packages;
            private ExternalClass _currentClass;

            public MCPConfigAnalysisHelper(ref Dictionary<string, ExternalPackage> packages)
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
