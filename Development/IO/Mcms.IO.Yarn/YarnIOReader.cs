using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mcms.IO.Core;
using Mcms.IO.Core.Artifacts;
using Mcms.IO.Core.Extensions;
using Mcms.IO.Data;
using Microsoft.Extensions.Logging;

namespace Mcms.IO.Yarn
{
    /// <summary>
    /// The IOReader for the yarn file format.
    /// </summary>
    public class YarnIOReader
        : IIOReader
    {
        private readonly ILogger<YarnIOReader> _logger;

        public YarnIOReader(ILogger<YarnIOReader> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<ExternalRelease>> ReadAllFrom(IArtifactHandler handler)
        {
            _logger.LogDebug($"Importing all releases from: {handler}.");

            var releases = new List<ExternalRelease>();
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

        public async Task<ExternalRelease> ReadFrom(IArtifact artifact)
        {
            _logger.LogDebug($"Importing {artifact}...");
            var release = new ExternalRelease
            {
                Name = artifact.Name,
                GameVersion = artifact.GameVersion,
                MappingType = Constants.YARN_MAPPING_NAME
            };

            var packages = new Dictionary<string, ExternalPackage>();
            ZipArchive zip;
            try
            {
                zip = new ZipArchive(await artifact.GetStreamAsync());
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to download the yarn artifact data.");
                return release;
            }

            var yarnJoinedFileContents =
                zip.ReadAllLines(Constants.TINY_MAPPING_DATA, Encoding.UTF8).ToList();

            var analysisHelper = new YarnAnalysisHelper(ref packages);

            yarnJoinedFileContents.ForEachWithProgressCallback((yarnLine) =>
                {
                    _logger.LogDebug($"Processing yarn line: {yarnLine}");
                    if (yarnLine.StartsWith("CLASS"))
                    {
                        //New class
                        var yarnClassData = yarnLine.Split('\t');
                        var originalInputMapping = yarnClassData[0].Trim();
                        var inputMapping = yarnClassData[1].Trim();
                        var outputMappingIncludingPackage = yarnClassData[2].Trim();

                        var outputMapping =
                            outputMappingIncludingPackage.Substring(outputMappingIncludingPackage.LastIndexOf('/'));
                        var package = outputMappingIncludingPackage.Replace(outputMapping, "").Replace("/", ".");
                        outputMapping = outputMapping.Substring(1);

                        _logger.LogDebug(
                            $"Processing entry as class, with mapping: {inputMapping} ->{outputMapping} in package: {package}");

                        analysisHelper.AddClass(originalInputMapping, inputMapping, outputMapping, package);
                    }
                    else if (yarnLine.StartsWith("METHOD"))
                    {
                        //New method
                        var yarnMethodData = yarnLine.Trim().Split('\t');
                        var originalClassMapping = yarnMethodData[1].Trim();
                        var descriptor = yarnMethodData[2].Trim();
                        var inputMapping = yarnMethodData[4].Trim();
                        var outputMapping = yarnMethodData[5].Trim();

                        _logger.LogDebug(
                            $"Processing entry as method, with mapping: {inputMapping} -> {outputMapping} and descriptor: {descriptor}");

                        analysisHelper.AddMethod(originalClassMapping, inputMapping, outputMapping, descriptor, false);
                    }
                    else if (yarnLine.StartsWith("FIELD"))
                    {
                        var yarnFieldData = yarnLine.Split('\t');
                        var originalClassMapping = yarnFieldData[1].Trim();
                        var descriptor = yarnFieldData[2].Trim();
                        var inputMapping = yarnFieldData[4].Trim();
                        var outputMapping = yarnFieldData[5].Trim();

                        _logger.LogDebug(
                            $"Processing entry as field, with mapping: {inputMapping} -> {outputMapping}");

                        analysisHelper.AddField(originalClassMapping, inputMapping, outputMapping, false, descriptor);
                    }
                },
                (count, current, percentage) =>
                {
                    _logger.LogInformation(
                        $"  > {percentage}% ({current}/{count}): Processing the {artifact.Name} yarn file ...");
                }
            );

            foreach (var externalPackage in packages.Values)
            {
                release.Packages.Add(externalPackage);
            }

            return release;
        }


        /// <summary>
        /// This class is a wrapper and utility handler that handles the conversion of a line based approach
        /// of the yarn file format into the tree based structure that Mcms external data supports.
        ///
        /// It is specifically designed to handle yarn data.
        /// </summary>
        private class YarnAnalysisHelper
        {
            private readonly Dictionary<string, ExternalPackage> _packages;
            private readonly Dictionary<string, ExternalClass> _classes = new Dictionary<string, ExternalClass>();

            public YarnAnalysisHelper(ref Dictionary<string, ExternalPackage> packages)
            {
                _packages = packages;
            }

            public void AddClass(string originalMapping, string inputMapping, string outputMapping, string packageName)
            {
                var package = _packages.GetOrAdd(packageName, () => new ExternalPackage
                {
                    Input = "",
                    Output = packageName
                });

                var currentClass = new ExternalClass
                {
                    Input = inputMapping,
                    Output = outputMapping
                };

                package.Classes.Add(currentClass);
                _classes.Add(originalMapping, currentClass);
            }

            public void AddMethod(string partOfClassWithOriginalMapping, string inputMapping, string outputMapping, string descriptor, bool isStatic)
            {
                var currentClass = _classes.GetValueOrDefault(partOfClassWithOriginalMapping);
                if (currentClass == null)
                {
                    throw new ArgumentOutOfRangeException(partOfClassWithOriginalMapping, "The given original mapping of a class is unknown.");
                }

                currentClass.Methods.Add(new ExternalMethod
                {
                    Input = inputMapping,
                    Output = outputMapping,
                    Descriptor = descriptor,
                    IsStatic = isStatic
                });
            }

            public void AddField(string partOfClassWithOriginalMapping, string inputMapping, string outputMapping, bool isStatic, string type)
            {
                var currentClass = _classes.GetValueOrDefault(partOfClassWithOriginalMapping);
                if (currentClass == null)
                {
                    throw new ArgumentOutOfRangeException(partOfClassWithOriginalMapping, "The given original mapping of a class is unknown.");
                }

                currentClass.Fields.Add(new ExternalField
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
