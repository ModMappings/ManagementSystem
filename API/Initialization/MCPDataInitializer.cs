using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Models.Field;
using Data.Core.Models.Method;
using Data.Core.Models.Parameter;
using Data.EFCore.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace API.Initialization
{
    public class MCPDataInitializer
    {
        private const string MAVEN_MCP_URL = "https://files.minecraftforge.net/maven/de/oceanlabs/mcp/mcp_config";

        private static User initUser;

        public static void InitializeData(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<MCPContext>())
                {
                    InitializeDummyUser(app, context);
                    InitializeMCP(app, context);
                }
            }

        }

        private static void InitializeDummyUser(IApplicationBuilder app, MCPContext context)
        {
            var logger = app.ApplicationServices.GetRequiredService <ILogger<MCPDataInitializer>>();

            if (context.Database.GetPendingMigrations().Any())
            {
                logger.LogWarning("Updating database schema.");
                context.Database.Migrate();
            }

            if (!EnumerableExtensions.Any(context.Users))
            {
                logger.LogWarning("Adding debugging users.");

                context.Users.Add(new User
                {
                    CanCommit = true,
                    CanCreateGameVersions = true,
                    CanEdit = true,
                    CanRelease = true,
                    CanReview = true,
                    Id = Guid.NewGuid(),
                    Name = "Dummy"
                });

                initUser = new User
                {
                    CanCommit = false,
                    CanCreateGameVersions = false,
                    CanEdit = false,
                    CanRelease = false,
                    CanReview = false,
                    Id = Guid.NewGuid(),
                    Name = "Initialization"
                };
                context.Users.Add(initUser);

                context.SaveChanges();
            }
            else
            {
                logger.LogWarning("Using existing initialization user.");
                initUser = context.Users.FirstOrDefault(user => user.Name == "Initialization");
            }
        }

        private static void InitializeMCP(IApplicationBuilder app, MCPContext context)
        {
            var logger = app.ApplicationServices.GetRequiredService <ILogger<MCPDataInitializer>>();

            if (context.ClassMappings.Any())
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
                    Classes = new List<ClassReleaseMember>(),
                    CreatedBy = initUser,
                    CreatedOn = DateTime.Now,
                    Fields = new List<FieldReleaseMember>(),
                    GameVersion = gameVersion,
                    Methods = new List<MethodReleaseMember>(),
                    Name = relName,
                    Parameters = new List<ParameterReleaseMember>()
                };
            }).ToDictionary(release => release.Name, release => release);

            foreach (var mcVersionsValue in mcVersions.Values)
            {
                context.GameVersions.Add(mcVersionsValue);
            }

            foreach (var releasesValue in releases.Values)
            {
                context.Releases.Add(releasesValue);
            }

            context.SaveChanges();

            logger.LogInformation(
                $"Processed MCP maven version information. Found: {mcVersions.Count} minecraft major version releases, and: {releases} MCP releases for those versions");

            var classes = new List<ClassMapping>();
            var methods = new List<MethodMapping>();
            var fields = new List<FieldMapping>();

            logger.LogInformation("Starting the processing of MCP Config data.");

            var mcVersionClassVersionMappings = Task.WhenAll(mavenReleaseNames.Select(mavenReleaseName =>
                Task.Run(() => ProcessMcpDataForRelease(logger, mavenReleaseName, releases)))).Result;


            logger.LogWarning($"Starting the linear processing of MCP data. Attempting to determine cross version history of classes.");

            classes = mcVersionClassVersionMappings.SelectMany(entry => entry).GroupBy(mapping =>
                mapping.Package + "." + mapping.CommittedMappings.First().OutputMapping).Select(
                matchingGrouping => new ClassMapping
                {
                    Id = Guid.NewGuid(),
                    VersionedMappings = matchingGrouping.ToList()
                }).ToList();

            classes.ForEach(cls => cls.VersionedMappings.ForEach(clsvm => clsvm.Mapping = cls));

            logger.LogWarning($"Continuing the linear processing of MCP data. Attempting to determine cross version history of methods.");

            classes.ForEach(classMapping =>
            {
                methods.AddRange(classMapping.VersionedMappings.SelectMany(mapping => mapping.Methods).GroupBy(method =>
                    method.CommittedMappings.First().OutputMapping + "%" + method.Descriptor + "%" + method.IsStatic).Select(
                    methodGrouping => new MethodMapping
                    {
                        Id = Guid.NewGuid(),
                        VersionedMappings = methodGrouping.ToList()
                    }));
            });

            methods.ForEach(mtd => mtd.VersionedMappings.ForEach(mtdvm => mtdvm.Mapping = mtd));

            logger.LogWarning($"Continuing the linear processing of MCP data. Attempting to determine cross version history of fields.");

            classes.ForEach(classMapping =>
            {
                fields.AddRange(classMapping.VersionedMappings.SelectMany(mapping => mapping.Fields).GroupBy(field =>
                    field.CommittedMappings.First().OutputMapping + "%" + field.IsStatic).Select(
                    fieldGrouping => new FieldMapping
                    {
                        Id = Guid.NewGuid(),
                        VersionedMappings = fieldGrouping.ToList()
                    }));
            });

            fields.ForEach(fld => fld.VersionedMappings.ForEach(fldvm => fldvm.Mapping = fld));

            logger.LogWarning($"Found: {classes.Count} classes, {methods.Count} methods and {fields.Count} fields.");

            foreach (var classMapping in classes)
            {
                context.ClassMappings.Add(classMapping);
            }

            context.SaveChanges();
        }

        private static List<ClassVersionedMapping> ProcessMcpDataForRelease(ILogger<MCPDataInitializer> logger, string releaseName, Dictionary<string, Release> releases)
        {
            List<ClassVersionedMapping> classVersionedMappings;
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
                    return new List<ClassVersionedMapping>();
                }

                var zip = new ZipArchive(mavenMetaDownloadResult.Content.ReadAsStreamAsync().Result);
                var tsrgContents = ReadLines(() => zip.GetEntry("config/joined.tsrg").Open(),
                    Encoding.UTF8).ToList();

                var staticMethods = ReadLines(() => zip.GetEntry("config/static_methods.txt").Open(),
                    Encoding.UTF8).ToList();

                logger.LogInformation($"Found: {tsrgContents.Count} entries in the tsrg information.");

                classVersionedMappings = new List<ClassVersionedMapping>();
                ClassVersionedMapping currentClass = null;

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

                        var committedMapping = new ClassCommittedMappingEntry
                        {
                            CreatedOn = DateTime.Now,
                            Documentation = "",
                            Id = Guid.NewGuid(),
                            InputMapping = inputMapping,
                            OutputMapping = outputMapping,
                            Proposal = null,
                            Releases = new List<ClassReleaseMember>(),
                            VersionedMapping = null
                        };

                        committedMapping.Releases.Add(new ClassReleaseMember
                        {
                            Id = Guid.NewGuid(),
                            Member = committedMapping,
                            Release = release
                        });

                        currentClass = new ClassVersionedMapping
                        {
                            Id = Guid.NewGuid(),
                            CommittedMappings = new List<ClassCommittedMappingEntry> {committedMapping},
                            CreatedBy = initUser,
                            CreatedOn = DateTime.Now,
                            GameVersion = gameVersion,
                            InheritsFrom = new List<ClassVersionedMapping>(),
                            Mapping = null,
                            Outer = null,
                            Package = package,
                            ProposalMappings = new List<ClassProposalMappingEntry>(),
                            Fields = new List<FieldVersionedMapping>(),
                            Methods = new List<MethodVersionedMapping>()
                        };

                        committedMapping.VersionedMapping = currentClass;
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

                        var committedMappingEntry = new MethodCommittedMappingEntry
                        {
                            Id = Guid.NewGuid(),
                            CreatedOn = DateTime.Now,
                            Documentation = "",
                            InputMapping = inputMapping,
                            OutputMapping = outputMapping,
                            Proposal = null,
                            Releases = new List<MethodReleaseMember>(),
                            VersionedMapping = null
                        };

                        committedMappingEntry.Releases.Add(new MethodReleaseMember
                        {
                            Id = Guid.NewGuid(),
                            Member = committedMappingEntry,
                            Release = release
                        });

                        var methodVersionedMapping = new MethodVersionedMapping
                        {
                            CommittedMappings = new List<MethodCommittedMappingEntry> {committedMappingEntry},
                            CreatedBy = initUser,
                            CreatedOn = DateTime.Now,
                            Descriptor = descriptor,
                            GameVersion = gameVersion,
                            Id = Guid.NewGuid(),
                            IsStatic = staticMethods.Contains(outputMapping),
                            Mapping = null,
                            MemberOf = currentClass,
                            Parameters = new List<ParameterVersionedMapping>(),
                        };

                        committedMappingEntry.VersionedMapping = methodVersionedMapping;
                        currentClass.Methods.Add(methodVersionedMapping);
                    }
                    else
                    {
                        var tsrgFieldData = tsrgLine.Split(' ');
                        var inputMapping = tsrgFieldData[0].Trim();
                        var outputMapping = tsrgFieldData[1].Trim();

                        logger.LogDebug(
                            $"Processing entry as field, with mapping: {inputMapping} -> {outputMapping}");

                        var committedMappingEntry = new FieldCommittedMappingEntry
                        {
                            Id = Guid.NewGuid(),
                            CreatedOn = DateTime.Now,
                            Documentation = "",
                            InputMapping = inputMapping,
                            OutputMapping = outputMapping,
                            Proposal = null,
                            Releases = new List<FieldReleaseMember>(),
                            VersionedMapping = null
                        };

                        committedMappingEntry.Releases.Add(new FieldReleaseMember
                        {
                            Id = Guid.NewGuid(),
                            Member = committedMappingEntry,
                            Release = release
                        });

                        var versionedMapping = new FieldVersionedMapping
                        {
                            CommittedMappings = new List<FieldCommittedMappingEntry> {committedMappingEntry},
                            CreatedBy = initUser,
                            CreatedOn = DateTime.Now,
                            GameVersion = gameVersion,
                            Id = Guid.NewGuid(),
                            IsStatic = false,
                            Mapping = null,
                            MemberOf = currentClass,
                            ProposalMappings = new List<FieldProposalMappingEntry>()
                        };

                        committedMappingEntry.VersionedMapping = versionedMapping;
                        currentClass.Fields.Add(versionedMapping);
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
