using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RailWiki.Api.Models.Entities.Photos;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Photos;
using RailWiki.Shared.Models.Photos;
using RailWiki.Shared.Services.Photos;

namespace RailWiki.Api.Controllers
{
    /// <summary>
    /// Manages photos
    /// </summary>
    [Route("photos")]
    public class PhotosController : BaseApiController
    {
        private readonly IRepository<Album> _albumRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        private readonly ILogger<PhotosController> _logger;

        public PhotosController(IRepository<Album> albumRepository,
            IPhotoService photoService,
            IMapper mapper,
            ILogger<PhotosController> logger)
        {
            _albumRepository = albumRepository;
            _photoService = photoService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(List<PhotoModel>), 200)]
        public async Task<ActionResult<List<PhotoModel>>> Get(int albumId)
        {
            var photos = await _photoService.GetByAlbumIdAsync(albumId);

            return Ok(photos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PhotoResponseModel>> GetById(int id)
        {
            var photo = await _photoService.GetWithFilesByIdAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(PhotoModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> Create([FromBody] int albumId, IFormFile file)
        {
            var userId = User.GetUserId();
            var album = await _albumRepository.GetByIdAsync(albumId);
            if (album == null)
            {
                return NotFound();
            }

            if (album.UserId != userId)
            {
                return Forbid();
            }

            using (var memStream = new MemoryStream())
            {
                await file.CopyToAsync(memStream);

                // TODO: file validation
                var fileBytes = memStream.ToArray();

                await _photoService.SavePhotoAsync(album, fileBytes, file.FileName, file.ContentType);
            }

            return Ok();
        }

        [HttpPost("multiple")]
        [ProducesResponseType(typeof(List<PhotoModel>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> CreateMultiple([FromBody] int albumId, List<IFormFile> files)
        {
            var userId = User.GetUserId();
            var album = await _albumRepository.GetByIdAsync(albumId);
            if (album == null)
            {
                return NotFound();
            }

            if (album.UserId != userId)
            {
                return Forbid();
            }

            var result = new List<PhotoResponseModel>();

            foreach (var file in files)
            {
                using (var memStream = new MemoryStream())
                {
                    await file.CopyToAsync(memStream);

                    // TODO: file validation
                    var fileBytes = memStream.ToArray();

                    var photoModel = await _photoService.SavePhotoAsync(album, fileBytes, file.FileName, file.ContentType);
                    result.Add(photoModel);
                }
            }

            // TODO: return 201
            return Created("", result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, PhotoModel model)
        {
            var userId = User.GetUserId();

            var photo = await _photoService.GetByIdAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            if (photo.UserId != userId)
            {
                return Forbid();
            }

            photo.Author = model.Author;
            photo.LocationName = model.LocationName;
            photo.LocationId = model.LocationId; // TODO: Validate location
            photo.Title = model.Title;
            photo.Description = model.Description;
            photo.PhotoDate = model.PhotoDate;

            await _photoService.UpdateAsync(photo);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = User.GetUserId();

            var photo = await _photoService.GetByIdAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            if (photo.UserId != userId)
            {
                return Forbid();
            }

            await _photoService.DeleteAsync(photo);

            return NoContent();
        }

    }
}
