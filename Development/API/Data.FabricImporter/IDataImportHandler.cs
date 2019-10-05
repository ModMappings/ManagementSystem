using System.Threading.Tasks;
using Data.EFCore.Context;
using Microsoft.Extensions.Configuration;

namespace Data.FabricImporter
{
    public interface IDataImportHandler
    {
        Task Import(MCMSContext context, IConfiguration configuration);
    }
}
