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
using Mcms.IO.Maven.Extensions;
using Mcms.IO.TSRG;
using Mcms.IO.TSRG.Maven;
using Microsoft.Extensions.Logging;

namespace Mcms.IO.MCP
{
    public class MCPIOReader : IIOReader
    {

        private readonly ILogger<MCPIOReader> _logger;
        private readonly MCPConfigIOReader _configIOReader;

        public MCPIOReader(ILogger<MCPIOReader> logger, MCPConfigIOReader configIoReader)
        {
            _logger = logger;
            _configIOReader = configIoReader;
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
                MappingType = Constants.MCP_MAPPING_NAME
            };

            var packages = new Dictionary<string, ExternalPackage>();
            ZipArchive zip;
            try
            {
                zip = new ZipArchive(await artifact.GetStreamAsync());
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to download the MCP artifact data.");
                return release;
            }

            var methodsFileContents =
                zip.ReadAllLines(Constants.MCP_METHODS_FILE, Encoding.UTF8).ToList();
            var fieldsFileContents =
                zip.ReadAllLines(Constants.MCP_FIELDS_FILE, Encoding.UTF8).ToList();
            var paramsFileContents =
                zip.ReadAllLines(Constants.MCP_PARAMS_FILE, Encoding.UTF8).ToList();

            var mcpConfigArtifact =
                (await MCPConfigMavenProject.Instance.GetArtifactsForGameVersion(release.GameVersion)).FirstOrDefault();
            if (mcpConfigArtifact == null)
            {
                _logger.LogCritical("Failed to find a matching MCP Config artifact that matches the MCP Game Version");
                return release;
            }

            var analysisHelper = new MCPAnalysisHelper(await _configIOReader.ReadFrom(mcpConfigArtifact), ref packages);

            methodsFileContents.ForEachWithProgressCallback((mcpConfigLine) =>
                {
                    _logger.LogDebug($"Processing MCP Method line: {mcpConfigLine}");
                    var methodData = mcpConfigLine.Split(",");
                    analysisHelper.AddMethod(methodData[0], methodData[1], GetDistFromString(methodData[2]), methodData[3]);
                },
                (count, current, percentage) =>
                {
                    _logger.LogInformation(
                        $"  > {percentage}% ({current}/{count}): Processing the {artifact.Name} methods file ...");
                }
            );

            fieldsFileContents.ForEachWithProgressCallback((mcpConfigLine) =>
                {
                    _logger.LogDebug($"Processing MCP Field line: {mcpConfigLine}");
                    var fieldData = mcpConfigLine.Split(",");
                    analysisHelper.AddField(fieldData[0], fieldData[1], GetDistFromString(fieldData[2]), fieldData[3]);
                },
                (count, current, percentage) =>
                {
                    _logger.LogInformation(
                        $"  > {percentage}% ({current}/{count}): Processing the {artifact.Name} fields file ...");
                }
            );

            paramsFileContents.ForEachWithProgressCallback((mcpConfigLine) =>
                {
                    _logger.LogDebug($"Processing MCP Param line: {mcpConfigLine}");
                    var paramData = mcpConfigLine.Split(",");
                    analysisHelper.AddParameter(paramData[0], paramData[1], GetDistFromString(paramData[2]), "");
                },
                (count, current, percentage) =>
                {
                    _logger.LogInformation(
                        $"  > {percentage}% ({current}/{count}): Processing the {artifact.Name} params file ...");
                }
            );

            foreach (var externalPackage in packages.Values)
            {
                release.Packages.Add(externalPackage);
            }

            return release;
        }

        private static ExternalDistribution GetDistFromString(string dist)
        {
            if (dist == null || dist.Trim().Any())
            {
                return ExternalDistribution.UNKNOWN;
            }

            if (!int.TryParse(dist, out var distIndex)) return ExternalDistribution.UNKNOWN;
            switch (distIndex)
            {
                case 0:
                    return ExternalDistribution.BOTH;
                case 1:
                    return ExternalDistribution.SERVER_ONLY;
                case 2:
                    return ExternalDistribution.CLIENT_ONLY;
                default:
                    return ExternalDistribution.UNKNOWN;
            }
        }

        /// <summary>
        /// This class is a wrapper and utility handler that handles the conversion of a line based approach
        /// of the mcp file formats into the tree based structure that Mcms external data supports.
        ///
        /// It is specifically designed to handle mcp data.
        /// </summary>
        private class MCPAnalysisHelper
        {
            private readonly ExternalRelease _mcpConfigRelease;
            private readonly Dictionary<string, ExternalPackage> _packages;

            private readonly Dictionary<string, ExternalClass> _srgMethodToParentClassMapping = new Dictionary<string, ExternalClass>();
            private readonly Dictionary<string, ExternalClass> _srgFieldToParentClassMapping = new Dictionary<string, ExternalClass>();
            private readonly Dictionary<string, ExternalMethod> _srgMethodToMcpConfigMethodMapping = new Dictionary<string, ExternalMethod>();
            private readonly Dictionary<string, ExternalField> _srgFieldToMcpConfigFieldMapping = new Dictionary<string, ExternalField>();
            private readonly Dictionary<string, ExternalMethod> _srgMethodIdToMethodMapping = new Dictionary<string, ExternalMethod>();

