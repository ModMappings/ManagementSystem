using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Models.Field;
using Data.Core.Models.Method;
using Data.Core.Models.Parameter;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Context
{
    public class MCPContext
        : DbContext
    {
        public MCPContext(DbContextOptions<MCPContext> options) : base(options)
        {
        }

        public DbSet<ClassCommittedMappingEntry> ClassCommittedMappingEntries { get; set; }

        public DbSet<ClassMapping> ClassMappings { get; set; }

        public DbSet<ClassProposalMappingEntry> ClassProposalMappingEntries { get; set; }

        public DbSet<ClassVersionedMapping> ClassVersionedMappings { get; set; }

        public DbSet<GameVersion> GameVersions { get; set; }

        public DbSet<Release> Releases { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<FieldCommittedMappingEntry> FieldCommittedMappingEntries { get; set; }

        public DbSet<FieldMapping> FieldMappings { get; set; }

        public DbSet<FieldProposalMappingEntry> FieldProposalMappingEntries { get; set; }

        public DbSet<FieldVersionedMapping> FieldVersionedMappings { get; set; }

        public DbSet<MethodCommittedMappingEntry> MethodCommittedMappingEntries { get; set; }

        public DbSet<MethodMapping> MethodMappings { get; set; }

        public DbSet<MethodProposalMappingEntry> MethodProposalMappingEntries { get; set; }

        public DbSet<MethodVersionedMapping> MethodVersionedMappings { get; set; }

        public DbSet<ParameterCommittedMappingEntry> ParameterCommittedMappingEntries { get; set; }

        public DbSet<ParameterMapping> ParameterMappings { get; set; }

        public DbSet<ParameterProposalMappingEntry> ParameterProposalMappingEntries { get; set; }

        public DbSet<ParameterVersionedMapping> ParameterVersionedMappings { get; set; }

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
