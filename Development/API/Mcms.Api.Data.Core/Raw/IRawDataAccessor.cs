using System.ComponentModel.DataAnnotations;
using System.Linq;
using Mcms.Api.Data.Poco.Models.Core;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Mapping.Metadata;

namespace Mcms.Api.Data.Core.Raw
{
    public interface IRawDataAccessor
    {
        IQueryable<GameVersion> GameVersions { get; }
        IQueryable<Release> Releases { get; }
        IQueryable<MappingType> MappingTypes { get; }
        IQueryable<Component> Components { get; }
        IQueryable<VersionedComponent> VersionedComponents { get; }
        IQueryable<CommittedMapping> LiveMappingEntries { get; }
        IQueryable<ProposedMapping> ProposalMappingEntries { get; }
        IQueryable<ReleaseComponent> ReleaseComponents { get; }
        IQueryable<LockingEntry> LockingEntries { get; }
        IQueryable<MetadataBase> VersionedComponentMetadata { get; }
        IQueryable<ClassMetadata> ClassMetadata { get; }
        IQueryable<MethodMetadata> MethodMetadata { get; }
        IQueryable<FieldMetadata> FieldMetadata { get; }
        IQueryable<ParameterMetadata> ParameterMetadata { get; }
        IQueryable<PackageMetadata> PackageMetadata { get; }

        void MarkObjectChanged(object obj);

        void MarkObjectAdded(object obj);

        void MarkObjectRemoved(object obj);

        void Detach(object obj);
    }
}
