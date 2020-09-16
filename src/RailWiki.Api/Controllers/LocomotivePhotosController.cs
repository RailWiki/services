using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RailWiki.Api.Models.Photos;
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

        public LocomotivePhotosController(IPhotoLocomotiveService photoLocomotiveService,
            IPhotoService photoService)
        {
            _photoLocomotiveService = photoLocomotiveService;
            _photoService = photoService;
        }

        [HttpGet("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PhotoModel>>> Get(int locomotiveId)
        {
            var photoIds = await _photoLocomotiveService.GetPhotoIdsForLocomotives(locomotiveId);
            var photos = await _photoService.GetByIdsAsync(photoIds);

            return Ok(photos);
        }
    }
}
