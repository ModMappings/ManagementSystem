using System;
using System.Linq;
using AutoMapper;
using Castle.Core.Internal;
using Data.WebApi.Model.Api.Core;
using Data.WebApi.Model.Api.Core.Releases;
using Data.WebApi.Model.Api.Mapping.Comment;
using Data.WebApi.Model.Api.Mapping.Component;
using Data.WebApi.Model.Api.Mapping.Mappings;
using Data.WebApi.Model.Api.Mapping.Mappings.Voting;
using Mcms.Api.Data.Poco.Models.Comments;
using Mcms.Api.Data.Poco.Models.Core;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings.Voting;
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

            SetupReleaseToDtoMapping();
            SetupDtoToReleaseMapping();

            SetupCommittedMappingToDtoMapping();
            SetupDtoToCommittedMappingMapping();

            SetupProposedMappingToDtoMapping();
            SetupDtoToProposedMappingMapping();

            SetupVotingRecordToDtoMapping();
            SetupDtoToVotingRecordMapping();

            SetupCommentToDtoMapping();
            SetupDtoToCommentMapping();

            SetupCommentReactionToDtoMapping();
            SetupDtoToCommentReactionMapping();
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

        private void SetupReleaseToDtoMapping()
        {
            var releaseToDtoMapping = CreateMap<Release, ReleaseDto>();
            releaseToDtoMapping.ForAllMembers(d => d.Ignore());
            releaseToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            releaseToDtoMapping.ForMember(d => d.Name,
                opts => opts.MapFrom(d => d.Name));
            releaseToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            releaseToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            releaseToDtoMapping.ForMember(d => d.GameVersion,
                opts => opts.MapFrom(d => d.GameVersion.Id));
            releaseToDtoMapping.ForMember(d => d.MappingType,
                opts => opts.MapFrom(d => d.MappingType.Id));
            releaseToDtoMapping.ForMember(d => d.IsSnapshot,
                opts => opts.MapFrom(d => d.IsSnapshot));
            releaseToDtoMapping.ForMember(d => d.PackageMappings,
                opts => opts.MapFrom(d =>
                    d.Components.Where(c => c.Mapping.VersionedComponent.Component.Type == ComponentType.PACKAGE)
                        .Select(c => c.Id).ToHashSet()));
            releaseToDtoMapping.ForMember(d => d.ClassMappings,
                opts => opts.MapFrom(d =>
                    d.Components.Where(c => c.Mapping.VersionedComponent.Component.Type == ComponentType.CLASS)
                        .Select(c => c.Id).ToHashSet()));
            releaseToDtoMapping.ForMember(d => d.MethodMappings,
                opts => opts.MapFrom(d =>
                    d.Components.Where(c => c.Mapping.VersionedComponent.Component.Type == ComponentType.METHOD)
                        .Select(c => c.Id).ToHashSet()));
            releaseToDtoMapping.ForMember(d => d.FieldMappings,
                opts => opts.MapFrom(d =>
                    d.Components.Where(c => c.Mapping.VersionedComponent.Component.Type == ComponentType.FIELD)
                        .Select(c => c.Id).ToHashSet()));
            releaseToDtoMapping.ForMember(d => d.ParameterMappings,
                opts => opts.MapFrom(d =>
                    d.Components.Where(c => c.Mapping.VersionedComponent.Component.Type == ComponentType.PARAMETER)
                        .Select(c => c.Id).ToHashSet()));
            releaseToDtoMapping.ForMember(d => d.Comments,
                opts => opts.MapFrom(d =>
                    d.Comments.Select(c => c.Id).ToHashSet()));
        }

        private void SetupDtoToReleaseMapping()
        {
            var dtoToReleaseMapping = CreateMap<ReleaseDto, Release>();
            dtoToReleaseMapping.ForAllMembers(d => d.Ignore());
            dtoToReleaseMapping.ForMember(d => d.Name,
                opts => opts.MapFrom(d => d.Name));
            dtoToReleaseMapping.ForMember(d => d.Comments,
                opts => opts.MapFrom(d => d.Comments.Select(id => new Comment {Id = id})));
        }

        private void SetupCommittedMappingToDtoMapping()
        {
            var committedMappingToDtoMapping = CreateMap<CommittedMapping, CommittedMappingDto>();
            committedMappingToDtoMapping.ForAllMembers(d => d.Ignore());
            committedMappingToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            committedMappingToDtoMapping.ForMember(d => d.VersionedComponent,
                opts => opts.MapFrom(d => d.VersionedComponent.Id));
            committedMappingToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            committedMappingToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            committedMappingToDtoMapping.ForMember(d => d.InputMapping,
                opts => opts.MapFrom(d => d.InputMapping));
            committedMappingToDtoMapping.ForMember(d => d.OutputMapping,
                opts => opts.MapFrom(d => d.OutputMapping));
            committedMappingToDtoMapping.ForMember(d => d.Documentation,
                opts => opts.MapFrom(d => d.Documentation));
            committedMappingToDtoMapping.ForMember(d => d.Distribution,
                opts => opts.MapFrom(d => d.Distribution));
            committedMappingToDtoMapping.ForMember(d => d.MappingType,
                opts => opts.MapFrom(d => d.MappingType.Id));
            committedMappingToDtoMapping.ForMember(d => d.Proposal,
                opts => opts.MapFrom(d => d.ProposedMapping != null ? (Guid?) d.ProposedMapping.Id : (Guid?) null));
            committedMappingToDtoMapping.ForMember(d => d.Releases,
                opts => opts.MapFrom(d => d.Releases.Select(r => r.Release.Id).ToHashSet()));
        }

        private void SetupDtoToCommittedMappingMapping()
        {
            var dtoToCommittedMappingMapping = CreateMap<CommittedMappingDto, CommittedMapping>();
            dtoToCommittedMappingMapping.ForAllMembers(d => d.Ignore());
            dtoToCommittedMappingMapping.ForMember(d => d.InputMapping,
                opts => opts.MapFrom(d => d.InputMapping));
            dtoToCommittedMappingMapping.ForMember(d => d.OutputMapping,
                opts => opts.MapFrom(d => d.OutputMapping));
            dtoToCommittedMappingMapping.ForMember(d => d.Documentation,
                opts => opts.MapFrom(d => d.Documentation));
            dtoToCommittedMappingMapping.ForMember(d => d.Distribution,
                opts => opts.MapFrom(d => d.Distribution));
            dtoToCommittedMappingMapping.ForMember(d => d.ProposedMapping,
                opts => opts.MapFrom(d => d.Proposal.HasValue ? new ProposedMapping {Id = d.Proposal.Value} : null));
        }

        private void SetupProposedMappingToDtoMapping()
        {
            var proposedMappingToDtoMapping = CreateMap<ProposedMapping, ProposedMappingDto>();
            proposedMappingToDtoMapping.ForAllMembers(d => d.Ignore());
            proposedMappingToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            proposedMappingToDtoMapping.ForMember(d => d.VersionedComponent,
                opts => opts.MapFrom(d => d.VersionedComponent.Id));
            proposedMappingToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            proposedMappingToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            proposedMappingToDtoMapping.ForMember(d => d.InputMapping,
                opts => opts.MapFrom(d => d.InputMapping));
            proposedMappingToDtoMapping.ForMember(d => d.OutputMapping,
                opts => opts.MapFrom(d => d.OutputMapping));
            proposedMappingToDtoMapping.ForMember(d => d.Documentation,
                opts => opts.MapFrom(d => d.Documentation));
            proposedMappingToDtoMapping.ForMember(d => d.Distribution,
                opts => opts.MapFrom(d => d.Distribution));
            proposedMappingToDtoMapping.ForMember(d => d.MappingType,
                opts => opts.MapFrom(d => d.MappingType.Id));
            proposedMappingToDtoMapping.ForMember(d => d.IsOpen,
                opts => opts.MapFrom(d => d.IsOpen));
            proposedMappingToDtoMapping.ForMember(d => d.IsPublicVote,
                opts => opts.MapFrom(d => d.IsPublicVote));
            proposedMappingToDtoMapping.ForMember(d => d.Votes,
                opts => opts.MapFrom(d => d.Votes.Select(v => v.Id).ToHashSet()));
            proposedMappingToDtoMapping.ForMember(d => d.Comments,
                opts => opts.MapFrom(d => d.Comments.Select(c => c.Id).ToHashSet()));
            proposedMappingToDtoMapping.ForMember(d => d.ClosedBy,
                opts => opts.MapFrom(d => d.ClosedBy));
            proposedMappingToDtoMapping.ForMember(d => d.ClosedOn,
                opts => opts.MapFrom(d => d.ClosedOn));
            proposedMappingToDtoMapping.ForMember(d => d.Merged,
                opts => opts.MapFrom(d => d.Merged));
            proposedMappingToDtoMapping.ForMember(d => d.CommittedWith,
                opts => opts.MapFrom(d => d.CommittedWithId));

        }

        private void SetupDtoToProposedMappingMapping()
        {
            var dtoToProposedMappingMapping = CreateMap<ProposedMappingDto, ProposedMapping>();
            dtoToProposedMappingMapping.ForAllMembers(d => d.Ignore());
            dtoToProposedMappingMapping.ForMember(d => d.InputMapping,
                opts => opts.MapFrom(d => d.InputMapping));
            dtoToProposedMappingMapping.ForMember(d => d.OutputMapping,
                opts => opts.MapFrom(d => d.OutputMapping));
            dtoToProposedMappingMapping.ForMember(d => d.Documentation,
                opts => opts.MapFrom(d => d.Documentation));
            dtoToProposedMappingMapping.ForMember(d => d.Distribution,
                opts => opts.MapFrom(d => d.Distribution));
            dtoToProposedMappingMapping.ForMember(d => d.IsOpen,
                opts => opts.MapFrom(d => d.IsOpen));
            dtoToProposedMappingMapping.ForMember(d => d.IsPublicVote,
                opts => opts.MapFrom(d => d.IsPublicVote));
            dtoToProposedMappingMapping.ForMember(d => d.Votes,
                opts => opts.MapFrom(d => d.Votes.Select(v => new VotingRecord {Id = v}).ToList()));
            dtoToProposedMappingMapping.ForMember(d => d.Comments,
                opts => opts.MapFrom(d => d.Comments.Select(c => new Comment {Id = c}).ToList()));
            dtoToProposedMappingMapping.ForMember(d => d.ClosedBy,
                opts => opts.MapFrom(d => d.ClosedBy));
            dtoToProposedMappingMapping.ForMember(d => d.ClosedOn,
                opts => opts.MapFrom(d => d.ClosedOn));
            dtoToProposedMappingMapping.ForMember(d => d.Merged,
                opts => opts.MapFrom(d => d.Merged));
            dtoToProposedMappingMapping.ForMember(d => d.CommittedWithId,
                opts => opts.MapFrom(d => d.CommittedWith));
            dtoToProposedMappingMapping.ForMember(d => d.CommittedWith,
                opts => opts.MapFrom(d =>
                    d.CommittedWith.HasValue ? new CommittedMapping {Id = d.CommittedWith.Value} : null));
        }

        private void SetupVotingRecordToDtoMapping()
        {
            var votingRecordToDtoMapping = CreateMap<VotingRecord, VotingRecordDto>();
            votingRecordToDtoMapping.ForAllMembers(d => d.Ignore());
            votingRecordToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            votingRecordToDtoMapping.ForMember(d => d.Proposal,
                opts => opts.MapFrom(d => d.Proposal.Id));
            votingRecordToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            votingRecordToDtoMapping.ForMember(d => d.VotedBy,
                opts => opts.MapFrom(d => d.VotedBy));
            votingRecordToDtoMapping.ForMember(d => d.IsForVote,
                opts => opts.MapFrom(d => d.IsForVote));
            votingRecordToDtoMapping.ForMember(d => d.HasBeenRescinded,
                opts => opts.MapFrom(d => d.HasBeenRescinded));
        }

        private void SetupDtoToVotingRecordMapping()
        {
            var dtoToVotingRecordMapping = CreateMap<VotingRecordDto, VotingRecord>();
            dtoToVotingRecordMapping.ForAllMembers(d => d.Ignore());
            dtoToVotingRecordMapping.ForMember(d => d.Proposal,
                opts => opts.MapFrom(d => new ProposedMapping {Id = d.Proposal}));
            dtoToVotingRecordMapping.ForMember(d => d.IsForVote,
                opts => opts.MapFrom(d => d.IsForVote));
            dtoToVotingRecordMapping.ForMember(d => d.HasBeenRescinded,
                opts => opts.MapFrom(d => d.HasBeenRescinded));
        }

        private void SetupCommentToDtoMapping()
        {
            var commentToDtoMapping = CreateMap<Comment, CommentDto>();
            commentToDtoMapping.ForAllMembers(d => d.Ignore());
            commentToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            commentToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            commentToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            commentToDtoMapping.ForMember(d => d.Content,
                opts => opts.MapFrom(d => d.Content));
            commentToDtoMapping.ForMember(d => d.Reactions,
                opts => opts.MapFrom(d => d.Reactions.Select(r => r.Id).ToHashSet()));
            commentToDtoMapping.ForMember(d => d.HasBeenEdited,
                opts => opts.MapFrom(d => d.HasBeenEdited));
            commentToDtoMapping.ForMember(d => d.IsDeleted,
                opts => opts.MapFrom(d => d.IsDeleted));
            commentToDtoMapping.ForMember(d => d.DeletedBy,
                opts => opts.MapFrom(d => d.DeletedBy));
            commentToDtoMapping.ForMember(d => d.DeletedOn,
                opts => opts.MapFrom(d => d.DeletedOn));
            commentToDtoMapping.ForMember(d => d.ProposedMapping,
                opts => opts.MapFrom(d => d.ProposedMapping != null ? (Guid?) d.ProposedMapping.Id : (Guid?) null));
            commentToDtoMapping.ForMember(d => d.Release,
                opts => opts.MapFrom(d => d.Release != null ? (Guid?) d.Release.Id : (Guid?) null));
            commentToDtoMapping.ForMember(d => d.Parent,
                opts => opts.MapFrom(d => d.Parent != null ? (Guid?) d.Parent.Id : (Guid?) null));
            commentToDtoMapping.ForMember(d => d.Children,
                opts => opts.MapFrom(d => d.Children.Select(c => c.Id).ToHashSet()));
        }

        private void SetupDtoToCommentMapping()
        {
            var dtoToCommentMapping = CreateMap<CommentDto, Comment>();
            dtoToCommentMapping.ForAllMembers(d => d.Ignore());
            dtoToCommentMapping.ForMember(d => d.Content,
                opts => opts.MapFrom(d => d.Content));
            dtoToCommentMapping.ForMember(d => d.Reactions,
                opts => opts.MapFrom(d => d.Reactions.Select(id => new CommentReaction {Id = id}).ToList()));
            dtoToCommentMapping.ForMember(d => d.IsDeleted,
                opts => opts.MapFrom(d => d.IsDeleted));
            dtoToCommentMapping.ForMember(d => d.ProposedMapping,
                opts => opts.MapFrom(d => d.ProposedMapping.HasValue ? new ProposedMapping {Id = d.ProposedMapping.Value} : null));
            dtoToCommentMapping.ForMember(d => d.Release,
                opts => opts.MapFrom(d => d.Release.HasValue ? new Release {Id = d.Release.Value} : null));
            dtoToCommentMapping.ForMember(d => d.Parent,
                opts => opts.MapFrom(d => d.Parent.HasValue ? new Comment {Id = d.Parent.Value} : null));
            dtoToCommentMapping.ForMember(d => d.Children,
                opts => opts.MapFrom(d => d.Children.Select(c => new Comment {Id = c}).ToList()));
            dtoToCommentMapping.BeforeMap((dto, comment) =>
            {
                if (!comment.Content.IsNullOrEmpty())
                {
                    if (comment.Content != dto.Content)
                    {
                        comment.HasBeenEdited = true;
                    }
                }
            });
        }

        private void SetupCommentReactionToDtoMapping()
        {
            var commentReactionToDtoMapping = CreateMap<CommentReaction, CommentReactionDto>();
            commentReactionToDtoMapping.ForAllMembers(d => d.Ignore());
            commentReactionToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            commentReactionToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            commentReactionToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            commentReactionToDtoMapping.ForMember(d => d.Type,
                opts => opts.MapFrom(d => d.Type));
            commentReactionToDtoMapping.ForMember(d => d.Comment,
                opts => opts.MapFrom(d => d.Comment));
        }

        private void SetupDtoToCommentReactionMapping()
        {
            var dtoToCommentReactionMapping = CreateMap<CommentReactionDto, CommentReaction>();
            dtoToCommentReactionMapping.ForAllMembers(d => d.Ignore());
            dtoToCommentReactionMapping.ForMember(d => d.Type,
                opts => opts.MapFrom(d => d.Type));
            dtoToCommentReactionMapping.ForMember(d => d.Comment,
                opts => opts.MapFrom(d => d.Comment));
        }
    }
}
