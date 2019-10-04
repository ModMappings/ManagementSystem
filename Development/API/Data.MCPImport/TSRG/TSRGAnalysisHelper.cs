using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.MCPImport.TSRG
{

    /// <summary>
    /// This class handles the creation and modification of MCMS data.
    /// It is specifically designed to handle TSRG data.
    /// As such no parameter mappings exists.
    /// </summary>
    public class TSRGAnalysisHelper
    {
        private readonly MCMSContext _context;
        private readonly List<Component> _newClassData;
        private readonly Release _release;
        private readonly GameVersion _gameVersion;
        private readonly MappingType _tsrgMappingType;

        private bool _currentIsNew = false;
        private Component _currentClass = null;
        private VersionedComponent _currentVersionedClass = null;

        public TSRGAnalysisHelper(MCMSContext context, ref List<Component> newClassData, Release release,
            GameVersion gameVersion, MappingType tsrgMappingType)
        {
            _context = context;
            _newClassData = newClassData;
            _release = release;
            _gameVersion = gameVersion;
            _tsrgMappingType = tsrgMappingType;
        }

        public async Task StartNewClass(string inputMapping, string outputMapping, string packageName)
        {
            await FinalizeCurrentClass();

            //outputMapping in TSRG is unique. Use this to detect if a new class is being imported.
            _currentClass = await _context.Components
                .Include(c => c.VersionedComponents)
                .Include("VersionedComponents.GameVersion")
                .Include("VersionedComponents.Mappings")
                .Include("VersionedComponents.Mappings.Releases")
                .Include("VersionedComponents.Mappings.Releases.Release")
                .Include("VersionedComponents.Proposals")
                .Include("VersionedComponents.Metadata")
                .Include("VersionedComponents.Metadata.Fields")
                .Include("VersionedComponents.Metadata.Methods")
                .Include("VersionedComponents.Metadata.InheritsFrom")
                .Include("VersionedComponents.Metadata.Outer")
                .Include("VersionedComponents.Metadata.Methods")
                .FirstOrDefaultAsync(c => c.Type == ComponentType.CLASS &&
                                          c.VersionedComponents.Any(vc =>
                                              vc.Mappings.Any(m =>
                                                  m.MappingType ==
                                                  _tsrgMappingType &&
                                                  m.OutputMapping ==
                                                  outputMapping)));

            if (_currentClass == null)
            {
                _currentClass = _newClassData.FirstOrDefault(c => c.Type == ComponentType.CLASS &&
                                                                  c.VersionedComponents.Any(vc =>
                                                                      vc.Mappings.Any(m =>
                                                                          m.MappingType ==
                                                                          _tsrgMappingType &&
                                                                          m.OutputMapping ==
                                                                          outputMapping)));

                if (_currentClass == null)
                {
                    _currentIsNew = true;
                    _currentClass = new Component
                    {
                        Id = Guid.NewGuid(),
                        Type = ComponentType.CLASS,
                        VersionedComponents = new List<VersionedComponent>()
                    };
                }
            }

            _currentVersionedClass =
                _currentClass.VersionedComponents.FirstOrDefault(vc => vc.GameVersion != _gameVersion);

            if (_currentVersionedClass == null)
            {
                _currentVersionedClass = new VersionedComponent
                {
                    Id = Guid.NewGuid(),
                    Component = _currentClass,
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    GameVersion = _gameVersion,
                    LockedMappingTypes = new List<LockingEntry>(),
                    Mappings = new List<LiveMappingEntry>(),
                    Metadata = null,
                    Proposals = new List<ProposalMappingEntry>()
                };

                _currentVersionedClass.Metadata = new ClassMetadata
                {
                    Fields = new List<FieldMetadata>(),
                    InheritsFrom = new List<ClassMetadata>(),
                    Methods = new List<MethodMetadata>(),
                    Outer = null, //TODO: Do splitting on the name and attempt lookup. (Might need to happen in the post processing, possibly)
                    Package = packageName,
                    VersionedComponent = _currentVersionedClass,
                    VersionedComponentForeignKey = _currentVersionedClass.Id
                };

                _currentClass.VersionedComponents.Add(_currentVersionedClass);
            }

            //Create the mapping (if needed) and associate the release
            HandleMappingCreation(inputMapping, outputMapping, _currentVersionedClass);
        }

        public Task FinalizeCurrentClass()
        {
            if (_currentClass == null)
                return Task.CompletedTask;

            if (_currentIsNew)
                _newClassData.Add(_currentClass);

            _currentClass = null;
            _currentIsNew = false;

            return Task.CompletedTask;
        }

        public void AddMethod(string inputMapping, string outputMapping, string descriptor, bool isStatic)
        {
            if (!(_currentVersionedClass.Metadata is ClassMetadata classMetadata))
                throw new InvalidOperationException("The current versioned component does not contain class metadata");

            var targetMethodVersionedComponent = classMetadata.Methods.Select(m => m.VersionedComponent).FirstOrDefault(
                vc => vc.Mappings.Any(m => m.MappingType == _tsrgMappingType && m.OutputMapping == outputMapping));

            //We need to create a new component and versioned component.
            //Then register the versioned components metadata properly in the class.
            if (targetMethodVersionedComponent == null)
            {
                var newMethodComponent = new Component
                {
                    Id = Guid.NewGuid(),
                    Type = ComponentType.METHOD,
                    VersionedComponents = new List<VersionedComponent>()
                };

                targetMethodVersionedComponent = new VersionedComponent
                {
                    Id = Guid.NewGuid(),
                    Component = newMethodComponent,
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    GameVersion = _gameVersion,
                    LockedMappingTypes = new List<LockingEntry>(),
                    Mappings = new List<LiveMappingEntry>(),
                    Metadata = null,
                    Proposals = new List<ProposalMappingEntry>()
                };

                var newMethodMetadata = new MethodMetadata
                {
                    Descriptor = descriptor,
                    IsStatic = isStatic,
                    MemberOf = classMetadata,
                    Parameters = new List<ParameterMetadata>(),
                    VersionedComponent = targetMethodVersionedComponent,
                    VersionedComponentForeignKey = targetMethodVersionedComponent.Id
                };

                targetMethodVersionedComponent.Metadata = newMethodMetadata;
                classMetadata.Methods.Add(newMethodMetadata);
            }

            //Create the mapping (if needed) and associate the release
            HandleMappingCreation(inputMapping, outputMapping, targetMethodVersionedComponent);
        }

        public void AddField(string inputMapping, string outputMapping, bool isStatic)
        {
            if (!(_currentVersionedClass.Metadata is ClassMetadata classMetadata))
                throw new InvalidOperationException("The current versioned component does not contain class metadata");

            var targetFieldVersionedComponent = classMetadata.Fields.Select(m => m.VersionedComponent).FirstOrDefault(
                vc => vc.Mappings.Any(m => m.MappingType == _tsrgMappingType && m.OutputMapping == outputMapping));

            //We need to create a new component and versioned component.
            //Then register the versioned components metadata properly in the class.
            if (targetFieldVersionedComponent == null)
            {
                var newFieldComponent = new Component
                {
                    Id = Guid.NewGuid(),
                    Type = ComponentType.FIELD,
                    VersionedComponents = new List<VersionedComponent>()
                };

                targetFieldVersionedComponent = new VersionedComponent
                {
                    Id = Guid.NewGuid(),
                    Component = newFieldComponent,
                    CreatedBy = Guid.Empty,
                    CreatedOn = DateTime.Now,
                    GameVersion = _gameVersion,
                    LockedMappingTypes = new List<LockingEntry>(),
                    Mappings = new List<LiveMappingEntry>(),
                    Metadata = null,
                    Proposals = new List<ProposalMappingEntry>()
                };

                var newFieldMetadata = new FieldMetadata
                {
                    IsStatic = isStatic,
                    MemberOf = classMetadata,
                    VersionedComponent = targetFieldVersionedComponent,
                    VersionedComponentForeignKey = targetFieldVersionedComponent.Id
                };

                targetFieldVersionedComponent.Metadata = newFieldMetadata;
                classMetadata.Fields.Add(newFieldMetadata);
            }

            //Create the mapping (if needed) and associate the release
            HandleMappingCreation(inputMapping, outputMapping, targetFieldVersionedComponent);
        }

        private void HandleMappingCreation(string inputMapping, string outputMapping,
            VersionedComponent versionedComponent)
        {
            var liveMapping = versionedComponent.Mappings.FirstOrDefault(m =>
                m.MappingType == _tsrgMappingType && m.OutputMapping == outputMapping);

            if (liveMapping == null)
            {
                liveMapping = new LiveMappingEntry
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    Distribution = Distribution.UNKNOWN,
                    Documentation = "",
                    InputMapping = inputMapping,
                    MappingType = _tsrgMappingType,
                    OutputMapping = outputMapping,
                    Proposal = null,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = versionedComponent
                };

                versionedComponent.Mappings.Add(liveMapping);
            }

            var releaseComponent = liveMapping.Releases.FirstOrDefault(r => r.Release == _release);

            if (releaseComponent == null)
            {
                releaseComponent = new ReleaseComponent
                {
                    Id = Guid.NewGuid(),
                    ComponentType = versionedComponent.Component.Type,
                    Member = liveMapping,
                    Release = _release
                };

                liveMapping.Releases.Add(releaseComponent);
            }
        }
    }
}