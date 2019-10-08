using Castle.Core.Internal;
using Mcms.Api.Data.Poco.Models.Core;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Mcms.Api.Data.Poco.Models.Mapping;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Mapping.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Context
{
    public class MCMSContext
        : DbContext
    {
        public MCMSContext(DbContextOptions<MCMSContext> options) : base(options)
        {
        }

        public DbSet<GameVersion> GameVersions { get; set; }

        public DbSet<Release> Releases { get; set; }

        public DbSet<MappingType> MappingTypes { get; set; }

        public DbSet<Component> Components { get; set; }

        public DbSet<VersionedComponent> VersionedComponents { get; set; }

        public DbSet<CommittedMapping> LiveMappingEntries { get; set; }

        public DbSet<ProposedMapping> ProposalMappingEntries { get; set; }

        public DbSet<ReleaseComponent> ReleaseComponents { get; set; }

        public DbSet<LockingEntry> LockingEntries { get; set; }

        public DbSet<MetadataBase> VersionedComponentMetadata { get; set; }

        public DbSet<ClassMetadata> ClassMetadata { get; set; }

        public DbSet<MethodMetadata> MethodMetadata { get; set; }

        public DbSet<FieldMetadata> FieldMetadata { get; set; }

        public DbSet<ParameterMetadata> ParameterMetadata { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GameVersion>()
                .HasIndex(version => version.Name)
                .IsUnique();

            modelBuilder.Entity<Release>()
                .HasIndex(release => release.Name)
                .IsUnique();
        }
    }
}
