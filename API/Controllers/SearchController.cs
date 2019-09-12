using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Model;
using Data.EFCore.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    public class SearchController : Controller
    {
        private readonly MCPContext _context;

        public SearchController(MCPContext context)
        {
            _context = context;
        }

        [HttpGet("/search/{pattern}/{pageSize}/{pageIndex}")]
        public async Task<ActionResult<IEnumerable<SearchResult>>> Search(string pattern, int pageSize, int pageIndex, [FromQuery] Guid? releaseId,
            [FromQuery] Guid? gameVersionId, [FromQuery] bool searchInputs)
        {
            return Json(await _context.LiveMappingEntries
                .Where(entry =>
                    (searchInputs && Regex.IsMatch(entry.InputMapping, pattern)) ||
                    Regex.IsMatch(entry.OutputMapping, pattern))
                .Where(entry => releaseId == null || entry.Releases.Any(rc => rc.Release.Id == releaseId))
                .Where(entry => gameVersionId == null || entry.Mapping.GameVersion.Id == gameVersionId)
                .Select(entry => new SearchResult()
                {
                    Input = entry.InputMapping,
                    IsProposal = false,
                    MappingId = entry.Id,
                    MappingTypeName = entry.MappingType.Name,
                    Output = entry.OutputMapping,
                    Type = entry.Mapping.Component.Type
                })
                .Union(
                    _context.ProposalMappingEntries
                        .Where(entry =>
                            (searchInputs && Regex.IsMatch(entry.InputMapping, pattern)) ||
                            Regex.IsMatch(entry.OutputMapping, pattern))
                        .Where(entry => releaseId == null)
                        .Where(entry => gameVersionId == null || entry.Mapping.GameVersion.Id == gameVersionId)
                        .Select(entry => new SearchResult()
                        {
                            Input = entry.InputMapping,
                            IsProposal = false,
                            MappingId = entry.Id,
                            MappingTypeName = entry.MappingType.Name,
                            Output = entry.OutputMapping,
                            Type = entry.Mapping.Component.Type
                        }))
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync()
            );
        }
    }
}
