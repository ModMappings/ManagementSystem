using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.EFCore.Context;
using Data.MCPImport.Extensions;
using Data.MCPImport.Maven;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace Data.MCPImport.TSRG
{
    public class TSRGImportHandler
        : IDataImportHandler
    {
        private readonly ILogger<TSRGImportHandler> _logger;

        public TSRGImportHandler(ILogger<TSRGImportHandler> logger)
        {
            _logger = logger;
        }

        public async Task Import(MCMSContext context)
        {
            _logger.LogInformation(
                $"Attempting import of TSRG data into context with database name: {context.Database.GetDbConnection().Database}");

            var tsrgMappingType = await GetOrCreateTSRGMappingType(context);

            var mcpConfigProject =
                MavenProject.Create(Constants.FORGE_MAVEN_URL, Constants.MCP_GROUP, Constants.MCP_CONFIG_NAME);

            var mcpConfigArtifacts = await mcpConfigProject.GetArtifacts();

            _logger.LogInformation($"{mcpConfigArtifacts.Count} MCP Config versions are available.");

            var mcVersions = await DetermineMCVersionsFromMCPConfigReleases(mcpConfigArtifacts);

            _logger.LogInformation($"{mcVersions.Count()} Minecraft versions are available.");

            var gameVersions = await GetOrCreateGameVersionsForMCVersions(context, mcVersions);
            var releases =
                await DetermineMCPConfigReleasesToImport(context, mcpConfigArtifacts, gameVersions, tsrgMappingType);

            if (!releases.Any())
            {
                _logger.LogWarning("No new MCP Config data to import.");
                return;
            }

            releases = releases.Take(1).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            _logger.LogWarning("Importing: " + releases.Count + " new MCP config releases");
            _logger.LogInformation("Importing the following MCP Config releases:");
            foreach (var releaseName in releases.Keys)
            {
                _logger.LogInformation($"  > {releaseName}");
            }

            _logger.LogDebug("Saving newly created data.");
            //await SaveInitialData(context, gameVersions.Values, releases.Values, tsrgMappingType);
            _logger.LogInformation("Saved initial import data into database. Attempting data import.");

            var mcVersionClassVersionMappings = (await Task.WhenAll(releases.Keys.Select(r =>
            {
                return Task.Run(() =>
                    ProcessMCPConfigArtifactForData(mcpConfigArtifacts[r], releases, tsrgMappingType));
            }))).ToList();

            var classes =
                await DetermineCrossVersionClassHistory(context, tsrgMappingType, mcVersionClassVersionMappings);
            var methods =
                await DetermineCrossVersionMethodHistory(context, tsrgMappingType, classes);
            var fields =
                await DetermineCrossVersionFieldHistory(context, tsrgMappingType, classes);

            await SaveData(context, classes, methods, fields);
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

        private static Task<List<string>> DetermineMCVersionsFromMCPConfigReleases(
            Dictionary<string, MavenArtifact> mcpConfigArtifacts) =>
            Task.FromResult(mcpConfigArtifacts.Keys
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => v.Split("-")[0])
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
                IsPreRelease = false,
                IsSnapshot = false,
                Name = mcVersion
            });

            return existingGameVersions.Union(newGameVersions).ToDictionary(gv => gv.Name);
        }

        private static async Task<Dictionary<string, Release>> DetermineMCPConfigReleasesToImport(
            MCMSContext context,
            IReadOnlyDictionary<string, MavenArtifact> mcpConfigArtifacts,
            IReadOnlyDictionary<string, GameVersion> gameVersions,
            MappingType tsrgMappingType) =>
            mcpConfigArtifacts.Keys
                .Except(await context.Releases.Select(r => r.Name).ToListAsync())
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => new Release()
                {
                    Components = new List<ReleaseComponent>(),
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    GameVersion = gameVersions[v.Split("-")[0]],
                    Id = Guid.NewGuid(),
                    MappingType = tsrgMappingType,
                    Name = v
                })
                .ToDictionary(r => r.Name);

        private static async Task SaveInitialData(MCMSContext context, IEnumerable<GameVersion> gameVersions,
            IEnumerable<Release> releases, MappingType tsrgMappingType)
        {
            await context.GameVersions.AddRangeAsync(gameVersions);
            await context.Releases.AddRangeAsync(releases);
            await context.MappingTypes.AddAsync(tsrgMappingType);
        }

        private async Task<IEnumerable<VersionedComponent>> ProcessMCPConfigArtifactForData(
            MavenArtifact mcpConfigArtifact,
            IReadOnlyDictionary<string, Release> releases,
            MappingType tsrgMappingType
        )
        {
            _logger.LogInformation($"Processing: {mcpConfigArtifact.Version}");
            var release = releases[mcpConfigArtifact.Version];
            var gameVersion = release.GameVersion;

            ZipArchive zip;
            try
            {
                zip = new ZipArchive(await mcpConfigArtifact.Path.GetStreamAsync());
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to download the MCP Config artifact data.");
                return new List<VersionedComponent>();
            }

            var tsrgJoinedFileContents = zip.ReadAllLines(Constants.TSRG_JOINED_DATA, Encoding.UTF8).ToList();
            var staticMethodsFileContents = zip.ReadAllLines(Constants.TSRG_STATIC_METHOD_DATA, Encoding.UTF8).ToList();

            _logger.LogInformation($"Found: {tsrgJoinedFileContents.Count()} entries in the tsrg information.");

            var classVersionedComponents = new List<VersionedComponent>();
            VersionedComponent currentClass = null;

            var totalLineCount = tsrgJoinedFileContents.Count;
            var currentlyProcessed = 0d;
            var currentPercentage = -1d;

            foreach (var tsrgLine in tsrgJoinedFileContents)
            {
                currentlyProcessed += 1;
                var newPercentage = Math.Floor(currentlyProcessed / totalLineCount * 100);
                if (newPercentage > currentPercentage)
                {
                    currentPercentage = newPercentage;
                    _logger.LogInformation(
                        $"Processed: {currentPercentage}% of the lines in the {mcpConfigArtifact.Version} tsrg file.");
                }

                _logger.LogDebug($"Processing tsrg line: {tsrgLine}");
                if (!tsrgLine.StartsWith('\t'))
                {
                    //Register previous class:
                    if (currentClass != null)
                    {
                        classVersionedComponents.Add(currentClass);
                        currentClass = null;
                    }

                    //New class
                    var tsrgClassData = tsrgLine.Split(' ');
                    var inputMapping = tsrgClassData[0].Trim();
                    var outputMappingIncludingPackage = tsrgClassData[1].Trim();

                    var outputMapping =
                        outputMappingIncludingPackage.Substring(outputMappingIncludingPackage.LastIndexOf('/'));
                    var package = outputMappingIncludingPackage.Replace(outputMapping, "").Replace("/", ".");
                    outputMapping = outputMapping.Substring(1);

                    _logger.LogDebug(
                        $"Processing entry as class, with mapping: {inputMapping} ->{outputMapping} in package: {package}");

                    var committedMapping = new LiveMappingEntry()
                    {
                        CreatedOn = DateTime.Now,
                        Documentation = "",
                        Distribution = Distribution.UNKNOWN,
                        Id = Guid.NewGuid(),
                        InputMapping = inputMapping,
                        OutputMapping = outputMapping,
                        Proposal = null,
                        Releases = new List<ReleaseComponent>(),
                        VersionedComponent = null,
                        MappingType = tsrgMappingType
                    };

                    committedMapping.Releases.Add(new ReleaseComponent()
                    {
                        Id = Guid.NewGuid(),
                        Member = committedMapping,
                        Release = release,
                        ComponentType = ComponentType.CLASS
                    });

                    currentClass = new VersionedComponent()
                    {
                        Id = Guid.NewGuid(),
                        Mappings = new List<LiveMappingEntry> {committedMapping},
                        CreatedBy = Guid.Empty,
                        CreatedOn = DateTime.Now,
                        GameVersion = gameVersion,
                        Component = null,
                        Proposals = new List<ProposalMappingEntry>()
                    };

                    currentClass.Metadata = new ClassMetadata()
                    {
                        InheritsFrom = new List<ClassMetadata>(),
                        Outer = null,
                        Package = package,
                        Fields = new List<FieldMetadata>(),
                        Methods = new List<MethodMetadata>(),
                        VersionedComponent = currentClass,
                        VersionedComponentForeignKey = currentClass.Id
                    };

                    committedMapping.VersionedComponent = currentClass;
                }
                else if (tsrgLine.Contains('('))
                {
                    //New method
                    var tsrgMethodData = tsrgLine.Trim().Split(' ');
                    var inputMapping = tsrgMethodData[0].Trim();
                    var descriptor = tsrgMethodData[1].Trim();
                    var outputMapping = tsrgMethodData[2].Trim();

                    _logger.LogDebug(
                        $"Processing entry as method, with mapping: {inputMapping} -> {outputMapping} and descriptor: {descriptor}");

                    var committedMappingEntry = new LiveMappingEntry()
                    {
                        Id = Guid.NewGuid(),
                        CreatedOn = DateTime.Now,
                        Documentation = "",
                        Distribution = Distribution.UNKNOWN,
                        InputMapping = inputMapping,
                        OutputMapping = outputMapping,
                        Proposal = null,
                        Releases = new List<ReleaseComponent>(),
                        VersionedComponent = null,
                        MappingType = tsrgMappingType
                    };

                    committedMappingEntry.Releases.Add(new ReleaseComponent()
                    {
                        Id = Guid.NewGuid(),
                        Member = committedMappingEntry,
                        Release = release,
                        ComponentType = ComponentType.METHOD
                    });

                    var methodVersionedMapping = new VersionedComponent()
                    {
                        Mappings = new List<LiveMappingEntry> {committedMappingEntry},
                        Proposals = new List<ProposalMappingEntry>(),
                        CreatedBy = Guid.Empty,
                        CreatedOn = DateTime.Now,
                        GameVersion = gameVersion,
                        Component = null,
                        Id = Guid.NewGuid(),
                    };

                    methodVersionedMapping.Metadata = new MethodMetadata()
                    {
                        Descriptor = descriptor,
                        IsStatic = staticMethodsFileContents.Contains(outputMapping),
                        MemberOf = currentClass.Metadata as ClassMetadata,
                        Parameters = new List<ParameterMetadata>(),
                        VersionedComponent = methodVersionedMapping,
                        VersionedComponentForeignKey = methodVersionedMapping.Id
                    };

                    committedMappingEntry.VersionedComponent = methodVersionedMapping;
                    (currentClass.Metadata as ClassMetadata).Methods.Add(methodVersionedMapping.Metadata as MethodMetadata);
                }
                else
                {
                    var tsrgFieldData = tsrgLine.Split(' ');
                    var inputMapping = tsrgFieldData[0].Trim();
                    var outputMapping = tsrgFieldData[1].Trim();

                    _logger.LogDebug(
                        $"Processing entry as field, with mapping: {inputMapping} -> {outputMapping}");

                    var committedMappingEntry = new LiveMappingEntry()
                    {
                        Id = Guid.NewGuid(),
                        CreatedOn = DateTime.Now,
                        Documentation = "",
                        Distribution = Distribution.UNKNOWN,
                        InputMapping = inputMapping,
                        OutputMapping = outputMapping,
                        Proposal = null,
                        Releases = new List<ReleaseComponent>(),
                        VersionedComponent = null,
                        MappingType = tsrgMappingType
                    };

                    committedMappingEntry.Releases.Add(new ReleaseComponent()
                    {
                        Id = Guid.NewGuid(),
                        Member = committedMappingEntry,
                        Release = release,
                        ComponentType = ComponentType.FIELD
                    });

                    var versionedMapping = new VersionedComponent()
                    {
                        Mappings = new List<LiveMappingEntry> {committedMappingEntry},
                        CreatedBy = Guid.Empty,
                        CreatedOn = DateTime.Now,
                        GameVersion = gameVersion,
                        Id = Guid.NewGuid(),
                        Proposals = new List<ProposalMappingEntry>(),
                        Component = null
                    };

                    versionedMapping.Metadata = new FieldMetadata()
                    {
                        IsStatic = false,
                        MemberOf = currentClass.Metadata as ClassMetadata,
                        VersionedComponent = versionedMapping,
                        VersionedComponentForeignKey = versionedMapping.Id
                    };

                    committedMappingEntry.VersionedComponent = versionedMapping;
                    (currentClass.Metadata as ClassMetadata).Fields.Add(versionedMapping.Metadata as FieldMetadata);
                }
            }

            //Register last class:
            if (currentClass != null)
            {
                classVersionedComponents.Add(currentClass);
            }

            return classVersionedComponents;
        }

        private Task<List<Component>> DetermineCrossVersionClassHistory(MCMSContext context, MappingType tsrgMappingType,
            IEnumerable<IEnumerable<VersionedComponent>> mcVersionClassVersionMappings)
        {
            try
            {
                _logger.LogWarning(
                $"Starting the linear processing of MCP data. Attempting to determine cross version history of classes.");

                var groupedClassData = mcVersionClassVersionMappings
                    .SelectMany(entry => entry)
                    .GroupBy(mapping => new ClassIdentification((mapping.Metadata as ClassMetadata).Package,
                        mapping.Mappings.First().OutputMapping))
                    .ToList();

                var groupedClassKeys = groupedClassData.Select(g => g.Key).Distinct().ToList();
                var existingData = context.Components.Where(c =>
                        c.Type == ComponentType.CLASS)
                    .Include(c => c.VersionedComponents)
                    .Include("VersionedComponents.Metadata")
                    .Include("VersionedComponents.Metadata.Methods")
                    .Include("VersionedComponents.Metadata.Fields")
                    .Include("VersionedComponents.Mappings")
                    .Include("VersionedComponents.Mappings.MappingType")
                    .Include("VersionedComponents.Mappings.Releases")
                    .Include("VersionedComponents.Mappings.Releases.Release")
                    .ToList()
                    .Where(c =>
                        c.VersionedComponents.Any(vc =>
                            groupedClassKeys.Any(k =>
                                (vc.Metadata as ClassMetadata).Package == k.Package &&
                                vc.Mappings.Any(lm =>
                                    lm.OutputMapping == k.TSRGOutputMapping && lm.MappingType == tsrgMappingType)
                            )))
                    .AsEnumerable()
                    .Join(groupedClassKeys,
                        c => groupedClassKeys.FirstOrDefault(k => c.VersionedComponents.Any(vc =>
                            (vc.Metadata as ClassMetadata).Package == k.Package &&
                            vc.Mappings.Any(lm =>
                                lm.OutputMapping == k.TSRGOutputMapping && lm.MappingType == tsrgMappingType)
                        )),
                        k => k,
                        (c, k) => new Tuple<ClassIdentification, Component>(k, c))
                    .GroupBy(t => t.Item1)
                    .Where(g => g.Count() == 1)
                    .ToDictionary(
                        g => g.Key,
                        g => g.ToList()[0].Item2
                    );

                var analysisData =
                    groupedClassData
                    .Select(matchingGrouping =>
                    {
                        var usingExisting = existingData.ContainsKey(matchingGrouping.Key);
                        var component = existingData.GetValueOrDefault(matchingGrouping.Key, new Component
                            {
                                Id = Guid.NewGuid(),
                                VersionedComponents = new List<VersionedComponent>(),
                                Type = ComponentType.CLASS
                            });

                        return (component: component, usingExisting: usingExisting, matchingGrouping: matchingGrouping);
                    }).ToList();

                foreach (var (component, usingExisting, matchingGrouping) in analysisData)
                {
                    component.VersionedComponents.AddRange(matchingGrouping.Where(vc => component.VersionedComponents.All(exvc => exvc.Id != vc.Id)));

                    var reducedVersionedComponents =
                        component.VersionedComponents
                            .GroupBy(vc => vc.GameVersion)
                            .ToList();

                    var reducedProcessedVersionedComponents = reducedVersionedComponents.Select(reducedVersionedMapping =>
                    {
                        if (reducedVersionedMapping.Count() == 1)
                            return reducedVersionedMapping.ToList()[0];

                        var oldestVersionedComponent = reducedVersionedMapping.OrderBy(vm => vm.CreatedOn).First();
                        var liveMappings = reducedVersionedMapping.SelectMany(r => r.Mappings).ToList();
                        var groupedLiveMappings = liveMappings.GroupBy(lm => lm.OutputMapping).ToList();
                        var processedLiveMappings = groupedLiveMappings.Select(groupedLiveMapping =>
                        {
                            if (groupedLiveMapping.Count() == 1)
                                return groupedLiveMapping.ToList()[0];

                            var oldestLiveMappings = groupedLiveMapping.OrderBy(lm => lm.CreatedOn).First();
                            var releaseComponents = groupedLiveMapping.SelectMany(lm => lm.Releases).ToList();
                            oldestLiveMappings.Releases.Clear();
                            foreach (var releaseComponent in releaseComponents)
                            {
                                releaseComponent.Member = oldestLiveMappings;
                                oldestLiveMappings.Releases.Add(releaseComponent);
                            }

                            return oldestLiveMappings;
                        });

                        oldestVersionedComponent.Mappings = processedLiveMappings.ToList();

                        return oldestVersionedComponent;
                    });

                    component.VersionedComponents = reducedProcessedVersionedComponents.ToList();

                    foreach (var versionedComponent in component.VersionedComponents)
                    {
                        versionedComponent.Component = component;
                    }
                }

                var classes = analysisData.Select(d => d.component).ToList();
                return Task.FromResult(classes);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to process class data linearly.");
                throw;
            }
        }

        private Task<List<Component>> DetermineCrossVersionMethodHistory(MCMSContext context,
            MappingType tsrgMappingType, List<Component> classesComponents)
        {
            try
            {
                _logger.LogWarning(
                $"Continuing the linear processing of MCP data. Attempting to determine cross version history of methods.");

                var methods = new List<Component>();

                var globalExistingMethodData = context.Components.Where(c =>
                        c.Type == ComponentType.METHOD)
                    .Include(c => c.VersionedComponents)
                    .Include("VersionedComponents.Metadata")
                    .Include("VersionedComponents.Mappings")
                    .Include("VersionedComponents.Mappings.MappingType")
                    .Include("VersionedComponents.Mappings.Releases")
                    .Include("VersionedComponents.Mappings.Releases.Release")
                    .ToList();

                var methodVersionedComponents = classesComponents
                    .SelectMany(c => c.VersionedComponents)
                    .Select(mapping => mapping.Metadata as ClassMetadata)
                    .SelectMany(clsmd => clsmd.Methods)
                    .Select(mthmd => mthmd.VersionedComponent);

                var groupedMethodData = methodVersionedComponents
                    .GroupBy(method =>
                        new MethodIdentification(method.Mappings.First().OutputMapping))
                    .ToList();

                var groupedMethodKeys = groupedMethodData.Select(g => g.Key).Distinct().ToList();

                var existingData =
                    globalExistingMethodData
                        .Where(c =>
                            c.VersionedComponents.Any(vc =>
                                groupedMethodKeys.Any(k =>
                                    vc.Mappings.Any(lm =>
                                        lm.OutputMapping == k.TSRGOutputMapping && lm.MappingType == tsrgMappingType)
                                )))
                        .AsEnumerable()
                        .Join(groupedMethodKeys,
                            c => groupedMethodKeys.FirstOrDefault(k => c.VersionedComponents.Any(vc =>
                                vc.Mappings.Any(lm =>
                                    lm.OutputMapping == k.TSRGOutputMapping && lm.MappingType == tsrgMappingType)
                            )),
                            k => k,
                            (c, k) => new Tuple<MethodIdentification, Component>(k, c))
                        .GroupBy(t => t.Item1)
                        .Where(g => g.Count() == 1)
                        .ToDictionary(
                            g => g.Key,
                            g => g.ToList()[0].Item2
                        );

                var analysisData =
                    groupedMethodData
                        .Select(matchingGrouping =>
                        {
                            var usingExisting = existingData.ContainsKey(matchingGrouping.Key);
                            var component = existingData.GetValueOrDefault(matchingGrouping.Key, new Component
                            {
                                Id = Guid.NewGuid(),
                                VersionedComponents = new List<VersionedComponent>(),
                                Type = ComponentType.METHOD
                            });

                            return (component: component, usingExisting: usingExisting, matchingGrouping: matchingGrouping);
                        }).ToList();

                foreach (var (component, usingExisting, matchingGrouping) in analysisData)
                {
                    component.VersionedComponents.AddRange(matchingGrouping.Where(vc => component.VersionedComponents.All(exvc => exvc.Id != vc.Id)));
                    var reducedVersionedComponents =
                        component.VersionedComponents
                            .GroupBy(vc => vc.GameVersion)
                            .ToList();

                    var reducedProcessedVersionedComponents = reducedVersionedComponents.Select(reducedVersionedMapping =>
                    {
                        if (reducedVersionedMapping.Count() == 1)
                            return reducedVersionedMapping.ToList()[0];

                        var oldestVersionedComponent = reducedVersionedMapping.OrderBy(vm => vm.CreatedOn).First();
                        var liveMappings = reducedVersionedMapping.SelectMany(r => r.Mappings).ToList();
                        var groupedLiveMappings = liveMappings.GroupBy(lm => lm.OutputMapping).ToList();
                        var processedLiveMappings = groupedLiveMappings.Select(groupedLiveMapping =>
                        {
                            if (groupedLiveMapping.Count() == 1)
                                return groupedLiveMapping.ToList()[0];

                            var oldestLiveMappings = groupedLiveMapping.OrderBy(lm => lm.CreatedOn).First();
                            var releaseComponents = groupedLiveMapping.SelectMany(lm => lm.Releases).ToList();
                            oldestLiveMappings.Releases.Clear();
                            foreach (var releaseComponent in releaseComponents)
                            {
                                releaseComponent.Member = oldestLiveMappings;
                                oldestLiveMappings.Releases.Add(releaseComponent);
                            }

                            return oldestLiveMappings;
                        });

                        oldestVersionedComponent.Mappings = processedLiveMappings.ToList();

                        return oldestVersionedComponent;
                    });

                    component.VersionedComponents = reducedProcessedVersionedComponents.ToList();

                    foreach (var versionedComponent in component.VersionedComponents)
                    {
                        versionedComponent.Component = component;
                    }

                    methods.Add(component);
                }

                return Task.FromResult(methods);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to process method data linearly.");
                throw;
            }
        }

        private Task<List<Component>> DetermineCrossVersionFieldHistory(MCMSContext context,
            MappingType tsrgMappingType, List<Component> classesComponents)
        {
            try
            {
                _logger.LogWarning(
                $"Continuing the linear processing of MCP data. Attempting to determine cross version history of fields.");

                var fields = new List<Component>();

                var globalExistingFieldData = context.Components.Where(c =>
                        c.Type == ComponentType.FIELD)
                    .Include(c => c.VersionedComponents)
                    .Include("VersionedComponents.Metadata")
                    .Include("VersionedComponents.Mappings")
                    .Include("VersionedComponents.Mappings.MappingType")
                    .Include("VersionedComponents.Mappings.Releases")
                    .Include("VersionedComponents.Mappings.Releases.Release")
                    .ToList();

                var fieldVersionedComponents = classesComponents
                    .SelectMany(c => c.VersionedComponents)
                    .Select(mapping => mapping.Metadata as ClassMetadata)
                    .SelectMany(clsmd => clsmd.Fields)
                    .Select(mthmd => mthmd.VersionedComponent);

                var groupedFieldData = fieldVersionedComponents
                    .GroupBy(field =>
                        new FieldIdentification(field.Mappings.First().OutputMapping))
                    .ToList();

                var groupedFieldKeys = groupedFieldData.Select(g => g.Key).Distinct().ToList();

                var existingData =
                    globalExistingFieldData
                        .Where(c =>
                            c.VersionedComponents.Any(vc =>
                                groupedFieldKeys.Any(k =>
                                    vc.Mappings.Any(lm =>
                                        lm.OutputMapping == k.TSRGOutputMapping && lm.MappingType == tsrgMappingType)
                                )))
                        .AsEnumerable()
                        .Join(groupedFieldKeys,
                            c => groupedFieldKeys.FirstOrDefault(k => c.VersionedComponents.Any(vc =>
                                vc.Mappings.Any(lm =>
                                    lm.OutputMapping == k.TSRGOutputMapping && lm.MappingType == tsrgMappingType)
                            )),
                            k => k,
                            (c, k) => new Tuple<FieldIdentification, Component>(k, c))
                        .GroupBy(t => t.Item1)
                        .Where(g => g.Count() == 1)
                        .ToDictionary(
                            g => g.Key,
                            g => g.ToList()[0].Item2
                        );

                var analysisData =
                    groupedFieldData
                        .Select(matchingGrouping =>
                        {
                            var usingExisting = existingData.ContainsKey(matchingGrouping.Key);
                            var component = existingData.GetValueOrDefault(matchingGrouping.Key, new Component
                            {
                                Id = Guid.NewGuid(),
                                VersionedComponents = new List<VersionedComponent>(),
                                Type = ComponentType.FIELD
                            });

                            return (component: component, usingExisting: usingExisting, matchingGrouping: matchingGrouping);
                        }).ToList();

                foreach (var (component, usingExisting, matchingGrouping) in analysisData)
                {
                    component.VersionedComponents.AddRange(matchingGrouping.Where(vc => component.VersionedComponents.All(exvc => exvc.Id != vc.Id)));
                    var reducedVersionedComponents =
                        component.VersionedComponents
                            .GroupBy(vc => vc.GameVersion)
                            .ToList();

                    var reducedProcessedVersionedComponents = reducedVersionedComponents.Select(reducedVersionedMapping =>
                    {
                        if (reducedVersionedMapping.Count() == 1)
                            return reducedVersionedMapping.ToList()[0];

                        var oldestVersionedComponent = reducedVersionedMapping.OrderBy(vm => vm.CreatedOn).First();
                        var liveMappings = reducedVersionedMapping.SelectMany(r => r.Mappings).ToList();
                        var groupedLiveMappings = liveMappings.GroupBy(lm => lm.OutputMapping).ToList();
                        var processedLiveMappings = groupedLiveMappings.Select(groupedLiveMapping =>
                        {
                            if (groupedLiveMapping.Count() == 1)
                                return groupedLiveMapping.ToList()[0];

                            var oldestLiveMappings = groupedLiveMapping.OrderBy(lm => lm.CreatedOn).First();
                            var releaseComponents = groupedLiveMapping.SelectMany(lm => lm.Releases).ToList();
                            oldestLiveMappings.Releases.Clear();
                            foreach (var releaseComponent in releaseComponents)
                            {
                                releaseComponent.Member = oldestLiveMappings;
                                oldestLiveMappings.Releases.Add(releaseComponent);
                            }

                            return oldestLiveMappings;
                        });

                        oldestVersionedComponent.Mappings = processedLiveMappings.ToList();

                        return oldestVersionedComponent;
                    });

                    component.VersionedComponents = reducedProcessedVersionedComponents.ToList();

                    foreach (var versionedComponent in component.VersionedComponents)
                    {
                        versionedComponent.Component = component;
                    }

                    fields.Add(component);
                }

                return Task.FromResult(fields);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to process field data linearly.");
                throw;
            }
        }

        private async Task SaveData(MCMSContext context, List<Component> classes, List<Component> methods,
            List<Component> fields)
        {
            _logger.LogWarning($"Found: {classes.Count()} classes, {methods.Count()} methods and {fields.Count()} fields.");

            await AttachComponents(context, classes, "class");
            //await AttachComponents(context, methods, "method");
            //await AttachComponents(context, fields, "field");

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to save imported data!");
                throw;
            }
        }

        private Task AttachComponents(MCMSContext context, List<Component> componentsToAttach, string displayName)
        {
            _logger.LogWarning($"Attaching {componentsToAttach.Count} {displayName} components to the context.");
            var totalComponentCount = componentsToAttach.Count;
            var currentlyProcessed = 0;
            var lastOutputPercentage = -1;

            foreach (var component in componentsToAttach.ToList())
            {
                currentlyProcessed++;
                var currentPercentage = (int) Math.Floor(currentlyProcessed / (double) totalComponentCount * 100);
                if (lastOutputPercentage < currentPercentage)
                {
                    lastOutputPercentage = currentPercentage;
                    _logger.LogInformation(
                        $"  > Attached: {currentPercentage}% ({currentlyProcessed}/{totalComponentCount}) {displayName} components.");
                }

                try
                {
                    if (context.Entry(component).State == EntityState.Detached)
                        context.Add(component);

                    context.SaveChanges();
                    context.Attach(component);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Failed to save imported data!");
                    throw;
                }
            }

            return Task.CompletedTask;
        }

        private class ClassIdentification
            : IEquatable<ClassIdentification>
        {
            public ClassIdentification(string package, string tsrgOutputMappingName)
            {
                Package = package ?? throw new ArgumentNullException(nameof(package));
                TSRGOutputMapping = tsrgOutputMappingName ??
                                        throw new ArgumentNullException(nameof(tsrgOutputMappingName));
            }

            public string Package { get; }

            public string TSRGOutputMapping { get; }


            public bool Equals(ClassIdentification other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Package == other.Package && TSRGOutputMapping == other.TSRGOutputMapping;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((ClassIdentification) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Package != null ? Package.GetHashCode() : 0) * 397) ^
                           (TSRGOutputMapping != null ? TSRGOutputMapping.GetHashCode() : 0);
                }
            }
        }

        private class MethodIdentification
            : IEquatable<MethodIdentification>
        {
            public MethodIdentification(string tsrgOutputMapping)
            {
                TSRGOutputMapping = tsrgOutputMapping ?? throw new ArgumentNullException(nameof(tsrgOutputMapping));
            }

            public string TSRGOutputMapping { get; }

            public bool Equals(MethodIdentification other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return TSRGOutputMapping == other.TSRGOutputMapping;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((MethodIdentification) obj);
            }

            public override int GetHashCode()
            {
                return (TSRGOutputMapping != null ? TSRGOutputMapping.GetHashCode() : 0);
            }
        }

        private class FieldIdentification
            : IEquatable<FieldIdentification>
        {
            public FieldIdentification(string tsrgOutputMapping)
            {
                TSRGOutputMapping = tsrgOutputMapping ?? throw new ArgumentNullException(nameof(tsrgOutputMapping));
            }

            public string TSRGOutputMapping { get; }

            public bool Equals(FieldIdentification other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return TSRGOutputMapping == other.TSRGOutputMapping;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((FieldIdentification) obj);
            }

            public override int GetHashCode()
            {
                return (TSRGOutputMapping != null ? TSRGOutputMapping.GetHashCode() : 0);
            }
        }
    }
}
