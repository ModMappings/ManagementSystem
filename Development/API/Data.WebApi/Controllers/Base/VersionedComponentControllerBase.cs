using System;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Writers.Core;
using Data.WebApi.Model.Read.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Base
{
    public abstract class VersionedComponentControllerBase<TVersionedReadModel> : Controller
        where TVersionedReadModel : AbstractVersionedReadModel
    {

        protected readonly IComponentWriter ComponentWriter;

        protected VersionedComponentControllerBase(IComponentWriter componentWriter)
        {
            ComponentWriter = componentWriter;
        }

        /// <summary>
        /// Gets the versioned component with the given id.
        /// </summary>
        /// <param name="id">The id of the versioned component you are looking for.</param>
        /// <returns>200-The versioned component with the given id. 404-When no versioned component exists with the given id.</returns>
        [HttpGet("id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<TVersionedReadModel>> GetById(Guid id)
        {
            var versionComponent = await ComponentWriter.GetVersionedComponent(id);

            if (versionComponent == null)
                return NotFound();

            return Json(ConvertVersionedDbModelToReadModel(versionComponent));
        }

        protected abstract TVersionedReadModel
            ConvertVersionedDbModelToReadModel(VersionedComponent versionedComponent);
    }
}
