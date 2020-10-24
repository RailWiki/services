using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RailWiki.Shared.Models.Photos;
using RailWiki.Shared.Security;
using RailWiki.Shared.Services.Photos;

namespace RailWiki.Api.Controllers
{
    /// <summary>
    /// Manages photos for a locomotive
    /// </summary>
    [Route("locomotives/{locomotiveId}/photos")]
    public class LocomotivePhotosController : BaseApiController
    {
        private readonly IPhotoLocomotiveService _photoLocomotiveService;
        private readonly IPhotoService _photoService;
        private readonly IAuthorizationService _authorizationService;

        public LocomotivePhotosController(IPhotoLocomotiveService photoLocomotiveService,
            IPhotoService photoService,
            IAuthorizationService authorizationService)
        {
            _photoLocomotiveService = photoLocomotiveService;
            _photoService = photoService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Gets the locomotives for a photo
        /// </summary>
        /// <param name="photoId">The photoId</param>
        /// <returns></returns>
        /// <response code="200">Requested locomotives for photo</response>
        [HttpGet("/photos/{photoId}/locomotives")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<PhotoLocomotiveModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PhotoLocomotiveModel>> GetByPhotoId(int photoId)
        {
            var locomotives = await _photoLocomotiveService.GetByPhotoId(photoId);
            return Ok(locomotives);
        }

        /// <summary>
        /// Gets the photos for a locomotive
        /// </summary>
        /// <param name="locomotiveId">The locomotiveId</param>
        /// <returns></returns>
        /// <response code="200">Requested photos for locomotive</response>
        [HttpGet("")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<PhotoLocomotiveModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PhotoModel>>> GetByLocomotiveId(int locomotiveId)
        {
            var photoIds = await _photoLocomotiveService.GetPhotoIdsForLocomotives(locomotiveId);
            var photos = await _photoService.GetByIdsAsync(photoIds);

            return Ok(photos);
        }

        /// <summary>
        /// Updates the locomotives for a photo.
        /// </summary>
        /// <remarks>
        /// Will add locomotives that aren't currently assigned, or remove photos that shouldn't be.
        /// </remarks>
        /// <param name="photoId">The photoId</param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Locomotives successfully updated</response>
        /// <response code="404">Photo was not found</response>
        [HttpPut("/photos/{photoId}/locomotives")]
        [Authorize(Policy = Policies.ApprovedUser)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdatePhotoLocomotives(int photoId, UpdatePhotoLocomotivesModel model)
        {
            // Only used to check if the photo exists to throw a 404
            var photo = await _photoService.GetEntityByIdAsync(photoId);
            if (photo == null)
            {
                return NotFound();
            }

            if (!(await _authorizationService.AuthorizeAsync(User, photo.Album, Policies.PhotoOwnerOrMod)).Succeeded)
            {
                return Forbid();
            }

            try
            {
                await _photoLocomotiveService.UpdatePhotoLocomotivesAsync(photoId, model.LocomotiveIds);

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
                // TODO: Log
            }
        }
    }

    public class UpdatePhotoLocomotivesModel
    {
        public List<int> LocomotiveIds { get; set; } = new List<int>();
    }
}
