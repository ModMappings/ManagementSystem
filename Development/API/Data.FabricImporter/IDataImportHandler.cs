using System.Threading.Tasks;
using Mcms.Api.Data.EfCore.Context;
using Microsoft.Extensions.Configuration;

namespace Data.FabricImporter
{
    public interface IDataImportHandler
    {
        Task Import(MCMSContext context, IConfiguration configuration);
    }
}
