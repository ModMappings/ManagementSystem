using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Context
{
    public class MCPContext
        : DbContext
    {
        public MCPContext(DbContextOptions<MCPContext> options) : base(options)
        {
        }

        public DbSet<GameVersion> GameVersions { get; set; }

        public DbSet<Release> Releases { get; set; }

        public DbSet<MappingType> MappingTypes { get; set; }

        public DbSet<Component> Components { get; set; }

        public DbSet<VersionedComponent> VersionedComponents { get; set; }

        public DbSet<LiveMappingEntry> LiveMappingEntries { get; set; }

        public DbSet<ProposalMappingEntry> ProposalMappingEntries { get; set; }

        public DbSet<ReleaseComponent> ReleaseComponents { get; set; }

        public DbSet<VersionedComponentMetadataBase> VersionedComponentMetadata { get; set; }

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
