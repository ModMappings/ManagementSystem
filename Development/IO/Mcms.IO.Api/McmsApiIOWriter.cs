using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mcms.Api.Data.Core.Manager.Core;
using Mcms.Api.Data.Core.Manager.Mapping.Component;
using Mcms.Api.Data.Core.Raw;
using Mcms.Api.Data.Poco.Models.Comments;
using Mcms.Api.Data.Poco.Models.Core;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Mapping.Metadata;
using Mcms.IO.Api.Artifact;
using Mcms.IO.Core;
using Mcms.IO.Core.Artifacts;
using Mcms.IO.Core.Extensions;
using Mcms.IO.Core.Writing;
using Mcms.IO.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Mcms.IO.Api
{
    public class McmsApiIOWriter : IIOWriter
    {
        private readonly ILogger<McmsApiIOWriter> _logger;

        public McmsApiIOWriter(ILogger<McmsApiIOWriter> logger)
        {
            _logger = logger;
        }

        public async Task WriteTo(ExternalRelease externalRelease, IArtifact artifact, WriteContext context)
        {
            var dbContext = (artifact as McmsApiArtifact)?.RawDataAccessor;
            if (dbContext == null)
                throw new ArgumentException( "No valid mcms artifact is given.", nameof(artifact));

            var gameVersionDataManager = ((McmsApiArtifact) artifact).GameVersionDataManager;
            if (gameVersionDataManager == null)
                throw new ArgumentException( "No valid mcms artifact is given.", nameof(artifact));
            var gameVersion = await InitializeGameVersion(gameVersionDataManager, artifact.GameVersion);

            var mappingTypeDataManager = ((McmsApiArtifact) artifact).MappingTypeDataManager;
            if (mappingTypeDataManager == null)
                throw new ArgumentException( "No valid mcms artifact is given.", nameof(artifact));
            var mappingType = await InitializeMappingType(mappingTypeDataManager, externalRelease.MappingType);

            var releaseDataManager = ((McmsApiArtifact) artifact).ReleaseDataManager;
            if (releaseDataManager == null)
                throw new ArgumentException( "No valid mcms artifact is given.", nameof(artifact));
            var release = await InitializeRelease(releaseDataManager, externalRelease.Name, externalRelease.IsSnapshot,
                mappingType, gameVersion);

            var componentDataManager = ((McmsApiArtifact) artifact).ComponentDataManager;

            await externalRelease.Packages.ForEachWithProgressCallbackAsync(async package =>
                {
                    var versionedPackageComponent = await HandlePackageData(componentDataManager, mappingType, gameVersion, release, package,
                        context.DeduplicationStrategies.Package);

                    await package.Classes.ForEachWithProgressCallbackAsync(async @class =>
                        {
                            var versionedClassComponent = await HandleClassData(componentDataManager, mappingType, gameVersion, release, versionedPackageComponent, @class,
                                context.DeduplicationStrategies.Class);

                            await @class.Methods.ForEachWithProgressCallbackAsync(async method =>
                                {
                                    var versionedMethodComponent = await HandleMethodData(componentDataManager, mappingType, gameVersion, release, versionedClassComponent, method,
                                        context.DeduplicationStrategies.Method);

                                    await method.Parameters.ForEachWithProgressCallbackAsync(async parameter =>
                                        {
                                            await HandleParameterData(componentDataManager, mappingType, gameVersion, release, versionedMethodComponent, method, parameter,
                                                context.DeduplicationStrategies.Parameter);
                                        },
                                        (count, current, percentage) =>
                                        {
                                            _logger.LogInformation(
                                                $"\t\t\t\t> {percentage}% ({current}/{count}): Importing parameters from: {method.Input}<->{method.Output} in {@class.Input}<->{@class.Output}, {package.Input}<->{package.Output}, {artifact.Name} ...");
                                        });

                                },
                                (count, current, percentage) =>
                                {
                                    _logger.LogInformation(
                                        $"\t\t\t> {percentage}% ({current}/{count}): Importing methods from: {@class.Input}<->{@class.Output} in {package.Input}<->{package.Output}, {artifact.Name} ...");
                                });

                            await @class.Fields.ForEachWithProgressCallbackAsync(async field =>
                                {
                                    await HandleFieldData(componentDataManager, mappingType, gameVersion, release, versionedClassComponent, field,
                                        context.DeduplicationStrategies.Field);
                                },
                                (count, current, percentage) =>
                                {
                                    _logger.LogInformation(
                                        $"\t\t\t> {percentage}% ({current}/{count}): Importing fields from: {@class.Input}<->{@class.Output} in {package.Input}<->{package.Output}, {artifact.Name} ...");
                                });

                        },
                        (count, current, percentage) =>
                        {
                            _logger.LogInformation(
                                $"\t\t> {percentage}% ({current}/{count}): Importing classes from: {package.Input}<->{package.Output} in {artifact.Name} ...");
                        });

                },
                (count, current, percentage) =>
            {
                _logger.LogInformation(
                    $"\t> {percentage}% ({current}/{count}): Importing packages from: {artifact.Name} ...");
            });
        }

        private async Task<GameVersion> InitializeGameVersion(IGameVersionDataManager dataManager, string gameVersionName)
        {
            var gameVersionQuery = await dataManager.FindByName(gameVersionName);
            var gameVersion = gameVersionQuery.FirstOrDefault();
            if (gameVersion != null) return gameVersion;

            gameVersion = new GameVersion()
            {
                CreatedBy = Guid.Empty,
                CreatedOn = DateTime.Now,
                Id = Guid.NewGuid(),
                IsSnapshot = Regex.IsMatch(gameVersionName, @"\d\dw.+") || gameVersionName.Contains("_"),
                IsPreRelease = Regex.IsMatch(gameVersionName, @"^(\d+)\.(\d+)(\.(\d+))?( |-).+$"),
                Name = gameVersionName
            };

            await dataManager.CreateGameVersion(gameVersion);
            await dataManager.SaveChanges();

            return gameVersion;
        }

        private async Task<MappingType> InitializeMappingType(IMappingTypeDataManager dataManager, string mappingTypeName)
        {
            var mappingTypeRequest = await dataManager.FindByName(mappingTypeName);
            var mappingType = mappingTypeRequest.FirstOrDefault();
            if (mappingType != null) return mappingType;

            mappingType = new MappingType()
            {
                CreatedBy = Guid.Empty,
                CreatedOn = DateTime.Now,
                Id = Guid.NewGuid(),
                LockedVersionedComponents = new List<LockingEntry>(),
                Name = mappingTypeName,
                Releases = new List<Release>()
            };

            await dataManager.CreateMappingType(mappingType);
            await dataManager.SaveChanges();

            return mappingType;
        }

        private async Task<Release> InitializeRelease(IReleaseDataManager releaseDataManager, string releaseName, bool isSnapshot,
            MappingType mappingType, GameVersion gameVersion)
        {
            var releaseQuery =
                await releaseDataManager.FindUsingFilter(null, releaseName, mappingType.Name, gameVersion.Name);
            var release = releaseQuery.FirstOrDefault();
            if (release != null) return release;

            release = new Release()
            {
                Comments = new List<Comment>(),
                Components = new List<ReleaseComponent>(),
                CreatedBy = Guid.Empty,
                CreatedOn = DateTime.Now,
                GameVersion = gameVersion,
                Id = Guid.NewGuid(),
                IsSnapshot = isSnapshot,
                MappingType = mappingType,
                Name = releaseName
            };

            await releaseDataManager.CreateRelease(release);
            await releaseDataManager.SaveChanges();
            return release;
        }

        private async Task<IQueryable<Component>> FindAllComponentsFromMappingAndStrategy(
            IComponentDataManager componentDataManager, MappingType mappingType, string inputMapping,
            string outputMapping, DeduplicationStrategy strategy)
        {
            var inputQuery = await componentDataManager.FindByInputMapping("*", inputMapping);
            var outputQuery = await componentDataManager.FindByOutputMapping("*", outputMapping);

            switch (strategy)
            {
                case DeduplicationStrategy.ALWAYS_CREATE:
                    return new List<Component>().AsQueryable();
                case DeduplicationStrategy.INPUT_UNIQUE:
                    return inputQuery;
                case DeduplicationStrategy.OUTPUT_UNIQUE:
                    return outputQuery;
                case DeduplicationStrategy.BOTH_UNIQUE:
                    return inputQuery.Union(outputQuery);
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }
        }

        private async Task<VersionedComponent> HandlePackageData(IComponentDataManager componentDataManager, MappingType mappingType, GameVersion gameVersion, Release release, ExternalPackage package, DeduplicationStrategy strategy)
        {
            //Get everything that matches the mapping.
            var existingCandidatesQuery = await FindAllComponentsFromMappingAndStrategy(componentDataManager, mappingType,
                package.Input, package.Output, strategy);

            //Now of all those mappings that match the mapping only get the packages.
            var existingPackageCandidatesQuery = existingCandidatesQuery.Where(c => c.Type == ComponentType.PACKAGE);

            //Either the package exists in that location, or not.
            //So check for an existing package.
            //If multiple exist we will still just take the first in case of packages.
            var packageComponent = existingPackageCandidatesQuery.FirstOrDefault();
            var createdNewComponent = false;

            //Step 1, check for a new package.
            //If so create a new component for it.
            if (packageComponent == null)
            {
                createdNewComponent = true;
                packageComponent = new Component()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Type = ComponentType.PACKAGE,
                    VersionedComponents = new List<VersionedComponent>()
                };
            }

            //Step 2, get the versioned component from it.
            var packageVersionedComponent =
                packageComponent.VersionedComponents.FirstOrDefault(vc => vc.GameVersion == gameVersion);

            //Step 3, check for a new version in the same package.
            //If so create a new versioned component, and its metadata.
            if (packageVersionedComponent == null)
            {
                packageVersionedComponent = new VersionedComponent()
                {
                    Component = packageComponent,
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    GameVersion = gameVersion,
                    Id = Guid.NewGuid(),
                    LockedMappingTypes = new List<LockingEntry>(),
                    Mappings = new List<CommittedMapping>(),
                    Metadata = null,
                    Proposals = new List<ProposedMapping>()
                };

                var packageVersionedComponentMetadata = new PackageMetadata()
                {
                    ChildPackages = new List<PackageMetadata>(),
                    Classes = new List<ClassMetadata>(),
                    Id = packageVersionedComponent.Id,
                    Parent = null,
                    VersionedComponent = packageVersionedComponent
                };

                packageVersionedComponent.Metadata = packageVersionedComponentMetadata;

                //Okey here is the special sauce for packages:
                //Looking up their parent.
                //Packages have no input mapping, but their directory structure is split via '/' so to get the parent (if it exists), it needs to strip of that last piece.
                var parentPackageNameEndIndex = package.Output.LastIndexOf("/", StringComparison.Ordinal);
                if (parentPackageNameEndIndex == -1)
                {
                    parentPackageNameEndIndex = 0;
                }
                else if (parentPackageNameEndIndex > 1)
                {
                    parentPackageNameEndIndex--;
                }

                var parentPackageName = package.Output.Substring(0, parentPackageNameEndIndex);
                //Now construct a new ExternalPackage with dummy data and process that to get the correct parent recursively.
                var dummyExternalParentPackage = new ExternalPackage()
                {
                    Classes = new List<ExternalClass>(),
                    Distribution = ExternalDistribution.UNKNOWN,
                    Documentation = "",
                    Input = "",
                    Output = parentPackageName
                };

                var parentPackageComponent = await this.HandlePackageData(componentDataManager, mappingType,
                    gameVersion, release, dummyExternalParentPackage, strategy);
                var parentPackageMetadata = parentPackageComponent?.Metadata as PackageMetadata;
                //Something might have failed an no parent was created, so do a null check just in case.
                if (parentPackageMetadata != null)
                {
                    parentPackageMetadata.ChildPackages.Add(packageVersionedComponentMetadata);
                    packageVersionedComponentMetadata.Parent = parentPackageMetadata;
                }
            }

            //Step 4, check if the package mapping already exists for the mapping type.
            //We need to filter both on input and output here.
            var packageMapping = packageVersionedComponent.Mappings.FirstOrDefault(m => m.InputMapping == package.Input && m.OutputMapping == package.Output && m.MappingType == mappingType);

            //Step 5, check if the mapping exists, if not create it.
            if (packageMapping == null)
            {
                packageMapping = new CommittedMapping()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    Distribution = ConvertExternalToInternalDistribution(package.Distribution),
                    Documentation = package.Documentation,
                    Id = Guid.NewGuid(),
                    InputMapping = package.Input,
                    MappingType = mappingType,
                    OutputMapping = package.Output,
                    ProposedMapping = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = packageVersionedComponent
                };

                packageVersionedComponent.Mappings.Add(packageMapping);
            }

            //Step 6, get the release link from the mapping
            var packageMappingReleaseComponent = packageMapping.Releases.FirstOrDefault(rc => rc.Release == release);

            //Step 7, check if the release is already associated with that mapping.
            //Else associate it with the mapping.
            if (packageMappingReleaseComponent == null)
            {
                packageMappingReleaseComponent = new ReleaseComponent()
                {
                    Id = Guid.NewGuid(),
                    Mapping = packageMapping,
                    Release = release
                };

                release.Components.Add(packageMappingReleaseComponent);
                packageMapping.Releases.Add(packageMappingReleaseComponent);
            }

            //Done.
            //The packages data and structure is copied into the apis structure.
            //Now save the data.
            if (createdNewComponent)
            {
                await componentDataManager.CreateComponent(packageComponent);
            }
            else
            {
                await componentDataManager.UpdateComponent(packageComponent);
            }

            return packageVersionedComponent;
        }

        private async Task<VersionedComponent> HandleClassData(IComponentDataManager componentDataManager,
            MappingType mappingType, GameVersion gameVersion, Release release, VersionedComponent package,
            ExternalClass @class, DeduplicationStrategy strategy)
        {
            //Get everything that matches the mapping.
            var existingCandidatesQuery = await FindAllComponentsFromMappingAndStrategy(componentDataManager, mappingType,
                @class.Input, @class.Output, strategy);

            //Now of all those mappings that match the mapping only get the class.
            var existingClassCandidatesQuery = existingCandidatesQuery.Where(c => c.Type == ComponentType.CLASS);

            //Either the class exists in that location, or not.
            //So check for an existing class.
            //If multiple exist we will still just take the first in case of class.
            var classComponent = existingClassCandidatesQuery.FirstOrDefault();
            var createdNewComponent = false;

            //Step 1, check for a new class.
            //If so create a new component for it.
            if (classComponent == null)
            {
                createdNewComponent = true;
                classComponent = new Component()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Type = ComponentType.CLASS,
                    VersionedComponents = new List<VersionedComponent>()
                };
            }

            //Step 2, get the versioned component from it.
            var classVersionedComponent =
                classComponent.VersionedComponents.FirstOrDefault(vc => vc.GameVersion == gameVersion);

            //Step 3, check for a new version in the same class.
            //If so create a new versioned component, and its metadata.
            if (classVersionedComponent == null)
            {
                classVersionedComponent = new VersionedComponent()
                {
                    Component = classComponent,
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    GameVersion = gameVersion,
                    Id = Guid.NewGuid(),
                    LockedMappingTypes = new List<LockingEntry>(),
                    Mappings = new List<CommittedMapping>(),
                    Metadata = null,
                    Proposals = new List<ProposedMapping>()
                };

                var newClassVersionedComponentMetadata = new ClassMetadata()
                {
                    Id = classVersionedComponent.Id,
                    VersionedComponent = classVersionedComponent,
                    Fields = new List<FieldMetadata>(),
                    InheritsFrom = new List<ClassMetadata>(),
                    InnerClasses = new List<ClassMetadata>(),
                    IsInheritedBy = new List<ClassMetadata>(),
                    Methods = new List<MethodMetadata>(),
                    Outer = null,
                    Package = package.Metadata as PackageMetadata
                };

                classVersionedComponent.Metadata = newClassVersionedComponentMetadata;
            }

            //This will never be null.
            var classVersionedComponentMetadata = classVersionedComponent.Metadata as ClassMetadata;
            if (classVersionedComponentMetadata == null)
                throw new ArgumentException("The given class data for: " + @class.Input + "<->" + @class.Output + " has no metadata!", nameof(@class));

            //Step 4, here is the special sauce for classes:
            //Looking up their outer class if need be.
            //Classes that have the '$' sign as an input, are input unique, as well as allow for an easy lookup :D
            //However we can only do this if the input is unique, which should always be the case for classes.... //TODO: keep an eye on this.
            if (classVersionedComponentMetadata.Outer == null && @class.Input.Contains("$") && (strategy == DeduplicationStrategy.INPUT_UNIQUE || strategy == DeduplicationStrategy.BOTH_UNIQUE))
            {
                //This can be done this way, $ is never the first character in java inner class names!
                var outerClassNameInput = @class.Input.Substring(0, @class.Input.LastIndexOf("$", StringComparison.Ordinal));
                var outerClassCandidatesQuery =
                    await componentDataManager.FindByInputMapping(mappingType.Name, outerClassNameInput);

                var outerClassCandidates = outerClassCandidatesQuery.ToList();
                if (outerClassCandidates.Count > 1)
                    throw new ArgumentException(
                        $"The given class seems to have a parent which exists twice in the database. The components do not line up: {outerClassNameInput} has {outerClassCandidates.Count} candidates!", nameof(@class));

                if (outerClassCandidates.Count == 1)
                {
                    //It exists, so grab the first no default needed.
                    var outerClassComponent = outerClassCandidates.First();
                    var outerClassVersionedComponent =
                        outerClassComponent.VersionedComponents.FirstOrDefault(vc => vc.GameVersion == gameVersion);

                    if (outerClassVersionedComponent != null)
                    {
                        var outerClassVersionedComponentMetadata =
                            ((ClassMetadata) outerClassVersionedComponent.Metadata);
                        classVersionedComponentMetadata.Outer = outerClassVersionedComponentMetadata;
                        outerClassVersionedComponentMetadata.InnerClasses.Add(outerClassVersionedComponentMetadata);
                    }
                }
            }
            //Additionally in this special sauce the lookup has to be done in the other direction, in case the inner classes where added by the system before the outer.
            if (classVersionedComponentMetadata.InnerClasses.Count == 0 &&
                (strategy == DeduplicationStrategy.INPUT_UNIQUE || strategy == DeduplicationStrategy.BOTH_UNIQUE))
            {
                var innerClassInputNameRegex = @class.Input + "\\$(.+)";
                var innerClassComponentCandidates =
                    await componentDataManager.FindByInputMapping(mappingType.Name, innerClassInputNameRegex);

                var innerClassMetadata = innerClassComponentCandidates
                    .Select(c => c.VersionedComponents.FirstOrDefault(vc => vc.GameVersion == gameVersion))
                    .Where(vc => vc != null).Select(vc => vc.Metadata as ClassMetadata).Where(m => m != null);

                classVersionedComponentMetadata.InnerClasses.AddRange(innerClassMetadata);
            }

            //Step 5, check if the class mapping already exists for the mapping type.
            //We need to filter both on input and output here.
            var classMapping = classVersionedComponent.Mappings.FirstOrDefault(m => m.InputMapping == @class.Input && m.OutputMapping == @class.Output && m.MappingType == mappingType);

            //Step 6, check if the mapping exists, if not create it.
            if (classMapping == null)
            {
                classMapping = new CommittedMapping()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    Distribution = ConvertExternalToInternalDistribution(@class.Distribution),
                    Documentation = @class.Documentation,
                    Id = Guid.NewGuid(),
                    InputMapping = @class.Input,
                    MappingType = mappingType,
                    OutputMapping = @class.Output,
                    ProposedMapping = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = classVersionedComponent
                };

                classVersionedComponent.Mappings.Add(classMapping);
            }

            //Step 7, get the release link from the mapping
            var classMappingReleaseComponent = classMapping.Releases.FirstOrDefault(rc => rc.Release == release);

            //Step 8, check if the release is already associated with that mapping.
            //Else associate it with the mapping.
            if (classMappingReleaseComponent == null)
            {
                classMappingReleaseComponent = new ReleaseComponent()
                {
                    Id = Guid.NewGuid(),
                    Mapping = classMapping,
                    Release = release
                };

                release.Components.Add(classMappingReleaseComponent);
                classMapping.Releases.Add(classMappingReleaseComponent);
            }

            //Done.
            //The classes data and structure is copied into the apis structure.
            //Now save the data.
            if (createdNewComponent)
            {
                await componentDataManager.CreateComponent(classComponent);
            }
            else
            {
                await componentDataManager.UpdateComponent(classComponent);
            }

            return classVersionedComponent;

        }

        private async Task<VersionedComponent> HandleMethodData(IComponentDataManager componentDataManager,
            MappingType mappingType, GameVersion gameVersion, Release release, VersionedComponent @class,
            ExternalMethod method, DeduplicationStrategy strategy)
        {
            //Get everything that matches the mapping.
            var existingCandidatesQuery = await FindAllComponentsFromMappingAndStrategy(componentDataManager, mappingType,
                method.Input, method.Output, strategy);

            //Now of all those mappings that match the mapping only get the method.
            var existingMethodCandidatesQuery = existingCandidatesQuery.Where(c => c.Type == ComponentType.METHOD);

            //Either the method exists in that location, or not.
            //So check for an existing method.
            //If multiple exist we will still just take the first in case of method.
            var methodComponent = existingMethodCandidatesQuery.FirstOrDefault();
            var createdNewComponent = false;

            //Step 1, check for a new method.
            //If so create a new component for it.
            if (methodComponent == null)
            {
                createdNewComponent = true;
                methodComponent = new Component()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Type = ComponentType.METHOD,
                    VersionedComponents = new List<VersionedComponent>()
                };
            }

            //Step 2, get the versioned component from it.
            var methodVersionedComponent =
                methodComponent.VersionedComponents.FirstOrDefault(vc => vc.GameVersion == gameVersion);

            //Step 3, check for a new version in the same method.
            //If so create a new versioned component, and its metadata.
            if (methodVersionedComponent == null)
            {
                methodVersionedComponent = new VersionedComponent()
                {
                    Component = methodComponent,
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    GameVersion = gameVersion,
                    Id = Guid.NewGuid(),
                    LockedMappingTypes = new List<LockingEntry>(),
                    Mappings = new List<CommittedMapping>(),
                    Metadata = null,
                    Proposals = new List<ProposedMapping>()
                };

                var newMethodVersionedComponentMetadata = new MethodMetadata()
                {
                    Id = methodVersionedComponent.Id,
                    VersionedComponent = methodVersionedComponent,
                    Descriptor = method.Descriptor,
                    IsStatic = method.IsStatic,
                    MemberOf = @class.Metadata as ClassMetadata,
                    Parameters = new List<ParameterMetadata>()
                };

                methodVersionedComponent.Metadata = newMethodVersionedComponentMetadata;
            }

            //Step 4, check if the method mapping already exists for the mapping type.
            //We need to filter both on input and output here.
            var methodMapping = methodVersionedComponent.Mappings.FirstOrDefault(m => m.InputMapping == method.Input && m.OutputMapping == method.Output && m.MappingType == mappingType);

            //Step 5, check if the mapping exists, if not create it.
            if (methodMapping == null)
            {
                methodMapping = new CommittedMapping()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    Distribution = ConvertExternalToInternalDistribution(method.Distribution),
                    Documentation = method.Documentation,
                    Id = Guid.NewGuid(),
                    InputMapping = method.Input,
                    MappingType = mappingType,
                    OutputMapping = method.Output,
                    ProposedMapping = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = methodVersionedComponent
                };

                methodVersionedComponent.Mappings.Add(methodMapping);
            }

            //Step 6, get the release link from the mapping
            var methodMappingReleaseComponent = methodMapping.Releases.FirstOrDefault(rc => rc.Release == release);

            //Step 7, check if the release is already associated with that mapping.
            //Else associate it with the mapping.
            if (methodMappingReleaseComponent == null)
            {
                methodMappingReleaseComponent = new ReleaseComponent()
                {
                    Id = Guid.NewGuid(),
                    Mapping = methodMapping,
                    Release = release
                };

                release.Components.Add(methodMappingReleaseComponent);
                methodMapping.Releases.Add(methodMappingReleaseComponent);
            }

            //Done.
            //The methodes data and structure is copied into the apis structure.
            //Now save the data.
            if (createdNewComponent)
            {
                await componentDataManager.CreateComponent(methodComponent);
            }
            else
            {
                await componentDataManager.UpdateComponent(methodComponent);
            }

            return methodVersionedComponent;

        }

        private async Task<VersionedComponent> HandleFieldData(IComponentDataManager componentDataManager,
            MappingType mappingType, GameVersion gameVersion, Release release, VersionedComponent @class,
            ExternalField field, DeduplicationStrategy strategy)
        {
            //Get everything that matches the mapping.
            var existingCandidatesQuery = await FindAllComponentsFromMappingAndStrategy(componentDataManager, mappingType,
                field.Input, field.Output, strategy);

            //Now of all those mappings that match the mapping only get the field.
            var existingFieldCandidatesQuery = existingCandidatesQuery.Where(c => c.Type == ComponentType.FIELD);

            //Either the field exists in that location, or not.
            //So check for an existing field.
            //If multiple exist we will still just take the first in case of field.
            var fieldComponent = existingFieldCandidatesQuery.FirstOrDefault();
            var createdNewComponent = false;

            //Step 1, check for a new field.
            //If so create a new component for it.
            if (fieldComponent == null)
            {
                createdNewComponent = true;
                fieldComponent = new Component()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Type = ComponentType.FIELD,
                    VersionedComponents = new List<VersionedComponent>()
                };
            }

            //Step 2, get the versioned component from it.
            var fieldVersionedComponent =
                fieldComponent.VersionedComponents.FirstOrDefault(vc => vc.GameVersion == gameVersion);

            //Step 3, check for a new version in the same field.
            //If so create a new versioned component, and its metadata.
            if (fieldVersionedComponent == null)
            {
                fieldVersionedComponent = new VersionedComponent()
                {
                    Component = fieldComponent,
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    GameVersion = gameVersion,
                    Id = Guid.NewGuid(),
                    LockedMappingTypes = new List<LockingEntry>(),
                    Mappings = new List<CommittedMapping>(),
                    Metadata = null,
                    Proposals = new List<ProposedMapping>()
                };

                var newFieldVersionedComponentMetadata = new FieldMetadata()
                {
                    Id = fieldVersionedComponent.Id,
                    VersionedComponent = fieldVersionedComponent,
                    IsStatic = field.IsStatic,
                    MemberOf = @class.Metadata as ClassMetadata,
                    Type = field.Type
                };

                fieldVersionedComponent.Metadata = newFieldVersionedComponentMetadata;
            }

            //Step 4, check if the field mapping already exists for the mapping type.
            //We need to filter both on input and output here.
            var fieldMapping = fieldVersionedComponent.Mappings.FirstOrDefault(m => m.InputMapping == field.Input && m.OutputMapping == field.Output && m.MappingType == mappingType);

            //Step 5, check if the mapping exists, if not create it.
            if (fieldMapping == null)
            {
                fieldMapping = new CommittedMapping()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    Distribution = ConvertExternalToInternalDistribution(field.Distribution),
                    Documentation = field.Documentation,
                    Id = Guid.NewGuid(),
                    InputMapping = field.Input,
                    MappingType = mappingType,
                    OutputMapping = field.Output,
                    ProposedMapping = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = fieldVersionedComponent
                };

                fieldVersionedComponent.Mappings.Add(fieldMapping);
            }

            //Step 6, get the release link from the mapping
            var fieldMappingReleaseComponent = fieldMapping.Releases.FirstOrDefault(rc => rc.Release == release);

            //Step 7, check if the release is already associated with that mapping.
            //Else associate it with the mapping.
            if (fieldMappingReleaseComponent == null)
            {
                fieldMappingReleaseComponent = new ReleaseComponent()
                {
                    Id = Guid.NewGuid(),
                    Mapping = fieldMapping,
                    Release = release
                };

                release.Components.Add(fieldMappingReleaseComponent);
                fieldMapping.Releases.Add(fieldMappingReleaseComponent);
            }

            //Done.
            //The fieldes data and structure is copied into the apis structure.
            //Now save the data.
            if (createdNewComponent)
            {
                await componentDataManager.CreateComponent(fieldComponent);
            }
            else
            {
                await componentDataManager.UpdateComponent(fieldComponent);
            }

            return fieldVersionedComponent;

        }

        private async Task<VersionedComponent> HandleParameterData(IComponentDataManager componentDataManager,
            MappingType mappingType, GameVersion gameVersion, Release release, VersionedComponent methodVersionedComponent,
            ExternalMethod externalMethod,
            ExternalParameter parameter, DeduplicationStrategy strategy)
        {
            //Get everything that matches the mapping.
            var existingCandidatesQuery = await FindAllComponentsFromMappingAndStrategy(componentDataManager, mappingType,
                parameter.Input, parameter.Output, strategy);

            //Now of all those mappings that match the mapping only get the parameter.
            var existingParameterCandidatesQuery = existingCandidatesQuery.Where(c => c.Type == ComponentType.PARAMETER);

            //Either the parameter exists in that location, or not.
            //So check for an existing parameter.
            //If multiple exist we will still just take the first in case of parameter.
            var parameterComponent = existingParameterCandidatesQuery.FirstOrDefault();
            var createdNewComponent = false;

            //Step 1, check for a new parameter.
            //If so create a new component for it.
            if (parameterComponent == null)
            {
                createdNewComponent = true;
                parameterComponent = new Component()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Type = ComponentType.PARAMETER,
                    VersionedComponents = new List<VersionedComponent>()
                };
            }

            //Step 2, get the versioned component from it.
            var parameterVersionedComponent =
                parameterComponent.VersionedComponents.FirstOrDefault(vc => vc.GameVersion == gameVersion);

            //Step 3, check for a new version in the same parameter.
            //If so create a new versioned component, and its metadata.
            if (parameterVersionedComponent == null)
            {
                parameterVersionedComponent = new VersionedComponent()
                {
                    Component = parameterComponent,
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    GameVersion = gameVersion,
                    Id = Guid.NewGuid(),
                    LockedMappingTypes = new List<LockingEntry>(),
                    Mappings = new List<CommittedMapping>(),
                    Metadata = null,
                    Proposals = new List<ProposedMapping>()
                };

                //The special source for parameters is in here:
                //The system needs to calculate the index from the parameter list.
                var newParameterVersionedComponentMetadata = new ParameterMetadata()
                {
                    Id = parameterVersionedComponent.Id,
                    VersionedComponent = parameterVersionedComponent,
                    Index = externalMethod.Parameters.OrderBy(p => p.Input).ToList().IndexOf(parameter),
                    ParameterOf = methodVersionedComponent.Metadata as MethodMetadata
                };

                parameterVersionedComponent.Metadata = newParameterVersionedComponentMetadata;
            }

            //Step 4, check if the parameter mapping already exists for the mapping type.
            //We need to filter both on input and output here.
            var parameterMapping = parameterVersionedComponent.Mappings.FirstOrDefault(m => m.InputMapping == parameter.Input && m.OutputMapping == parameter.Output && m.MappingType == mappingType);

            //Step 5, check if the mapping exists, if not create it.
            if (parameterMapping == null)
            {
                parameterMapping = new CommittedMapping()
                {
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    Distribution = ConvertExternalToInternalDistribution(parameter.Distribution),
                    Documentation = parameter.Documentation,
                    Id = Guid.NewGuid(),
                    InputMapping = parameter.Input,
                    MappingType = mappingType,
                    OutputMapping = parameter.Output,
                    ProposedMapping = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = parameterVersionedComponent
                };

                parameterVersionedComponent.Mappings.Add(parameterMapping);
            }

            //Step 6, get the release link from the mapping
            var parameterMappingReleaseComponent = parameterMapping.Releases.FirstOrDefault(rc => rc.Release == release);

            //Step 7, check if the release is already associated with that mapping.
            //Else associate it with the mapping.
            if (parameterMappingReleaseComponent == null)
            {
                parameterMappingReleaseComponent = new ReleaseComponent()
                {
                    Id = Guid.NewGuid(),
                    Mapping = parameterMapping,
                    Release = release
                };

                release.Components.Add(parameterMappingReleaseComponent);
                parameterMapping.Releases.Add(parameterMappingReleaseComponent);
            }

            //Done.
            //The parameter data and structure is copied into the apis structure.
            //Now save the data.
            if (createdNewComponent)
            {
                await componentDataManager.CreateComponent(parameterComponent);
            }
            else
            {
                await componentDataManager.UpdateComponent(parameterComponent);
            }

            return parameterVersionedComponent;
        }

        private Distribution ConvertExternalToInternalDistribution(ExternalDistribution distribution)
        {
            switch (distribution)
            {
                case ExternalDistribution.BOTH:
                    return Distribution.BOTH;
                case ExternalDistribution.SERVER_ONLY:
                    return Distribution.SERVER_ONLY;
                case ExternalDistribution.CLIENT_ONLY:
                    return Distribution.CLIENT_ONLY;
                case ExternalDistribution.UNKNOWN:
                    return Distribution.UNKNOWN;
                default:
                    return Distribution.UNKNOWN;
            }
        }
    }
}
