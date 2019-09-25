using System;
using System.Threading.Tasks;
using Data.EFCore.Context;

namespace Data.MCPImport
{
    public interface IDataImportHandler
    {
        Task Import(MCMSContext context);
    }
}
