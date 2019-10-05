using System;
using System.Threading.Tasks;
using Data.EFCore.Context;
using Microsoft.Extensions.Configuration;

namespace Data.MCP.TSRG.Importer
{
    public interface IDataImportHandler
    {
        Task Import(MCMSContext context, IConfiguration configuration);
    }
}
