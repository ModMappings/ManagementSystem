using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.EFCore.Context
{
    public class MCPContextDesignTimeFactory
        : IDesignTimeDbContextFactory<MCPContext>
    {
        public MCPContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MCPContext>();
            optionsBuilder.UseNpgsql(
                "host=localhost;port=5432;database=mcp;username=mcp-migrations;password=mcp-migrations");

            return new MCPContext(optionsBuilder.Options);
        }
    }
}
