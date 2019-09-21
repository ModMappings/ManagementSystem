using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Writers.Core;
using Data.WebApi.Model.Creation.Core;
using Data.WebApi.Model.Read.Core;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers
{
    [ApiController]
    [Route("/mappingtypes")]
    public class MappingTypeController : Controller
    {

        private readonly IUserResolvingService _userResolvingService;

        private readonly IMappingTypeWriter _mappingTypeWriter;

        public MappingTypeController(IMappingTypeWriter mappingTypeWriter, IUserResolvingService userResolvingService)
        {
            _mappingTypeWriter = mappingTypeWriter;
            _userResolvingService = userResolvingService;
        }

        /// <summary>
        /// Allows for the lookup of a single mapping type.
        /// </summary>
        /// <param name="id">The id of the mapping type to lookup.</param>
        /// <returns>The mapping type with the given id. Or 404.</returns>
        [HttpGet("id/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<MappingTypeReadModel>> GetById(Guid id)
        {
            var mappingType = await _mappingTypeWriter.GetById(id);

            if (mappingType == null)
                return NotFound();

            return Json(ConvertDbModelToReadModel(mappingType));
        }

        /// <summary>
        /// Allows for the lookup of a single mapping type.
        /// </summary>
        /// <param name="name">The name of the mapping type to lookup.</param>
        /// <returns>The mapping type with the given name. Or 404.</returns>
        [HttpGet("name/{name}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<MappingTypeReadModel>> GetByName(string name)
        {
            var mappingType = await _mappingTypeWriter.GetByName(name);

            if (mappingType == null)
                return NotFound();

            return Json(ConvertDbModelToReadModel(mappingType));
        }

        /// <summary>
        /// Allows for the lookup of all mapping types in the database.
        /// </summary>
        /// <param name="pageSize">The size of the page to collect.</param>
        /// <param name="pageIndex">The index of the page to collect.</param>
        /// <returns>A paginated result of all stored mapping types.</returns>
        [HttpGet("all/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<MappingTypeReadModel>>> AsQueryable(int pageSize, int pageIndex)
        {
            var dbModels = await _mappingTypeWriter.AsQueryable();

            var resultingModels = dbModels.Skip(pageIndex * pageSize).Take(pageSize).Select(ConvertDbModelToReadModel);

            return Json(resultingModels);
        }

        /// <summary>
        /// Counts all mapping types in the database.
        /// </summary>
        /// <returns>A count of all mapping types in the database.</returns>
        [HttpGet("all/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountAsQueryable()
        {
            return Content((await _mappingTypeWriter.AsQueryable()).Count().ToString());
        }

        /// <summary>
        /// Allows for the lookup of all mapping types, created by a given user, in the database.
        /// </summary>
        /// <param name="userId">The id of the user who created the mapping types.</param>
        /// <param name="pageSize">The size of the page to collect.</param>
        /// <param name="pageIndex">The index of the page to collect.</param>
        /// <returns>A paginated result of all stored mapping types, made by a given user.</returns>
        [HttpGet("user/{userId}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<MappingTypeReadModel>>> GetMadeBy(Guid userId, int pageSize, int pageIndex)
        {
            var dbModels = await _mappingTypeWriter.GetMadeBy(userId);

            var resultingModels = dbModels.Skip(pageIndex * pageSize).Take(pageSize).Select(ConvertDbModelToReadModel);

            return Json(resultingModels);
        }

        /// <summary>
        /// Counts all mapping types in the database, made by a given user.
        /// </summary>
        /// <param name="userId">The id of the user who created the mapping types.</param>
        /// <returns>A count of all mapping types in the database, created by the given user.</returns>
        [HttpGet("user/{userId}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> CountMadeBy(Guid userId)
        {
            var dbModels = await _mappingTypeWriter.GetMadeBy(userId);

            return Content(dbModels.Count().ToString());
        }

        /// <summary>
        /// Allows for the lookup of all mapping types, created on a given date, in the database.
        /// </summary>
        /// <param name="date">The date the mapping types where created on.</param>
        /// <param name="pageSize">The size of the page to collect.</param>
        /// <param name="pageIndex">The index of the page to collect.</param>
        /// <returns>A paginated result of all stored mapping types, made on a given date.</returns>
        [HttpGet("date/{date}/{pageSize}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<IQueryable<MappingTypeReadModel>>> GetMadeOn(DateTime date, int pageSize, int pageIndex)
        {
            var dbModels = await _mappingTypeWriter.GetMadeOn(date);

            var resultingModels = dbModels.Skip(pageIndex * pageSize).Take(pageSize).Select(ConvertDbModelToReadModel);

            return Json(resultingModels);
        }

        /// <summary>
        /// Counts all mapping types in the database, made on a given date.
        /// </summary>
        /// <param name="date">The date the mapping types where created on.</param>
        /// <returns>A count of all mapping types in the database, created on a given date.</returns>
        [HttpGet("date/{date}/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task<ActionResult<int>> GetMadeOn(DateTime date)
        {
            var dbModels = await _mappingTypeWriter.GetMadeOn(date);

            return Content(dbModels.Count().ToString());
        }

        /// <summary>
        /// Allows for the creation of a mapping type.
        /// </summary>
        /// <param name="createMappingModel">The model of the data used to created a new mapping.</param>
        /// <returns>A 201 status code.</returns>
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize()]
        public async Task<ActionResult> Add([FromBody] CreateMappingTypeModel createMappingModel)
        {
            var createdBy = await _userResolvingService.Get();

            var mapping = new MappingType
            {
                Id = Guid.NewGuid(),
                Name = createMappingModel.Name,
                CreatedBy = createdBy.Id,
                CreatedOn = DateTime.Now,
                Releases = new List<Release>()
            };

            await _mappingTypeWriter.Add(mapping);
            await _mappingTypeWriter.SaveChanges();
            return CreatedAtAction("GetById", mapping.Id, ConvertDbModelToReadModel(mapping));
        }

        private MappingTypeReadModel ConvertDbModelToReadModel(MappingType mappingType)
        {
            return new MappingTypeReadModel
            {
                Id = mappingType.Id,
                Name = mappingType.Name,
                CreatedBy = mappingType.CreatedBy,
                CreatedOn = mappingType.CreatedOn,
                Releases = mappingType.Releases.Select(m => m.Id).ToList()
            };
        }
    }
}
