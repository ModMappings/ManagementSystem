using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Core.Release;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Component;
using Data.Core.Models.Mapping.Mappings;
using Data.EFCore.Context;
using Data.MCP.TSRG.Importer.Extensions;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Data.MCP.TSRG.Importer
{
    public class MCPImportHandler : IDataImportHandler
    {
        private readonly ILogger<MCPImportHandler> _logger;

        private readonly string _mcpType;

        public MCPImportHandler(ILogger<MCPImportHandler> logger, string mcpType)
        {
            _logger = logger;
            _mcpType = mcpType;
        }

        public async Task Import(MCMSContext context, IConfiguration configuration)
        {
            _logger.LogInformation(
                $"Attempting import of MCP data into context with database name: {context.Database.GetDbConnection().Database} from type: {_mcpType}");

            var tsrgMappingType = await GetOrCreateTSRGMappingType(context);
            var mcpMappingType = await GetOrCreateMCPMappingType(context);

            var mcpConfigProject =
                MavenProject.Create(Constants.FORGE_MAVEN_URL, Constants.MCP_GROUP, _mcpType);

            var mcpConfigArtifacts = await mcpConfigProject.GetArtifacts();

            _logger.LogInformation($"{mcpConfigArtifacts.Count} {_mcpType} versions are available.");

            var mcpVersionsInfo = await GetMCPVersionsInformation();
            var mcVersions = await DetermineMCVersionsFromMCPVersionInfoReleases(mcpVersionsInfo);

            _logger.LogInformation($"{mcVersions.Count()} Minecraft versions are available.");

            var gameVersions = await GetOrCreateGameVersionsForMCVersions(context, mcVersions);
            var releases =
                await DetermineReleasesToImport(context, mcpConfigArtifacts, mcpVersionsInfo, gameVersions, mcpMappingType);

            if (!releases.Any())
            {
                _logger.LogWarning($"No new {_mcpType} data to import.");
                return;
            }

            _logger.LogWarning($"Importing: {releases.Count} new {_mcpType} releases");
            _logger.LogInformation($"Importing the following {_mcpType} releases:");
            foreach (var releaseName in releases.Keys)
            {
                _logger.LogInformation($"  > {releaseName}");
            }

            _logger.LogDebug("Saving newly created data.");
            await SaveInitialData(context, gameVersions.Values, releases.Values, mcpMappingType);
            _logger.LogInformation("Saved initial import data into database. Attempting data import.");

            await Task.WhenAll(releases.Keys.Select(r =>
            {
                return Task.Run(() =>
                {
                    var methodMappings = ProcessMCPArtifactForData(mcpConfigArtifacts[r], releases, mcpMappingType,
                        Constants.MCP_METHODS_DATA, ComponentType.METHOD).Result;

                    DetermineCrossVersionHistoryFromLiveMappings(context, methodMappings, tsrgMappingType).ConfigureAwait(false);

                    var fieldMappings = ProcessMCPArtifactForData(mcpConfigArtifacts[r], releases, mcpMappingType,
                        Constants.MCP_FIELDS_DATA, ComponentType.FIELD).Result;

                    DetermineCrossVersionHistoryFromLiveMappings(context, fieldMappings, tsrgMappingType).ConfigureAwait(false);
                });
            }));

            await context.SaveChangesAsync();
        }

        private static async Task<MappingType> GetOrCreateTSRGMappingType(MCMSContext context) =>
            await context.MappingTypes.FirstOrDefaultAsync(m => m.Name == Constants.OBF_TO_TSRG_MAPPING_NAME) ??
            await CreateTSRGMappingType();

        private static Task<MappingType> CreateTSRGMappingType() =>
            Task.FromResult(new MappingType()
            {
                Id = Guid.NewGuid(),
                CreatedBy = Guid.Empty,
                CreatedOn = DateTime.Now,
                Name = Constants.OBF_TO_TSRG_MAPPING_NAME,
                Releases = new List<Release>()
            });

        private static async Task<MappingType> GetOrCreateMCPMappingType(MCMSContext context) =>
            await context.MappingTypes.FirstOrDefaultAsync(m => m.Name == Constants.TSRG_TO_MCP_MAPPING_NAME) ??
            await CreateMCPMappingType();

        private static Task<MappingType> CreateMCPMappingType() =>
            Task.FromResult(new MappingType()
            {
                Id = Guid.NewGuid(),
                CreatedBy = Guid.Empty,
                CreatedOn = DateTime.Now,
                Name = Constants.TSRG_TO_MCP_MAPPING_NAME,
                Releases = new List<Release>()
            });

        private static async Task<Dictionary<string, MCPDataVersionConfiguration>> GetMCPVersionsInformation() =>
            await Constants.MCP_VERSION_INFO_URL.GetJsonAsync<Dictionary<string, MCPDataVersionConfiguration>>();

        private static Task<List<string>> DetermineMCVersionsFromMCPVersionInfoReleases(
            Dictionary<string, MCPDataVersionConfiguration> mcpDataVersionConfigurations) =>
            Task.FromResult(mcpDataVersionConfigurations.Keys.ToList());

        private static async Task<Dictionary<string, GameVersion>> GetOrCreateGameVersionsForMCVersions(
            MCMSContext context,
            IEnumerable<string> mcVersions)
        {
            var newMcVersions = mcVersions.Except(await context.GameVersions.Select(g => g.Name).ToListAsync());
            var existingGameVersions = await context.GameVersions.ToListAsync();
            var newGameVersions = newMcVersions.Select(mcVersion => new GameVersion()
            {
                CreatedBy = Guid.Empty,
                CreatedOn = DateTime.Now,
                Id = Guid.NewGuid(),
                IsPreRelease = false,
                IsSnapshot = false,
                Name = mcVersion
            });

            return existingGameVersions.Union(newGameVersions).ToDictionary(gv => gv.Name);
        }

        private static async Task<Dictionary<string, Release>> DetermineReleasesToImport(
            MCMSContext context,
            IReadOnlyDictionary<string, MavenArtifact> mcpMappingArtifacts,
            IReadOnlyDictionary<string, MCPDataVersionConfiguration> mcpVersionInfo,
            IReadOnlyDictionary<string, GameVersion> gameVersions,
            MappingType mcpMappingType) =>
            mcpMappingArtifacts.Keys
                .Except(await context.Releases.Select(r => r.Name).ToListAsync())
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => new Release()
                {
                    Components = new List<ReleaseComponent>(),
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    GameVersion = gameVersions[mcpVersionInfo.Where(kvpData => kvpData.Value.Snapshot.Contains(v.Split("-")[0]) || kvpData.Value.Stable.Contains(v.Split("-")[0])).Select(kvpData => kvpData.Key).FirstOrDefault()],
                    Id = Guid.NewGuid(),
                    MappingType = mcpMappingType,
                    Name = v,
                    IsSnapshot = mcpVersionInfo.Any(kvpData => kvpData.Value.Snapshot.Contains(v.Split("-")[0]))
                })
                .ToDictionary(r => r.Name);

        private static async Task SaveInitialData(MCMSContext context, IEnumerable<GameVersion> gameVersions,
            IEnumerable<Release> releases, MappingType tsrgMappingType)
        {
            await context.GameVersions.AddRangeAsync(gameVersions);
            await context.Releases.AddRangeAsync(releases);
            await context.MappingTypes.AddAsync(tsrgMappingType);
        }

        private async Task<IEnumerable<CommittedMapping>> ProcessMCPArtifactForData(
            MavenArtifact mcpArtifact,
            IReadOnlyDictionary<string, Release> releases,
            MappingType mcpMappingType,
            string mcpArtifactDataType,
            ComponentType type
        )
        {
            _logger.LogInformation($"Processing: {mcpArtifact.Version}");
            var release = releases[mcpArtifact.Version];
            var gameVersion = release.GameVersion;

            var zip = new ZipArchive(await mcpArtifact.Path.GetStreamAsync());
            var artifactEntries = zip.ReadAllLines(mcpArtifactDataType, Encoding.UTF8).Skip(1).ToList();

            _logger.LogInformation($"Found: {artifactEntries.Count} entries in {mcpArtifactDataType}");

            var mappingEntries = new List<CommittedMapping>();
            var totalLineCount = artifactEntries.Count;
            var currentlyProcessed = 0d;
            var currentPercentage = -1d;

            foreach (var entry in artifactEntries)
            {
                currentlyProcessed += 1;
                var newPercentage = Math.Floor(currentlyProcessed / totalLineCount * 100);
                if (newPercentage > currentPercentage)
                {
                    currentPercentage = newPercentage;
                    _logger.LogInformation(
                        $"Processed: {currentPercentage}% of the lines in the {mcpArtifact.Version} {_mcpType} file.");
                }

                var entryData = entry.Split(";");

                var mappingEntry = new CommittedMapping()
                {
                    CreatedOn = DateTime.Now,
                    Documentation = entryData[3],
                    Distribution = (Distribution) int.Parse(entryData[2]),
                    Id = Guid.NewGuid(),
                    InputMapping = entryData[0],
                    OutputMapping = entryData[1],
                    ProposedMapping = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = null,
                    MappingType = mcpMappingType
                };

                mappingEntry.Releases.Add(new ReleaseComponent()
                {
                    Id = Guid.NewGuid(),
                    Member = mappingEntry,
                    Release = release,
                    ComponentType = type
                });

                mappingEntries.Add(mappingEntry);
            }

            return mappingEntries;
        }

        private async Task DetermineCrossVersionHistoryFromLiveMappings(
            MCMSContext context,
            IEnumerable<CommittedMapping> liveMappingEntries,
            MappingType tsrgMappingType)
        {
            foreach (var liveMappingEntry in liveMappingEntries)
            {
                var versionedComponent = await context.VersionedComponents.FirstOrDefaultAsync(vc =>
                    vc.Mappings.Any(lm =>
                        lm.OutputMapping == liveMappingEntry.InputMapping && lm.MappingType == tsrgMappingType));

                if (versionedComponent == null)
                {
                    _logger.LogInformation(
                        $"Missing versioned component of type: {_mcpType} with mapping: {liveMappingEntry.InputMapping} -> {liveMappingEntry.OutputMapping}");
                    continue;
                }

                versionedComponent.Mappings.Add(liveMappingEntry);
                liveMappingEntry.VersionedComponent = versionedComponent;

                context.Update(versionedComponent);
            }
        }
    }
}
