using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
using Data.Core.Readers.Core;
using Data.Core.Writers.Core;
using Data.WebApi.Model;
using Data.WebApi.Model.Read.Core;
using Data.WebApi.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Data.WebApi.Controllers.Base
{
    public abstract class LockingControllerBase : Controller
    {
        private readonly IComponentWriter _componentWriter;

        private readonly IUserResolvingService _userResolvingService;

        private readonly IMappingTypeReader _mappingTypeReader;

        protected LockingControllerBase(IComponentWriter componentWriter, IUserResolvingService userResolvingService, IMappingTypeReader mappingTypeReader)
        {
            _componentWriter = componentWriter;
            _userResolvingService = userResolvingService;
            _mappingTypeReader = mappingTypeReader;
        }

        /// <summary>
        /// Locks the mappings of the given type in the versioned component of the component with the given game version.
        /// Closes all open proposals for the given mapping type.
        /// </summary>
        /// <param name="componentId">The id of the component to lock.</param>
        /// <param name="gameVersionId">The id of the gameversion to lock.</param>
        /// <param name="mappingTypeName">The name of the mappingtype to lock.</param>
        /// <returns>An http status code: 401-No component with the id exists, or the given component does not exist in the given gameversion, 400-If the component is already locked, 403-Forbidden the user does not have the right to lock a component, 202-If the locking was successful.</returns>
        [HttpPost("lock/{componentId}/{gameVersionId}/{mappingTypeName}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> LockByComponentAndGameVersion(Guid componentId, Guid gameVersionId, string mappingTypeName)
        {
            var currentComponent = await _componentWriter.GetById(componentId);
            if (currentComponent == null)
                return NotFound("No component with the given id exists.");

            var versionedComponent =
                currentComponent.VersionedComponents.FirstOrDefault(vc => vc.GameVersion.Id == gameVersionId);
            if (versionedComponent == null)
                return NotFound("The component does not contain a gameversion with the given id.");

            if (versionedComponent.LockedMappingTypes.Any(lmt => lmt.MappingType.Name == mappingTypeName))
                return BadRequest("The component is already locked for the given version");

            var user = await _userResolvingService.Get();
            if (user == null)
                return Unauthorized();

            if (!user.CanLock)
                return Forbid();

            var lockingMappingEntry = new LockingEntry()
            {
                Id = Guid.NewGuid(),
                MappingType = await _mappingTypeReader.GetByName(mappingTypeName),
                VersionedComponent = versionedComponent
            };

            versionedComponent.LockedMappingTypes.Add(lockingMappingEntry);

            foreach (var proposalMappingEntry in versionedComponent.Proposals.Where(p => p.MappingType.Name == mappingTypeName && p.IsOpen))
            {
                await ProcessClosing(false, proposalMappingEntry, user);
            }

            await _componentWriter.Update(versionedComponent);
            await _componentWriter.SaveChanges();

            return Accepted();
        }

        /// <summary>
        /// Locks the mappings of the given type in the versioned component.
        /// Closes all open proposals for the given mapping type.
        /// </summary>
        /// <param name="versionedComponentId">The id of the versioned component to lock.</param>
        /// <param name="mappingTypeName">The name of the mappingtype to lock.</param>
        /// <returns>An http status code: 401-No component with the id exists, or the given component does not exist in the given gameversion, 400-If the component is already locked, 403-Forbidden the user does not have the right to lock a component, 202-If the locking was successful.</returns>
        [HttpPost("lock/{versionedComponentId}/{mappingTypeName}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> LockByVersionedComponent(Guid versionedComponentId, string mappingTypeName)
        {
            var versionedComponent = await _componentWriter.GetVersionedComponent(versionedComponentId);
            if (versionedComponent == null)
                return NotFound("The component does not contain a gameversion with the given id.");

            if (versionedComponent.LockedMappingTypes.Any(lmt => lmt.MappingType.Name == mappingTypeName))
                return BadRequest("The component is already locked for the given version");

            var user = await _userResolvingService.Get();
            if (user == null)
                return Unauthorized();

            if (!user.CanLock)
                return Forbid();

            var lockingMappingEntry = new LockingEntry()
            {
                Id = Guid.NewGuid(),
                MappingType = await _mappingTypeReader.GetByName(mappingTypeName),
                VersionedComponent = versionedComponent
            };

            versionedComponent.LockedMappingTypes.Add(lockingMappingEntry);

            foreach (var proposalMappingEntry in versionedComponent.Proposals.Where(p => p.MappingType.Name == mappingTypeName && p.IsOpen))
            {
                await ProcessClosing(false, proposalMappingEntry, user);
            }

            await _componentWriter.Update(versionedComponent);
            await _componentWriter.SaveChanges();

            return Accepted();
        }

        /// <summary>
        /// Unlocks the mappings of the given type in the versioned component of the component with the given game version.
        /// </summary>
        /// <param name="componentId">The id of the component to unlock.</param>
        /// <param name="gameVersionId">The id of the gameversion to unlock.</param>
        /// <param name="mappingTypeName">The name of the mappingtype to unlock.</param>
        /// <returns>An http status code: 401-No component with the id exists, or the given component does not exist in the given gameversion, 400-If the component is already unlocked, 403-Forbidden the user does not have the right to unlock a component, 202-If the unlocking was successful.</returns>
        [HttpPost("unlock/{componentId}/{gameVersionId}/{mappingTypeName}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> UnlockByComponentAndGameVersion(Guid componentId, Guid gameVersionId, string mappingTypeName)
        {
            var currentComponent = await _componentWriter.GetById(componentId);
            if (currentComponent == null)
                return NotFound("No component with the given id exists.");

            var versionedComponent =
                currentComponent.VersionedComponents.FirstOrDefault(vc => vc.GameVersion.Id == gameVersionId);
            if (versionedComponent == null)
                return NotFound("The component does not contain a gameversion with the given id.");

            if (versionedComponent.LockedMappingTypes.All(lmt => lmt.MappingType.Name != mappingTypeName))
                return BadRequest("The component is not locked for the given version");

            var user = await _userResolvingService.Get();
            if (user == null)
                return Unauthorized();

            if (!user.CanUnlock)
                return Forbid();

            var lockedMappingType =
                versionedComponent.LockedMappingTypes.FirstOrDefault(lmt => lmt.MappingType.Name != mappingTypeName);
            versionedComponent.LockedMappingTypes.Remove(lockedMappingType);

            await _componentWriter.Update(versionedComponent);
            await _componentWriter.SaveChanges();

            return Accepted();
        }

        /// <summary>
        /// Unlocks the mappings of the given type in the versioned component.
        /// </summary>
        /// <param name="versionedComponentId">The id of the versioned component to unlock.</param>
        /// <param name="mappingTypeName">The name of the mappingtype to unlock.</param>
        /// <returns>An http status code: 401-No component with the id exists, or the given component does not exist in the given gameversion, 400-If the component is already unlocked, 403-Forbidden the user does not have the right to unlock a component, 202-If the unlocking was successful.</returns>
        [HttpPost("unlock/{versionedComponentId}/{mappingTypeName}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize()]
        public async Task<ActionResult> UnlockByVersionedComponent(Guid versionedComponentId, string mappingTypeName)
        {
            var versionedComponent = await _componentWriter.GetVersionedComponent(versionedComponentId);
            if (versionedComponent == null)
                return NotFound("The component does not contain a gameversion with the given id.");

            if (versionedComponent.LockedMappingTypes.All(lmt => lmt.MappingType.Name == mappingTypeName))
                return BadRequest("The component is not locked for the given version");

            var user = await _userResolvingService.Get();
            if (user == null)
                return Unauthorized();

            if (!user.CanUnlock)
                return Forbid();

            var lockedMappingType =
                versionedComponent.LockedMappingTypes.FirstOrDefault(lmt => lmt.MappingType.Name != mappingTypeName);
            versionedComponent.LockedMappingTypes.Remove(lockedMappingType);

            await _componentWriter.Update(versionedComponent);
            await _componentWriter.SaveChanges();

            return Accepted();
        }

        private async Task ProcessClosing(bool merge, ProposalMappingEntry currentProposal, User user)
        {
            currentProposal.ClosedBy = user.Id;
            currentProposal.ClosedOn = DateTime.Now;
            currentProposal.Merged = merge;

            if (merge)
            {
                var newCommittedMapping = new LiveMappingEntry()
                {
                    InputMapping = currentProposal.InputMapping,
                    OutputMapping = currentProposal.OutputMapping,
                    Proposal = currentProposal,
                    Releases = new List<ReleaseComponent>(),
                    VersionedComponent = currentProposal.VersionedComponent,
                    CreatedOn = DateTime.Now
                };

                currentProposal.VersionedComponent.Mappings.Add(newCommittedMapping);
            }

            await _componentWriter.SaveChanges();
        }
    }
}
