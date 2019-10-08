using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Core;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Mcms.Api.Data.Poco.Models.Mapping;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Data.EFCore.Context;
using Data.FabricImporter.Extensions;
using Data.FabricImporter.Maven;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Data.FabricImporter.Intermediary
{
    public class IntermediaryImportHandler
        : IDataImportHandler
    {
        private readonly ILogger<IntermediaryImportHandler> _logger;

        public IntermediaryImportHandler(ILogger<IntermediaryImportHandler> logger)
        {
            _logger = logger;
        }

        public async Task Import(MCMSContext context, IConfiguration configuration)
        {
            _logger.LogInformation(
                $"Attempting import of Intermediary data into context with database name: {context.Database.GetDbConnection().Database}");

            var intermediaryMappingType = await GetOrCreateIntermediaryMappingType(context);

            var intermediaryConfigProject =
                MavenProject.Create(Constants.FABRIC_MAVEN_URL, Constants.FABRIC_GROUP, Constants.INTERMEDIARY_PROJECT_NAME);

            var intermediaryConfigArtifacts = await intermediaryConfigProject.GetArtifacts();

            _logger.LogInformation($"{intermediaryConfigArtifacts.Count} Intermediary Config versions are available.");

            var mcVersions = await DetermineMCVersionsFromIntermediaryReleases(intermediaryConfigArtifacts);

            _logger.LogInformation($"{mcVersions.Count} Minecraft versions are available.");

            var gameVersions = await GetOrCreateGameVersionsForMCVersions(context, mcVersions);
            var releases =
                await DetermineIntermediaryReleasesToImport(context, intermediaryConfigArtifacts, gameVersions, intermediaryMappingType);

            if (!releases.Any())
            {
                _logger.LogWarning("No new Intermediary Config data to import.");
                return;
            }

            _logger.LogWarning("Importing: " + releases.Count + " new Intermediary config releases");
            _logger.LogInformation("Importing the following Intermediary Config releases:");
            foreach (var releaseName in releases.Keys)
            {
                _logger.LogInformation($"  > {releaseName}");
            }

            _logger.LogWarning("Saving initial data.");
            await SaveInitialData(context, releases.Values, gameVersions.Values, intermediaryMappingType);

            var newClassData = new List<Component>();

            foreach (var releasesKey in releases.Keys)
            {
                await ProcessIntermediaryArtifact(
                    intermediaryConfigArtifacts[releasesKey],
                    releases,
                    intermediaryMappingType,
                    context,
                    newClassData
                );
            }

            await SaveData(context, newClassData);
        }

        private static async Task<MappingType> GetOrCreateIntermediaryMappingType(MCMSContext context) =>
            await context.MappingTypes.FirstOrDefaultAsync(m => m.Name == Constants.INTERMEDIARY_MAPPING_NAME) ??
            await CreateIntermediaryMappingType();

        private static Task<MappingType> CreateIntermediaryMappingType() =>
            Task.FromResult(new MappingType()
            {
                Id = Guid.NewGuid(),
                CreatedBy = Guid.Empty,
                CreatedOn = DateTime.Now,
                Name = Constants.INTERMEDIARY_MAPPING_NAME,
                Releases = new List<Release>()
            });

        private static Task<List<string>> DetermineMCVersionsFromIntermediaryReleases(
            Dictionary<string, MavenArtifact> intermediaryConfigArtifacts) =>
            Task.FromResult(intermediaryConfigArtifacts.Keys
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct()
                .ToList());

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
                IsPreRelease = mcVersion.ToLower().Contains("Pre"),
                IsSnapshot = mcVersion.ToLower().Contains("w") || mcVersion.ToLower().Contains("d"),
                Name = mcVersion
            });

            return existingGameVersions.Union(newGameVersions).ToDictionary(gv => gv.Name);
        }

        private static async Task<Dictionary<string, Release>> DetermineIntermediaryReleasesToImport(
            MCMSContext context,
            IReadOnlyDictionary<string, MavenArtifact> intermediaryConfigArtifacts,
            IReadOnlyDictionary<string, GameVersion> gameVersions,
            MappingType intermediaryMappingType) =>
            intermediaryConfigArtifacts.Keys
                .Except(await context.Releases.Select(r => r.Name).ToListAsync())
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => new Release()
                {
                    Components = new List<ReleaseComponent>(),
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    GameVersion = gameVersions[v],
                    Id = Guid.NewGuid(),
                    MappingType = intermediaryMappingType,
                    Name = v
                })
                .ToDictionary(r => r.Name);

        private async Task SaveInitialData(
            MCMSContext context,
            IEnumerable<Release> releasesToSave,
            IEnumerable<GameVersion> gameVersionsToSave,
            MappingType intermediaryMappingTypeToSave)
        {
            foreach (var release in releasesToSave)
            {
                var releaseEntityEntry = context.Entry(release);
                if (releaseEntityEntry.State == EntityState.Detached)
                    releaseEntityEntry.State = EntityState.Added;
            }

            foreach (var gameVersion in gameVersionsToSave)
            {
                var gameVersionEntityEntry = context.Entry(gameVersion);
                if (gameVersionEntityEntry.State == EntityState.Detached)
                    gameVersionEntityEntry.State = EntityState.Added;
            }

            var mappingTypeEntityEntry = context.Entry(intermediaryMappingTypeToSave);
            if (mappingTypeEntityEntry.State == EntityState.Detached)
                mappingTypeEntityEntry.State = EntityState.Added;

            await context.SaveChangesAsync();
        }

        private async Task ProcessIntermediaryArtifact(
            MavenArtifact intermediaryConfigArtifact,
            IReadOnlyDictionary<string, Release> releases,
            MappingType intermediaryMappingType,
            MCMSContext context,
            List<Component> newClassData)
        {
            _logger.LogInformation($"Processing: {intermediaryConfigArtifact.Version}");
            var release = releases[intermediaryConfigArtifact.Version];
            var gameVersion = release.GameVersion;

            ZipArchive zip;
            try
            {
                zip = new ZipArchive(await intermediaryConfigArtifact.Path.GetStreamAsync());
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to download the Intermediary Config artifact data.");
                return;
            }

            var intermediaryJoinedFileContents = zip.ReadAllLines(Constants.INTERMEDIARY_JOINED_DATA, Encoding.UTF8).ToList();

            _logger.LogInformation($"Found: {intermediaryJoinedFileContents.Count} entries in the intermediary information.");

            var analysisHelper =
                new IntermediaryAnalysisHelper(context, ref newClassData, release, gameVersion, intermediaryMappingType);

            await intermediaryJoinedFileContents.ForEachWithProgressCallback(async (intermediaryLine) =>
                {
                    _logger.LogDebug($"Processing intermediary line: {intermediaryLine}");
                    if (intermediaryLine.StartsWith("CLASS"))
                    {
                        analysisHelper.FinalizeCurrentClass();

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

                        await analysisHelper.StartNewClass(inputMapping, outputMapping, package);
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
                        var inputMapping = intermediaryFieldData[3].Trim();
                        var outputMapping = intermediaryFieldData[4].Trim();

                        _logger.LogDebug(
                            $"Processing entry as field, with mapping: {inputMapping} -> {outputMapping}");

                        analysisHelper.AddField(inputMapping, outputMapping, false);
                    }
                },
                (count, current, percentage) =>
                {
                    _logger.LogInformation(
                        $"  > {percentage}% ({current}/{count}): Processing the {intermediaryConfigArtifact.Version} intermediary file ...");
                });
        }

        private async Task SaveData(MCMSContext context, List<Component> classes)
        {
            _logger.LogWarning(
                $"Saving: {classes.Count} new classes");

            await context.SaveChangesAsync();

            _logger.LogWarning("Intermediary Import saved.");
        }
    }
}
