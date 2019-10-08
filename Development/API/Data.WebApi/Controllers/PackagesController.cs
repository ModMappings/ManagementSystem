using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Mapping.Metadata;
using Data.EFCore.Writer.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers
{
    [ApiController]
    [Route("/packages")]
    public class PackagesController : Controller
    {
        private readonly ClassWriter _classes;

        public PackagesController(ClassWriter classes)
        {
            _classes = classes;
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
            return Json((await _classes.GetByLatestRelease()).Select(mapping =>
                (mapping.VersionedComponents.OrderByDescending(versionedMapping => versionedMapping.CreatedOn)
                    .FirstOrDefault().Metadata as ClassMetadata).Package).Distinct().OrderBy(s => s));
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
            return Json((await _classes.GetByVersion(versionId)).Select(mapping =>
                (mapping.VersionedComponents
                    .FirstOrDefault(versionedMapping => versionedMapping.GameVersion.Id == versionId).Metadata as ClassMetadata).Package).Distinct().OrderBy(s => s));
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
            return Json((await _classes.GetByRelease(releaseId))
                .SelectMany(mapping => mapping.VersionedComponents)
                .Where(versionedMapping => versionedMapping.Mappings.Any(committedMapping =>
                    committedMapping.Releases.Any(release => release.Id == releaseId)))
                .Select(versionedMapping => (versionedMapping.Metadata as ClassMetadata).Package).Distinct().OrderBy(s => s));
        }
    }
}
