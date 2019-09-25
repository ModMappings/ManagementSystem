using System.Threading.Tasks;
using Data.EFCore.Context;
using Data.MCPImport;
using Microsoft.Extensions.Logging;
using ILogger = Castle.Core.Logging.ILogger;

namespace Data.FabricImporter.Intermediary
{
    public class IntermediaryImportHandler
        : IDataImportHandler
    {
        private readonly ILogger<IntermediaryImportHandler> _logger;

        public IntermediaryImportHandler(ILogger<IntermediaryImportHandler> logger)
        {
            _logger = logger;
        }

        public async Task Import(MCMSContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
