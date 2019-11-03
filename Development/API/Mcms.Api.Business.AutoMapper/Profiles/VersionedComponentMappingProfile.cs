using System.Linq;
using AutoMapper;
using Mcms.Api.Business.Poco.Api.REST.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Core;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Mapping.Metadata;

namespace Mcms.Api.Business.AutoMapper.Profiles
{
    internal class VersionedComponentMappingProfile
        : Profile
    {
        public VersionedComponentMappingProfile()
        {
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
                vc.Type == ComponentTypeDto.PACKAGE
                    ? (MetadataBase) new PackageMetadata()
                    : vc.Type == ComponentTypeDto.CLASS
                        ? (MetadataBase) new ClassMetadata()
                        : vc.Type == ComponentTypeDto.METHOD
                            ? (MetadataBase) new MethodMetadata()
                            : vc.Type == ComponentTypeDto.FIELD
                                ? (MetadataBase) new FieldMetadata()
                                : vc.Type == ComponentTypeDto.PARAMETER
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
    }
}
