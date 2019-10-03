using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Dtos.Log;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Services.Interfaces;
using Auth.Admin.Configuration.Constants;

namespace Auth.Admin.Controllers
{
    [Authorize(Policy = AuthorizationConsts.AdministrationPolicy)]
    public class LogController : BaseController
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService,
            ILogger<ConfigurationController> logger) : base(logger)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> ErrorsLog(int? page, string search)
        {
            ViewBag.Search = search;
            var logs = await _logService.GetLogsAsync(search, page ?? 1);

            return View(logs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLogs(LogsDto logsDto)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(ErrorsLog), logsDto);
            }

            await _logService.DeleteLogsOlderThanAsync(logsDto.DeleteOlderThan.Value);

            return RedirectToAction(nameof(ErrorsLog));
        }
    }
}
