using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.EFCore.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Data.WebApi.Initialization
{
    public class MCPDataInitializer
    {
        private const string MAVEN_MCP_URL = "https://files.minecraftforge.net/maven/de/oceanlabs/mcp/mcp_config";

        private static Guid initUser;

        public static void InitializeData(IApplicationBuilder app)
        {
            return;

            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<MCMSContext>())
                {
                    InitializeDummyUser(app, context);
                    InitializeMCPConfigData(app, context);
                }
            }

        }

        private static void InitializeDummyUser(IApplicationBuilder app, MCMSContext context)
        {
            var logger = app.ApplicationServices.GetRequiredService <ILogger<MCPDataInitializer>>();

            if (context.Database.GetPendingMigrations().Any())
            {
                logger.LogWarning("Updating database schema.");
                context.Database.Migrate();
            }

            initUser = context.VersionedComponents.OrderByDescending(vc => vc.CreatedOn).Select(vc => vc.CreatedBy)
                .FirstOrDefault();

            if (initUser == null)
                initUser = Guid.NewGuid();
        }

        private static void InitializeMCPConfigData(IApplicationBuilder app, MCMSContext context)
        {
            var logger = app.ApplicationServices.GetRequiredService <ILogger<MCPDataInitializer>>();

            if (context.Components.Any())
            {
                logger.LogWarning("MCP Data already loaded. Skipping init.");
                return;
            }

            logger.LogWarning("MCP Data missing, initializing database.");

            var mcVersions = new Dictionary<string, GameVersion>();
            var releases = new Dictionary<string, Release>();
            var mavenReleaseNames = new List<string>();



            //Download the maven metadata and parse the versioning information.
            using (var client = new HttpClient())
            {
                var mavenMetaDownloadResult = client.GetAsync($"{MAVEN_MCP_URL}/maven-metadata.xml").Result;
                if (!mavenMetaDownloadResult.IsSuccessStatusCode)
                    throw new Exception("Could not initialize. No maven metadata available");

                var xDoc = XDocument.Parse(
                    Encoding.UTF8.GetString(mavenMetaDownloadResult.Content.ReadAsByteArrayAsync().Result));

                mavenReleaseNames = xDoc.Root.Element("versioning").Element("versions").Elements()
                    .Select(element => element.Value).OrderBy(s => s).ToList();
            }

            mavenReleaseNames = mavenReleaseNames.ToList();

            logger.LogInformation(
                $"MCP Version information extracted. Found: {mavenReleaseNames.Count} released versions on its maven.");

            //Parse the maven metadata versions list into game versions.
            mcVersions = mavenReleaseNames.Where(relName => !relName.Contains("-")).Select(mcVerName => new GameVersion
            {
                Id = Guid.NewGuid(),
                CreatedBy = initUser,
                CreatedOn = DateTime.Now,
                IsPreRelease = false,
                IsSnapshot = false,
                Name = mcVerName
            }).ToDictionary(version => version.Name, version => version);

            //Now create a release for each version information.
            releases = mavenReleaseNames.Select(relName =>
            {
                var relNameComp = relName.Split("-");
                var gameVersion = mcVersions[relNameComp[0]];

                return new Release
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = initUser,
                    CreatedOn = DateTime.Now,
                    GameVersion = gameVersion,
                    Name = relName,
                    Components = new List<ReleaseComponent>()
                };
            }).ToDictionary(release => release.Name, release => release);

            var tsrgMappingType = new MappingType()
            {
                CreatedBy = initUser,
                CreatedOn = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = "OBF to TSRG",
                Releases = releases.Values.ToList(),
            };

            foreach (var mcVersionsValue in mcVersions.Values)
            {
                context.GameVersions.Add(mcVersionsValue);
            }

            foreach (var releasesValue in releases.Values)
            {
                context.Releases.Add(releasesValue);
            }

            context.MappingTypes.Add(tsrgMappingType);

            context.SaveChanges();

            logger.LogInformation(
                $"Processed MCP maven version information. Found: {mcVersions.Count} minecraft major version releases, and: {releases} MCP releases for those versions");

            var classes = new List<Component>();
            var methods = new List<Component>();
            var fields = new List<Component>();

            logger.LogInformation("Starting the processing of MCP Config data.");

            var mcVersionClassVersionMappings = Task.WhenAll(mavenReleaseNames.Select(mavenReleaseName =>
                Task.Run(() => ProcessMcpConfigDataForRelease(logger, mavenReleaseName, releases, tsrgMappingType)))).Result;


            logger.LogWarning($"Starting the linear processing of MCP data. Attempting to determine cross version history of classes.");

            classes = mcVersionClassVersionMappings.SelectMany(entry => entry).GroupBy(mapping =>
                (mapping.Metadata as ClassMetadata).Package + "." + mapping.Mappings.First().OutputMapping).Select(
                matchingGrouping => new Component()
                {
                    Id = Guid.NewGuid(),
                    VersionedMappings = matchingGrouping.ToList(),
                    Type = ComponentType.CLASS
                }).ToList();

            classes.ForEach(cls => cls.VersionedMappings.ForEach(clsvm => clsvm.Component = cls));

            logger.LogWarning($"Continuing the linear processing of MCP data. Attempting to determine cross version history of methods.");

            classes.ForEach(classMapping =>
            {
                methods
                    .AddRange(
                        classMapping.VersionedMappings
                            .Select(mapping => mapping.Metadata as ClassMetadata)
                            .SelectMany(clsmd => clsmd.Methods)
                            .Select(mthmd => mthmd.MemberOf.Component)
                            .GroupBy(method =>
                                method.Mappings.First().OutputMapping + "%" + (method.Metadata as MethodMetadata).Descriptor + "%" + (method.Metadata as MethodMetadata).IsStatic).Select(
                                methodGrouping => new Component()
                                {
                                    Id = Guid.NewGuid(),
                                    VersionedMappings = methodGrouping.ToList(),
                                    Type = ComponentType.METHOD
                                }));
            });

            methods.ForEach(mtd => mtd.VersionedMappings.ForEach(mtdvm => mtdvm.Component = mtd));

            logger.LogWarning($"Continuing the linear processing of MCP data. Attempting to determine cross version history of fields.");

            classes.ForEach(classMapping =>
            {
                fields.AddRange(
                    classMapping.VersionedMappings
                        .Select(mapping => mapping.Metadata as ClassMetadata)
                        .SelectMany(mapping => mapping.Fields)
                        .Select(fldmd => fldmd.MemberOf.Component)
                        .GroupBy(field =>
                    field.Mappings.First().OutputMapping + "%" + (field.Metadata as FieldMetadata).IsStatic).Select(
                    fieldGrouping => new Component()
                    {
                        Id = Guid.NewGuid(),
                        VersionedMappings = fieldGrouping.ToList(),
                        Type = ComponentType.FIELD
                    }));
            });

            fields.ForEach(fld => fld.VersionedMappings.ForEach(fldvm => fldvm.Component = fld));

            logger.LogWarning($"Found: {classes.Count} classes, {methods.Count} methods and {fields.Count} fields.");

            context.Components.AddRange(classes);
            context.Components.AddRange(methods);
            context.Components.AddRange(fields);

            context.SaveChanges();
        }

        private static List<VersionedComponent> ProcessMcpConfigDataForRelease(ILogger<MCPDataInitializer> logger, string releaseName, Dictionary<string, Release> releases, MappingType tsrgMappingType)
        {
            List<VersionedComponent> classVersionedMappings;
            logger.LogInformation($"Processing: {releaseName}");
            var release = releases[releaseName];
            var gameVersion = release.GameVersion;

            //Download the config zip for a given client.
            using (var client = new HttpClient())
            {
                var mavenMetaDownloadResult =
                    client.GetAsync($"{MAVEN_MCP_URL}/{releaseName}/mcp_config-{releaseName}.zip").Result;
                if (!mavenMetaDownloadResult.IsSuccessStatusCode)
                {
                    Log.Error("Could not download the mcp config zip. Error message: " + mavenMetaDownloadResult.ReasonPhrase + " from url: " + $"{MAVEN_MCP_URL}/{releaseName}/mcp_config-{releaseName}.zip");
                    return new List<VersionedComponent>();
                }

                var zip = new ZipArchive(mavenMetaDownloadResult.Content.ReadAsStreamAsync().Result);
                var tsrgContents = ReadLines(() => zip.GetEntry("config/joined.tsrg").Open(),
                    Encoding.UTF8).ToList();

                var staticMethods = ReadLines(() => zip.GetEntry("config/static_methods.txt").Open(),
                    Encoding.UTF8).ToList();

                logger.LogInformation($"Found: {tsrgContents.Count} entries in the tsrg information.");

                classVersionedMappings = new List<VersionedComponent>();
                VersionedComponent currentClass = null;

                var totalLineCount = tsrgContents.Count;
                var currentlyProcessed = 0d;
                var currentProcentage = -1d;

                foreach (var tsrgLine in tsrgContents)
                {
                    currentlyProcessed += 1;
                    var newProcentage = Math.Floor(currentlyProcessed / totalLineCount * 100);
                    if (newProcentage > currentProcentage)
                    {
                        currentProcentage = newProcentage;
                        logger.LogInformation(
                            $"Processed: {currentProcentage}% of the lines in the{releaseName} tsrg file.");
                    }

                    logger.LogDebug($"Processing tsrg line: {tsrgLine}");
                    if (!tsrgLine.StartsWith('\t'))
                    {
                        //Register previous class:
                        if (currentClass != null)
                        {
                            classVersionedMappings.Add(currentClass);
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

                        logger.LogDebug(
                            $"Processing entry as class, with mapping: {inputMapping} ->{outputMapping} in package: {package}");

                        var committedMapping = new LiveMappingEntry()
                        {
                            CreatedOn = DateTime.Now,
                            Documentation = "",
                            Id = Guid.NewGuid(),
                            InputMapping = inputMapping,
                            OutputMapping = outputMapping,
                            Proposal = null,
                            Releases = new List<ReleaseComponent>(),
                            Mapping = null,
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
                            CreatedBy = initUser,
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
                            Component = currentClass
                        };

                        committedMapping.Mapping = currentClass;
                    }
                    else if (tsrgLine.Contains('('))
                    {
                        //New method
                        var tsrgMethodData = tsrgLine.Trim().Split(' ');
                        var inputMapping = tsrgMethodData[0].Trim();
                        var descriptor = tsrgMethodData[1].Trim();
                        var outputMapping = tsrgMethodData[2].Trim();

                        logger.LogDebug(
                            $"Processing entry as method, with mapping: {inputMapping} -> {outputMapping} and descriptor: {descriptor}");

                        var committedMappingEntry = new LiveMappingEntry()
                        {
                            Id = Guid.NewGuid(),
                            CreatedOn = DateTime.Now,
                            Documentation = "",
                            InputMapping = inputMapping,
                            OutputMapping = outputMapping,
                            Proposal = null,
                            Releases = new List<ReleaseComponent>(),
                            Mapping = null,
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
                            CreatedBy = initUser,
                            CreatedOn = DateTime.Now,
                            GameVersion = gameVersion,
                            Id = Guid.NewGuid(),
                        };

                        methodVersionedMapping.Metadata = new MethodMetadata()
                        {
                            Descriptor = descriptor,
                            IsStatic = staticMethods.Contains(outputMapping),
                            MemberOf = currentClass.Metadata as ClassMetadata,
                            Parameters = new List<ParameterMetadata>(),
                        };

                        committedMappingEntry.Mapping = methodVersionedMapping;
                        (currentClass.Metadata as ClassMetadata).Methods.Add(methodVersionedMapping.Metadata as MethodMetadata);
                    }
                    else
                    {
                        var tsrgFieldData = tsrgLine.Split(' ');
                        var inputMapping = tsrgFieldData[0].Trim();
                        var outputMapping = tsrgFieldData[1].Trim();

                        logger.LogDebug(
                            $"Processing entry as field, with mapping: {inputMapping} -> {outputMapping}");

                        var committedMappingEntry = new LiveMappingEntry()
                        {
                            Id = Guid.NewGuid(),
                            CreatedOn = DateTime.Now,
                            Documentation = "",
                            InputMapping = inputMapping,
                            OutputMapping = outputMapping,
                            Proposal = null,
                            Releases = new List<ReleaseComponent>(),
                            Mapping = null,
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
                            CreatedBy = initUser,
                            CreatedOn = DateTime.Now,
                            GameVersion = gameVersion,
                            Id = Guid.NewGuid(),
                            Proposals = new List<ProposalMappingEntry>(),
                            Component = null
                        };

                        versionedMapping.Metadata = new FieldMetadata()
                        {
                            IsStatic = false,
                            MemberOf = currentClass.Metadata as ClassMetadata
                        };

                        committedMappingEntry.Mapping = versionedMapping;
                        (currentClass.Metadata as ClassMetadata).Fields.Add(versionedMapping.Metadata as FieldMetadata);
                    }
                }
            }

            return classVersionedMappings;
        }

        private static IEnumerable<string> ReadLines(Func<Stream> streamProvider,
            Encoding encoding)
        {
            using (var stream = streamProvider())
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
