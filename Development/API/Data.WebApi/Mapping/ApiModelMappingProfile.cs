using System.Linq;
using AutoMapper;
using Data.WebApi.Model.Api.Core;
using Data.WebApi.Model.Api.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Core;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Mapping.Metadata;

namespace Data.WebApi.Mapping
{
    public class ApiModelMappingProfile
        : Profile
    {
        public ApiModelMappingProfile()
        {
            SetupComponentToDtoMapping();
            SetupDtoToComponentMapping();

            SetupVersionedComponentToDtoMapping();
            SetupPackageMetadataToDtoMapping();
            SetupClassMetadataToDtoMapping();
            SetupMethodMetadataToDtoMapping();
            SetupFieldMetadataToDtoMapping();
            SetupParameterMetadataToDtoMapping();
            SetupDtoToMetadataBaseMapping();
            SetupDtoToVersionedComponentMapping();
            SetupDtoToPackageMetadataMapping();
            SetupDtoToClassMetadataMapping();
            SetupDtoToMethodMetadataMapping();
            SetupDtoToFieldMetadataMapping();
            SetupDtoToParameterMetadataMapping();

            SetupGameVersionToDtoMapping();
            SetupDtoToGameVersionMapping();

            SetupMappingTypeToDtoMapping();
            SetupDtoToMappingTypeMapping();
        }

        private void SetupComponentToDtoMapping()
        {
            var componentToDtoMap = CreateMap<Component, ComponentDto>();
            componentToDtoMap.ForAllMembers(d => d.Ignore());
            componentToDtoMap.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            componentToDtoMap.ForMember(d => d.Type,
                opts => opts.MapFrom(d => d.Type));
            componentToDtoMap.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            componentToDtoMap.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            componentToDtoMap.ForMember(c => c.VersionedComponents,
                opts => opts.MapFrom(
                    map => map.VersionedComponents.Select(vc => vc.Id).ToHashSet()));
        }

        private void SetupDtoToComponentMapping()
        {
            var dtoToComponentMap = CreateMap<ComponentDto, Component>();
            dtoToComponentMap.ForAllMembers(opt => opt.Ignore());
            dtoToComponentMap.ForMember(dto => dto.Type, opt => opt.MapFrom(src => src.Type));
            dtoToComponentMap.ForMember(d => d.VersionedComponents,
                opts => opts.MapFrom(d =>
                    d.VersionedComponents.Select(id => new VersionedComponent() {Id = id}).ToList()));
        }

        private void SetupVersionedComponentToDtoMapping()
        {
            var versionedComponentToDtoMap = CreateMap<VersionedComponent, VersionedComponentDto>();
            versionedComponentToDtoMap.ForAllMembers(d => d.Ignore());
            versionedComponentToDtoMap.ForMember(d => d.Id,
                opts => opts.MapFrom(c => c.Id));
            versionedComponentToDtoMap.ForMember(d => d.Type,
                opts => opts.MapFrom(c => c.Component.Type));
            versionedComponentToDtoMap.ForMember(d => d.GameVersion,
                opts => opts.MapFrom(c => c.GameVersion.Name));
            versionedComponentToDtoMap.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(c => c.CreatedBy));
            versionedComponentToDtoMap.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(c => c.CreatedOn));
            versionedComponentToDtoMap.ForMember(d => d.Component,
                opts => opts.MapFrom(c => c.Component.Id));
            versionedComponentToDtoMap.ForMember(d => d.Mappings,
                opts => opts.MapFrom(c => c.Mappings.Select(m => m.Id).ToHashSet()));
            versionedComponentToDtoMap.ForMember(d => d.Proposals,
                opts => opts.MapFrom(c => c.Proposals.Select(p => p.Id).ToHashSet()));
            versionedComponentToDtoMap.ForMember(d => d.LockedMappingTypes,
                opts => opts.MapFrom(c => c.LockedMappingTypes.Select(l => l.MappingType.Name).ToHashSet()));
        }

        private void SetupPackageMetadataToDtoMapping()
        {
            var packageMetadataToDtoMapping = CreateMap<PackageMetadata, VersionedComponentDto>();
            packageMetadataToDtoMapping.ForAllMembers(d => d.Ignore());
            packageMetadataToDtoMapping.ForMember(d => d.MemberOf,
                opts => { opts.MapFrom(v => v.Parent.Id); });
            packageMetadataToDtoMapping.ForMember(d => d.ChildPackages,
                opts =>
                {
                    opts.MapFrom(v =>
                        v.ChildPackages.Select(p => p.Id).ToHashSet());
                });
            packageMetadataToDtoMapping.ForMember(d => d.Classes,
                opts => { opts.MapFrom(v => v.Classes.Select(c => c.Id).ToHashSet()); });
        }

        private void SetupClassMetadataToDtoMapping()
        {
            var classMetadataToDtoMapping = CreateMap<ClassMetadata, VersionedComponentDto>();
            classMetadataToDtoMapping.ForAllMembers(d => d.Ignore());
            classMetadataToDtoMapping.ForMember(d => d.MemberOf,
                opts => { opts.MapFrom(v => v.Package.Id); });
            classMetadataToDtoMapping.ForMember(d => d.Classes,
                opts => { opts.MapFrom(c => c.InnerClasses.Select(i => i.Id).ToHashSet()); });
            classMetadataToDtoMapping.ForMember(d => d.OuterClass,
                opts => { opts.MapFrom(c => c.Outer.VersionedComponent); });
            classMetadataToDtoMapping.ForMember(d => d.InheritsFrom,
                opts =>
                {
                    opts.MapFrom(v =>
                        v.InheritsFrom.Select(p => p.Id).ToHashSet());
                });
            classMetadataToDtoMapping.ForMember(d => d.IsInheritedBy,
                opts =>
                {
                    opts.MapFrom(v =>
                        v.IsInheritedBy.Select(p => p.Id).ToHashSet());
                });
            classMetadataToDtoMapping.ForMember(p => p.Methods,
                opts => { opts.MapFrom(v => v.Methods.Select(m => m.Id).ToHashSet()); });
            classMetadataToDtoMapping.ForMember(p => p.Fields,
                opts => opts.MapFrom(v => v.Fields.Select(f => f.Id).ToHashSet()));
        }

        private void SetupMethodMetadataToDtoMapping()
        {
            var methodMetadataToDtoMapping = CreateMap<MethodMetadata, VersionedComponentDto>();
            methodMetadataToDtoMapping.ForAllMembers(d => d.Ignore());
            methodMetadataToDtoMapping.ForMember(d => d.MemberOf,
                opts => { opts.MapFrom(v => v.MemberOf.Id); });
            methodMetadataToDtoMapping.ForMember(d => d.Parameters,
                opts =>
                {
                    opts.MapFrom(v =>
                        v.Parameters.Select(p => p.Id).ToHashSet());
                });
            methodMetadataToDtoMapping.ForMember(d => d.IsStatic,
                opts => { opts.MapFrom(v => v.IsStatic); });
            methodMetadataToDtoMapping.ForMember(d => d.Descriptor,
                opts => { opts.MapFrom(v => v.Descriptor); });
        }

        private void SetupFieldMetadataToDtoMapping()
        {
            var fieldMetadataToDtoMapping = CreateMap<FieldMetadata, VersionedComponentDto>();
            fieldMetadataToDtoMapping.ForAllMembers(d => d.Ignore());
            fieldMetadataToDtoMapping.ForMember(d => d.MemberOf,
                opts => { opts.MapFrom(v => v.MemberOf.Id); });
            fieldMetadataToDtoMapping.ForMember(d => d.IsStatic,
                opts => { opts.MapFrom(v => v.IsStatic); });
        }

        private void SetupParameterMetadataToDtoMapping()
        {
            var parameterMetadataToDtoMapping = CreateMap<ParameterMetadata, VersionedComponentDto>();
            parameterMetadataToDtoMapping.ForAllMembers(d => d.Ignore());
            parameterMetadataToDtoMapping.ForMember(d => d.MemberOf,
                opts => { opts.MapFrom(v => v.ParameterOf.Id); });
            parameterMetadataToDtoMapping.ForMember(d => d.Index,
                opts => { opts.MapFrom(v => v.Index); });
        }

        private void SetupDtoToVersionedComponentMapping()
        {
            var dtoToVersionedComponentMap = CreateMap<VersionedComponentDto, VersionedComponent>();
            dtoToVersionedComponentMap.ForAllMembers(opt => opt.Ignore());
            dtoToVersionedComponentMap.ForMember(d => d.Mappings,
                opt => { opt.MapFrom(d => d.Mappings.Select(m => new CommittedMapping {Id = m}).ToList()); });
            dtoToVersionedComponentMap.ForMember(d => d.Proposals,
                opt => { opt.MapFrom(d => d.Proposals.Select(p => new ProposedMapping {Id = p}).ToList()); });
            dtoToVersionedComponentMap.ForMember(d => d.LockedMappingTypes,
                opt =>
                {
                    opt.MapFrom(d =>
                        d.LockedMappingTypes.Select(
                            t => new LockingEntry {MappingType = new MappingType {Id = t}}));
                });
        }

        private void SetupDtoToMetadataBaseMapping()
        {
            var dtoToMetadataBaseMapping = CreateMap<VersionedComponentDto, MetadataBase>();
            dtoToMetadataBaseMapping.ConstructUsing(vc =>
                vc.Type == ComponentType.PACKAGE
                    ? (MetadataBase) new PackageMetadata()
                    : vc.Type == ComponentType.CLASS
                        ? (MetadataBase) new ClassMetadata()
                        : vc.Type == ComponentType.METHOD
                            ? (MetadataBase) new MethodMetadata()
                            : vc.Type == ComponentType.FIELD
                                ? (MetadataBase) new FieldMetadata()
                                : vc.Type == ComponentType.PARAMETER
                                    ? (MetadataBase) new PackageMetadata()
                                    : null);
            dtoToMetadataBaseMapping.IncludeAllDerived();
            dtoToMetadataBaseMapping.ForMember(d => d.Id, opts => opts.Ignore());
            dtoToMetadataBaseMapping.ForMember(d => d.VersionedComponent, opts => opts.Ignore());
        }

        private void SetupDtoToPackageMetadataMapping()
        {
            var dtoToPackageMetadataMapping = CreateMap<VersionedComponentDto, PackageMetadata>();
            dtoToPackageMetadataMapping.IncludeBase<VersionedComponentDto, MetadataBase>();
            dtoToPackageMetadataMapping.ForMember(d => d.Classes,
                opts => opts.MapFrom(d =>
                    d.Classes.Select(id => new ClassMetadata {Id = id}).ToList()));
            dtoToPackageMetadataMapping.ForMember(d => d.ChildPackages,
                opts => opts.MapFrom(d =>
                    d.ChildPackages.Select(id => new PackageMetadata {Id = id}).ToList()));
            dtoToPackageMetadataMapping.ForMember(d => d.Parent,
                opts => opts.MapFrom(d => d.MemberOf.HasValue ? new PackageMetadata() {Id = d.MemberOf.Value} : null));
        }

        private void SetupDtoToClassMetadataMapping()
        {
            var dtoToClassMetadataMapping = CreateMap<VersionedComponentDto, ClassMetadata>();
            dtoToClassMetadataMapping.IncludeBase<VersionedComponentDto, MetadataBase>();
            dtoToClassMetadataMapping.ForMember(d => d.Outer,
                opts => opts.MapFrom(d => new ClassMetadata {Id = d.OuterClass.Value}));
            dtoToClassMetadataMapping.ForMember(d => d.InnerClasses,
                opts => opts.MapFrom(d => d.Classes.Select(id => new ClassMetadata {Id = id}).ToList()));
            dtoToClassMetadataMapping.ForMember(d => d.InheritsFrom,
                opts => opts.MapFrom(d => d.InheritsFrom.Select(id => new ClassMetadata {Id = id}).ToList()));
            dtoToClassMetadataMapping.ForMember(d => d.IsInheritedBy,
                opts => opts.MapFrom(d => d.IsInheritedBy.Select(id => new ClassMetadata {Id = id}).ToList()));
            dtoToClassMetadataMapping.ForMember(d => d.Package,
                opts => opts.MapFrom(d => new PackageMetadata() {Id = d.MemberOf.Value}));
            dtoToClassMetadataMapping.ForMember(d => d.Methods,
                opts => opts.MapFrom(d => d.Methods.Select(id => new MethodMetadata() {Id = id}).ToList()));
            dtoToClassMetadataMapping.ForMember(d => d.Fields,
                opts => opts.MapFrom(d => d.Fields.Select(id => new FieldMetadata() {Id = id}).ToList()));
        }

        private void SetupDtoToMethodMetadataMapping()
        {
            var dtoToMethodMetadataMapping = CreateMap<VersionedComponentDto, MethodMetadata>();
            dtoToMethodMetadataMapping.IncludeBase<VersionedComponentDto, MetadataBase>();
            dtoToMethodMetadataMapping.ForMember(d => d.MemberOf,
                opts => opts.MapFrom(d => new ClassMetadata() {Id = d.MemberOf.Value}));
            dtoToMethodMetadataMapping.ForMember(d => d.IsStatic,
                opts => opts.MapFrom(d => d.IsStatic));
            dtoToMethodMetadataMapping.ForMember(d => d.Parameters,
                opts => opts.MapFrom(d => d.Parameters.Select(id => new ParameterMetadata() {Id = id}).ToList()));
            dtoToMethodMetadataMapping.ForMember(d => d.Descriptor,
                opts => opts.MapFrom(d => d.Descriptor));
        }

        private void SetupDtoToFieldMetadataMapping()
        {
            var dtoToFieldMetadataMapping = CreateMap<VersionedComponentDto, FieldMetadata>();
            dtoToFieldMetadataMapping.IncludeBase<VersionedComponentDto, MetadataBase>();
            dtoToFieldMetadataMapping.ForMember(d => d.MemberOf,
                opts => opts.MapFrom(d => new ClassMetadata() {Id = d.MemberOf.Value}));
            dtoToFieldMetadataMapping.ForMember(d => d.IsStatic,
                opts => opts.MapFrom(d => d.IsStatic));
        }

        private void SetupDtoToParameterMetadataMapping()
        {
            var dtoToParameterMetadataMapping = CreateMap<VersionedComponentDto, ParameterMetadata>();
            dtoToParameterMetadataMapping.IncludeBase<VersionedComponentDto, MetadataBase>();
            dtoToParameterMetadataMapping.ForMember(d => d.ParameterOf,
                opts => opts.MapFrom(d => new MethodMetadata() {Id = d.MemberOf.Value}));
            dtoToParameterMetadataMapping.ForMember(d => d.Index,
                opts => opts.MapFrom(d => d.Index));
        }

        private void SetupGameVersionToDtoMapping()
        {
            var gameVersionToDtoMapping = CreateMap<GameVersion, GameVersionDto>();
            gameVersionToDtoMapping.ForAllMembers(d => d.Ignore());
            gameVersionToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            gameVersionToDtoMapping.ForMember(d => d.Name,
                opts => opts.MapFrom(d => d.Name));
            gameVersionToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            gameVersionToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            gameVersionToDtoMapping.ForMember(d => d.IsPreRelease,
                opts => opts.MapFrom(d => d.IsPreRelease));
            gameVersionToDtoMapping.ForMember(d => d.IsSnapshot,
                opts => opts.MapFrom(d => d.IsSnapshot));
        }

        private void SetupDtoToGameVersionMapping()
        {
            var dtoToGameVersionMapping = CreateMap<GameVersionDto, GameVersion>();
            dtoToGameVersionMapping.ForAllMembers(d => d.Ignore());
            dtoToGameVersionMapping.ForMember(d => d.Name,
                opts => opts.MapFrom(d => d.Name));
            dtoToGameVersionMapping.ForMember(d => d.IsPreRelease,
                opts => opts.MapFrom(d => d.IsPreRelease));
            dtoToGameVersionMapping.ForMember(d => d.IsSnapshot,
                opts => opts.MapFrom(d => d.IsSnapshot));
        }

        private void SetupMappingTypeToDtoMapping()
        {
            var mappingTypeToDtoMapping = CreateMap<MappingType, MappingTypeDto>();
            mappingTypeToDtoMapping.ForAllMembers(d => d.Ignore());
            mappingTypeToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            mappingTypeToDtoMapping.ForMember(d => d.Name,
                opts => opts.MapFrom(d => d.Name));
            mappingTypeToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            mappingTypeToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            mappingTypeToDtoMapping.ForMember(d => d.Releases,
                opts => opts.MapFrom(d => d.Releases.Select(r => r.Id).ToHashSet()));
        }

        private void SetupDtoToMappingTypeMapping()
        {
            var dtoToMappingTypeMapping = CreateMap<MappingTypeDto, MappingType>();
            dtoToMappingTypeMapping.ForAllOtherMembers(d => d.Ignore());
            dtoToMappingTypeMapping.ForMember(d => d.Name,
                opts => opts.MapFrom(d => d.Name));
            dtoToMappingTypeMapping.ForMember(d => d.Releases,
                opts => opts.MapFrom(d => d.Releases.Select(id => new Release() {Id = id}).ToList()));
        }
    }
}
