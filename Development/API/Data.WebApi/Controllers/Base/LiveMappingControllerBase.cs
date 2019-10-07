using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Writers.Core;
using Data.WebApi.Model.Read.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Base
{
    public abstract class LiveMappingControllerBase : Controller
    {
        private readonly IComponentWriter _componentWriter;

        protected LiveMappingControllerBase(IComponentWriter componentWriter)
        {
            _componentWriter = componentWriter;
        }

        /// <summary>
        /// Gets the live mapping with the given id.
        /// </summary>
        /// <param name="id">The id of the live mapping you are looking for.</param>
        /// <returns>200-The live mapping with the given id. 404-When no live mapping exists with the given id.</returns>
        [HttpGet("id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<DetailedMappingReadModel>> GetById(Guid id)
        {
            var liveMappingEntry = await _componentWriter.GetLiveMapping(id);

            if (liveMappingEntry == null)
                return NotFound();

            return Json(ConvertLiveDbModelToDetailedMappingReadModel(liveMappingEntry));
        }

        protected DetailedMappingReadModel ConvertLiveDbModelToDetailedMappingReadModel(LiveMappingEntry liveMappingEntry)
        {
            return new DetailedMappingReadModel()
            {
                Id = liveMappingEntry.Id,
                In = liveMappingEntry.InputMapping,
                Out = liveMappingEntry.OutputMapping,
                Proposal = liveMappingEntry.ProposedMapping.Id,
                Releases = liveMappingEntry.Releases.Select(release => release.Id),
                VersionedMapping = liveMappingEntry.VersionedComponent.Id,
                Documentation = liveMappingEntry.Documentation,
                MappingName = liveMappingEntry.MappingType.Name,
                Distribution = liveMappingEntry.Distribution
            };
        }
    }
}
