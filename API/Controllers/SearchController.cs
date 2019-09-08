using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Model;
using Data.EFCore.Context;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/search")]
    public class SearchController : Controller
    {

        private readonly MCPContext _context;

        public SearchController(MCPContext context)
        {
            _context = context;
        }

        [HttpGet("{query}")]
        public async Task<ActionResult<IEnumerable<SearchResult>>> Search(string query)
        {
            return null;
        }
    }
}
