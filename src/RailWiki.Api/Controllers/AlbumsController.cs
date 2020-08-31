using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RailWiki.Api.Models.Entities.Photos;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Photos;

namespace RailWiki.Api.Controllers
{
    /// <summary>
    /// Manages albums
    /// </summary>
    [Route("albums")]
    public class AlbumsController : BaseApiController
    {
        private readonly IRepository<Album> _albumRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AlbumsController> _logger;

        public AlbumsController(IRepository<Album> albumRepository,
            IMapper mapper,
            ILogger<AlbumsController> logger)
        {
            _albumRepository = albumRepository;
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
        [ProducesResponseType(typeof(List<AlbumModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AlbumModel>>> Get(int? userId = null, string title = null)
        {
            // TODO: check to make sure user can view albums
            var albums = await _albumRepository.TableNoTracking
                .Where(x => (!userId.HasValue || x.UserId == userId.Value)
                    && (string.IsNullOrEmpty(title) || x.Title.Contains(title)))
                .OrderBy(x => x.Title)
                .ProjectTo<AlbumModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(albums);
        }

        /// <summary>
        /// Get a list of albums for current user
        /// </summary>
        /// <returns>A list of albums</returns>
        /// <param name="title">The title of albums to search for (contains)</param>
        /// <response code="200">The list of albums</response>
        [HttpGet("mine")]
        [ProducesResponseType(typeof(List<AlbumModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AlbumModel>>> GetCurrentUser(string title = null)
        {
            // TODO: check to make sure user can view albums
            var albums = await _albumRepository.TableNoTracking
                .Where(x => x.UserId == User.GetUserId()
                    && (string.IsNullOrEmpty(title) || x.Title.Contains(title)))
                .OrderBy(x => x.Title)
                .ProjectTo<AlbumModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

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
        [ProducesResponseType(typeof(AlbumModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AlbumModel>> GetById(int id)
        {
            // TODO: check to make sure user can view albums
            var album = await _albumRepository.GetByIdAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<AlbumModel>(album);
            return result;
        }

        /// <summary>
        /// Create a new album
        /// </summary>
        /// <param name="model">The album to create</param>
        /// <returns>Newly created album</returns>
        /// <response code="201">The album was created</response>
        /// <response code="400">Invalid album data specified</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(AlbumModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AlbumModel>> Create(AlbumModel model)
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
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            await _albumRepository.CreateAsync(album);

            // TODO: Make sure the related props are set
            var newAlbum = await _albumRepository.GetByIdAsync(album.Id);
            model = _mapper.Map<AlbumModel>(newAlbum);

            return CreatedAtAction(nameof(GetById), new { id = album.Id }, model);
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
        [ProducesResponseType(typeof(AlbumModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AlbumModel>> Update(int id, AlbumModel model)
        {
            var album = await _albumRepository.GetByIdAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            if (album.UserId != User.GetUserId())
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            album.Title = model.Title;
            album.Description = model.Description;
            album.UpdatedOn = DateTime.UtcNow;

            await _albumRepository.UpdateAsync(album);

            return Ok(model);
        }
    }
}