            public MCPAnalysisHelper(ExternalRelease mcpConfigRelease, ref Dictionary<string, ExternalPackage> packages)
            {
                _mcpConfigRelease = mcpConfigRelease;
                _packages = packages;

                this.BuildMappingStructureData();
            }

            public void AddMethod(string inputMapping, string outputMapping, ExternalDistribution distribution, string documentation)
            {
                var parentClass = _srgMethodToParentClassMapping[inputMapping];
                if (parentClass == null)
                    throw new ArgumentOutOfRangeException(nameof(inputMapping),
                        $"No MCP Config class found for SRG Method mapping: {inputMapping}");

                var mcpConfigMethod = _srgMethodToMcpConfigMethodMapping[inputMapping];
                if (mcpConfigMethod == null)
                    throw new ArgumentOutOfRangeException(nameof(inputMapping),
                        $"No MCP Config method found for SRG Method mapping: {inputMapping}");

                var externalMethod = new ExternalMethod()
                {
                    Input = inputMapping,
                    Output = outputMapping,
                    Descriptor = mcpConfigMethod.Descriptor,
                    Distribution = distribution,
                    Documentation = documentation,
                    IsStatic = mcpConfigMethod.IsStatic
                };

                parentClass.Methods.Add(externalMethod);
                _srgMethodIdToMethodMapping.Add(GetFuncIdFromMethodName(inputMapping), externalMethod);
            }

            public void AddField(string inputMapping, string outputMapping, ExternalDistribution distribution, string documentation)
            {
                var parentClass = _srgFieldToParentClassMapping[inputMapping];
                if (parentClass == null)
                    throw new ArgumentOutOfRangeException(nameof(inputMapping),
                        $"No MCP Config class found for SRG Field mapping: {inputMapping}");

                var mcpConfigField = _srgFieldToMcpConfigFieldMapping[inputMapping];
                if (mcpConfigField == null)
                    throw new ArgumentOutOfRangeException(nameof(inputMapping),
                        $"No MCP Config field found for SRG Field mapping: {inputMapping}");

                var externalField = new ExternalField()
                {
                    Input = inputMapping,
                    Output = outputMapping,
                    Type = mcpConfigField.Type,
                    Distribution = distribution,
                    Documentation = documentation,
                    IsStatic = mcpConfigField.IsStatic
                };

                parentClass.Fields.Add(externalField);
            }

            public void AddParameter(string inputMapping, string outputMapping, ExternalDistribution distribution,
                string documentation)
            {
                var parentMethod = _srgMethodIdToMethodMapping[GetFuncIdFromParameterName(inputMapping)];
                if (parentMethod == null)
                    throw new ArgumentOutOfRangeException(nameof(inputMapping),
                        $"Could not find a MCP Method mapping to which the parameter {inputMapping} belongs.");

                var externalParameter = new ExternalParameter()
                {
                    Input = inputMapping,
                    Output = outputMapping,
                    Distribution = distribution,
                    Documentation = documentation
                };

                parentMethod.Parameters.Add(externalParameter);
            }

            private void BuildMappingStructureData()
            {
                foreach (var externalMcpConfigPackage in _mcpConfigRelease.Packages)
                {
                    var externalPackage = new ExternalPackage()
                    {
                        Input = externalMcpConfigPackage.Output,
                        Output = externalMcpConfigPackage.Output,
                        Distribution = externalMcpConfigPackage.Distribution,
                        Documentation = externalMcpConfigPackage.Documentation
                    };

                    _packages.Add(externalPackage.Output, externalPackage);

                    foreach (var externalMcpConfigClass in externalMcpConfigPackage.Classes)
                    {
                        var externalClass = new ExternalClass()
                        {
                            Input = externalMcpConfigClass.Output,
                            Output = externalMcpConfigClass.Output,
                            Distribution = externalMcpConfigClass.Distribution,
                            Documentation = externalMcpConfigClass.Documentation
                        };

                        externalPackage.Classes.Add(externalClass);

                        foreach (var externalMcpConfigMethod in externalMcpConfigClass.Methods)
                        {
                            _srgMethodToParentClassMapping.Add(externalMcpConfigMethod.Output, externalClass);
                            _srgMethodToMcpConfigMethodMapping.Add(externalMcpConfigMethod.Output, externalMcpConfigMethod);
                        }
                        foreach (var externalMcpConfigField in externalMcpConfigClass.Fields)
                        {
                            _srgFieldToParentClassMapping.Add(externalMcpConfigField.Output, externalClass);
                            _srgFieldToMcpConfigFieldMapping.Add(externalMcpConfigField.Output, externalMcpConfigField);
                        }
                    }
                }
            }

            private string GetFuncIdFromMethodName(string srgMethodName)
            {
                return srgMethodName.Replace("func_", "").Split("_")[0];
            }

            private string GetFuncIdFromParameterName(string srgParameterName)
            {
                return srgParameterName.Replace("p_", "").Split("_")[0];
            }
        }
    }
}
