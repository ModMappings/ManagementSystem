using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Mcms.IO.Core;
using Mcms.IO.Core.Artifacts;
using Mcms.IO.Data;
using Microsoft.Extensions.Logging;

namespace Mcms.IO.MCP
{
    public class MCPIOWriter : IIOWriter
    {

        private readonly ILogger<MCPIOWriter> _logger;

        public MCPIOWriter(ILogger<MCPIOWriter> logger)
        {
            _logger = logger;
        }

        public async Task WriteAll(IEnumerable<ExternalRelease> externalReleases, IArtifactHandler artifactHandler)
        {
            var releaseList = externalReleases.ToList();
            _logger.LogDebug($"Exporting {releaseList.Count()} releases to: {artifactHandler}.");

            foreach (var release in releaseList)
            {
                await WriteTo(release, await artifactHandler.CreateNewArtifactWithName(release.Name));
            }
        }

        public async Task WriteTo(ExternalRelease release, IArtifact artifact)
        {
            var methodLines = new LinkedList<string>();
            var fieldLines = new LinkedList<string>();
            var parameterLines = new LinkedList<string>();
            
            foreach (var externalPackage in release.Packages)
            {
                foreach (var externalClass in externalPackage.Classes)
                {
                    foreach (var externalMethod in externalClass.Methods)
                    {
                        methodLines.AddLast(
                            $"{externalMethod.Input},{externalMethod.Output},{GetDistributionIndexFromDist(externalMethod.Distribution)},{externalMethod.Documentation}");

                        foreach (var externalParameter in externalMethod.Parameters)
                        {
                            parameterLines.AddLast(
                                $"{externalParameter.Input},{externalParameter.Output},{GetDistributionIndexFromDist(externalParameter.Distribution)}");
                        }
                    }

                    foreach (var externalField in externalClass.Fields)
                    {
                        fieldLines.AddLast(
                            $"{externalField.Input},{externalField.Output},{GetDistributionIndexFromDist(externalField.Distribution)},{externalField.Documentation}");
                    }
                }
            }
            
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var methodsFile = archive.CreateEntry(Constants.MCP_METHODS_FILE);
                    var fieldsFile = archive.CreateEntry(Constants.MCP_FIELDS_FILE);
                    var parameterFile = archive.CreateEntry(Constants.MCP_PARAMS_FILE);

                    using (var entryStream = methodsFile.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        foreach (var methodLine in methodLines)
                        {
                            await streamWriter.WriteLineAsync(methodLine);
                        }
                    }
                    
                    using (var entryStream = fieldsFile.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        foreach (var fieldLine in fieldLines)
                        {
                            await streamWriter.WriteLineAsync(fieldLine);
                        }
                    }
                    
                    using (var entryStream = parameterFile.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        foreach (var parameterLine in parameterLines)
                        {
                            await streamWriter.WriteLineAsync(parameterLine);
                        }
                    }
                }

                await artifact.WriteStreamAsync(memoryStream.ToArray());
            }
        }

        private static int GetDistributionIndexFromDist(ExternalDistribution distribution)
        {
            switch (distribution)
            {
                case ExternalDistribution.BOTH:
                    return 0;
                case ExternalDistribution.SERVER_ONLY:
                    return 1;
                case ExternalDistribution.CLIENT_ONLY:
                    return 2;
                case ExternalDistribution.UNKNOWN:
                    return 3;
                default:
                    return 3;
            }
        }
    }
}