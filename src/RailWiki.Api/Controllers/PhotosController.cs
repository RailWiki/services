using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RailWiki.Shared.Models.Photos;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Photos;
using RailWiki.Shared.Models.Photos;
using RailWiki.Shared.Security;
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
        private readonly IImportPhotoService _importPhotoService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly ILogger<PhotosController> _logger;

        public PhotosController(IRepository<Album> albumRepository,
            IPhotoService photoService,
            IImportPhotoService importPhotoService,
            IAuthorizationService authorizationService,
            IMapper mapper,
            ILogger<PhotosController> logger)
        {
            _albumRepository = albumRepository;
            _photoService = photoService;
            _importPhotoService = importPhotoService;
            _authorizationService = authorizationService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<PhotoModel>), 200)]
        public async Task<ActionResult<List<PhotoModel>>> Get(int albumId)
        {
            var photos = await _photoService.GetByAlbumIdAsync(albumId);

            return Ok(photos);
        }

        [HttpGet("latest")]
        [AllowAnonymous]
        public async Task<ActionResult<PhotoResponseModel>> GetLatest(int max = 10)
        {
            var photos = await _photoService.GetLatestAsync(max);
            return Ok(photos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PhotoResponseModel>> GetById(int id)
        {
            var photo = await _photoService.GetByIdAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }

        [HttpPost("")]
        [Authorize(Policy = Policies.ApprovedUser)]
        [ProducesResponseType(typeof(PhotoModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> Create([FromForm] int albumId, IFormFile file)
        {
            var album = await _albumRepository.GetByIdAsync(albumId);
            if (album == null)
            {
                return NotFound();
            }

            if (!(await _authorizationService.AuthorizeAsync(User, album, Policies.AlbumOwnerOrMod)).Succeeded)
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
        [Authorize(Policy = Policies.ApprovedUser)]
        [ProducesResponseType(typeof(List<PhotoModel>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<ActionResult> CreateMultiple([FromForm] int albumId, List<IFormFile> files)
        {
            var album = await _albumRepository.GetByIdAsync(albumId);
            if (album == null)
            {
                return NotFound();
            }

            if (!(await _authorizationService.AuthorizeAsync(User, album, Policies.AlbumOwnerOrMod)).Succeeded)
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

        [HttpPost("import")]
        [AllowAnonymous] // TODO: Remove
        public async Task<ActionResult> Import([FromBody] ImportPhotoModel model)
        {
            await _importPhotoService.ImportPhotoAsync(model);

            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Policies.ApprovedUser)]
        public async Task<ActionResult> Update(int id, PhotoModel model)
        {
            var photo = await _photoService.GetEntityByIdAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            if (!(await _authorizationService.AuthorizeAsync(User, photo.Album, Policies.AlbumOwnerOrMod)).Succeeded)
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
        [Authorize(Policy = Policies.ApprovedUser)]
        public async Task<ActionResult> Delete(int id)
        {
            var photo = await _photoService.GetEntityByIdAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            if (!(await _authorizationService.AuthorizeAsync(User, photo.Album, Policies.AlbumOwnerOrMod)).Succeeded)
            {
                return Forbid();
            }

            await _photoService.DeleteAsync(photo);

            return NoContent();
        }

    }
}
