using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Readers.Class;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace API.Controllers
{
    [ApiController]
    [Route("/packages")]
    public class PackagesController : Controller
    {
        private readonly IClassMappingReader _classMappingReader;

        public PackagesController(IClassMappingReader classMappingReader)
        {
            _classMappingReader = classMappingReader;
        }

        /// <summary>
        /// Lists all packages in the latest version.
        /// </summary>
        /// <returns>All packages in the latest version.</returns>
        [HttpGet("version/latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<string>>> GetByLatestVersion()
        {
            return Json((await _classMappingReader.GetByLatestRelease()).Select(mapping =>
                mapping.VersionedMappings.OrderByDescending(versionedMapping => versionedMapping.CreatedOn)
                    .FirstOrDefault().Package).Distinct().OrderBy(s => s));
        }

        /// <summary>
        /// Lists all packages in the latest version.
        /// </summary>
        /// <param name="versionId">The id of the version to collect the packages from.</param>
        /// <returns>All packages in the given version.</returns>
        [HttpGet("version/{versionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<string>>> GetByVersion(Guid versionId)
        {
            return Json((await _classMappingReader.GetByVersion(versionId)).Select(mapping =>
                mapping.VersionedMappings
                    .FirstOrDefault(versionedMapping => versionedMapping.GameVersion.Id == versionId).Package).Distinct().OrderBy(s => s));
        }

        /// <summary>
        /// Lists all packages in the latest release.
        /// </summary>
        /// <param name="releaseId">The id of the release to collect the packages from.</param>
        /// <returns>All packages in the given release.</returns>
        [HttpGet("release/{releaseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<string>>> GetByRelease(Guid releaseId)
        {
            return Json((await _classMappingReader.GetByRelease(releaseId))
                .SelectMany(mapping => mapping.VersionedMappings)
                .Where(versionedMapping => versionedMapping.CommittedMappings.Any(committedMapping =>
                    committedMapping.Releases.Any(release => release.Id == releaseId)))
                .Select(versionedMapping => versionedMapping.Package).Distinct().OrderBy(s => s));
        }
    }
}
