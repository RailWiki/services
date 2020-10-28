using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Photos;
using RailWiki.Shared.Models.Photos;
using RailWiki.Shared.Security;
using RailWiki.Shared.Services.Photos;

namespace RailWiki.Api.Controllers
{
    /// <summary>
    /// Manages albums
    /// </summary>
    [Route("albums")]
    public class AlbumsController : BaseApiController
    {
        private readonly IRepository<Album> _albumRepository;
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly ILogger<AlbumsController> _logger;

        public AlbumsController(IRepository<Album> albumRepository,
            IAlbumService albumService,
            IPhotoService photoService,
            IAuthorizationService authorizationService,
            IMapper mapper,
            ILogger<AlbumsController> logger)
        {
            _albumRepository = albumRepository;
            _albumService = albumService;
            _photoService = photoService;
            _authorizationService = authorizationService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of albums
        /// </summary>
        /// <returns>A list of albums</returns>
        /// <param name="userId">The user ID to get albums for</param>
        /// <param name="title">The title of albums to search for (contains)</param>
        /// <response code="200">The list of albums</response>
        [HttpGet("")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetAlbumModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetAlbumModel>>> Get(int? userId = null, string title = null)
        {
            // TODO: check to make sure user can view albums
            var albums = await _albumService.GetAlbumsAsync(userId, title);

            return Ok(albums);
        }

        /// <summary>
        /// Get a list of albums for current user
        /// </summary>
        /// <returns>A list of albums</returns>
        /// <param name="title">The title of albums to search for (contains)</param>
        /// <response code="200">The list of albums</response>
        [Obsolete("Use Get() and pass in user's ID")]
        [HttpGet("mine")]
        [ProducesResponseType(typeof(List<GetAlbumModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetAlbumModel>>> GetCurrentUser(string title = null)
        {
            var albums = await _albumService.GetAlbumsAsync(User.GetUserId(), title);

            return Ok(albums);
        }

        /// <summary>
        /// Get a album by ID
        /// </summary>
        /// <param name="id">The album ID</param>
        /// <returns>The requested album</returns>
        /// <response code="200">The requested album</response>
        /// <response code="404">Album not found</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetAlbumModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAlbumModel>> GetById(int id)
        {
            // TODO: check to make sure user can view albums
            var album = await _albumService.GetAlbumByIdAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            return album;
        }

        /// <summary>
        /// Create a new album
        /// </summary>
        /// <param name="model">The album to create</param>
        /// <returns>Newly created album</returns>
        /// <response code="201">The album was created</response>
        /// <response code="400">Invalid album data specified</response>
        [HttpPost("")]
        [Authorize(Policy = Policies.ApprovedUser)]
        [ProducesResponseType(typeof(GetAlbumModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetAlbumModel>> Create(CreateAlbumModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var album = new Album
            {
                UserId = User.GetUserId(),
                Title = model.Title,
                Description = model.Description,
                LocationId = model.LocationId,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            await _albumRepository.CreateAsync(album);

            // TODO: Make sure the related props are set
            var newAlbum = await _albumRepository.GetByIdAsync(album.Id);
            var returnModel = _mapper.Map<GetAlbumModel>(newAlbum);

            return CreatedAtAction(nameof(GetById), new { id = album.Id }, returnModel);
        }

        /// <summary>
        /// Update a album
        /// </summary>
        /// <param name="id">ID of album to update</param>
        /// <param name="model">Updated album information</param>
        /// <returns>The updated album</returns>
        /// <response code="200">Album was updated</response>
        /// <response code="400">Invalid album data specified</response>
        /// <response code="403">User cannot edit the album</response>
        /// <response code="404">Album not found</response>
        [HttpPut("{id}")]
        [Authorize(Policy = Policies.ApprovedUser)]
        [ProducesResponseType(typeof(GetAlbumModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAlbumModel>> Update(int id, CreateAlbumModel model)
        {
            var album = await _albumRepository.GetByIdAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            if (!(await _authorizationService.AuthorizeAsync(User, album, Policies.AlbumOwnerOrMod)).Succeeded)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            album.Title = model.Title;
            album.Description = model.Description;
            album.LocationId = model.LocationId;
            album.UpdatedOn = DateTime.UtcNow;

            await _albumRepository.UpdateAsync(album);

            return Ok(model);
        }

        /// <summary>
        /// Set the cover photo for an album
        /// </summary>
        /// <param name="id">The album ID</param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="204">Cover photo was updated</response>
        /// <response code="400">Invalid photo specified</response>
        /// <response code="403">User cannot edit the album</response>
        /// <response code="404">Album not found</response>
        [HttpPut("{id}/cover")]
        [Authorize(Policy = Policies.ApprovedUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> SetCoverPhoto([FromRoute] int id, SetCoverPhotoModel model)
        {
            try
            {
                var album = await _albumRepository.GetByIdAsync(id);
                if (album == null)
                {
                    return NotFound();
                }

                if (!(await _authorizationService.AuthorizeAsync(User, album, Policies.AlbumOwnerOrMod)).Succeeded)
                {
                    return Forbid();
                }

                var photo = await _photoService.GetEntityByIdAsync(model.PhotoId);
                if (photo == null || photo.AlbumId != album.Id)
                {
                    return BadRequest();
                }

                album.CoverPhotoId = photo.Id;
                album.CoverPhotoFileName = photo.Filename;

                await _albumRepository.UpdateAsync(album);

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error updating cover photo for album ID {id}");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

    public class SetCoverPhotoModel
    {
        public int PhotoId { get; set;  }
    }
}
