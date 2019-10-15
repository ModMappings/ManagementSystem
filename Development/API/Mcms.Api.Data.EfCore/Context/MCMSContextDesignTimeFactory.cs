using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.EFCore.Context
{
    public class MCMSContextDesignTimeFactory
        : IDesignTimeDbContextFactory<MCMSContext>
    {
        public MCMSContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MCMSContext>();
            optionsBuilder.UseNpgsql(
                "host=localhost;port=5432;database=mcp;username=mcp-migrations;password=mcp-migrations");

            return new MCMSContext(optionsBuilder.Options);
        }
    }
}
