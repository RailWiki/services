using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RailWiki.Api.Models.Roster;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Roster;

namespace RailWiki.Api.Controllers
{
    /// <summary>
    /// Manages railroad company types (major carrier, regional, etc)
    /// </summary>
    [Route("road-types")]
    public class RoadTypesController : BaseApiController
    {
        private readonly IRepository<RoadType> _roadTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoadTypesController> _logger;

        public RoadTypesController(IRepository<RoadType> roadTypeRepository,
            IMapper mapper,
            ILogger<RoadTypesController> logger)
        {
            _roadTypeRepository = roadTypeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of roadTypes
        /// </summary>
        /// <param name="name">Optional. The roadType name to filter by (contains)</param>
        /// <returns>A list of roadTypes</returns>
        /// <response code="200">The list of roadTypes</response>
        [HttpGet("")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<RoadTypeModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<RoadTypeModel>>> Get(string name = null)
        {
            var roadTypes = await _roadTypeRepository.TableNoTracking
                .Where(x => (string.IsNullOrEmpty(name) || x.Name.Contains(name)))
                .OrderBy(x => x.Name)
                .ProjectTo<RoadTypeModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(roadTypes);
        }

        /// <summary>
        /// Get a roadType by ID
        /// </summary>
        /// <param name="id">The roadType ID</param>
        /// <returns>The requested roadType</returns>
        /// <response code="200">The requested roadType</response>
        /// <response code="404">RoadType not found</response>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RoadTypeModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoadTypeModel>> GetById(int id)
        {
            var roadType = await _roadTypeRepository.GetByIdAsync(id);
            if (roadType == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<RoadTypeModel>(roadType);
            return result;
        }

        /// <summary>
        /// Get a roadType by slug
        /// </summary>
        /// <param name="slug">The roadType slug</param>
        /// <returns>The requested roadType</returns>
        /// <response code="200">The requested roadType</response>
        /// <response code="404">RoadType not found</response>
        [HttpGet("slug/{slug:length(0,50)}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RoadTypeModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoadTypeModel>> GetBySlug(string slug)
        {
            var roadType = await _roadTypeRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Slug == slug);
            if (roadType == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<RoadTypeModel>(roadType);
            return result;
        }

        /// <summary>
        /// Create a new roadType
        /// </summary>
        /// <param name="model">The roadType to create</param>
        /// <returns>Newly created roadType</returns>
        /// <response code="201">The roadType was created</response>
        /// <response code="400">Invalid roadType data specified</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(RoadTypeModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoadTypeModel>> Create(RoadTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roadType = new RoadType
            {
                Name = model.Name,
                //Slug = model.Slug, // TODO: auto generate
                DisplayOrder = model.DisplayOrder
            };

            await _roadTypeRepository.CreateAsync(roadType);

            model = _mapper.Map<RoadTypeModel>(roadType);

            return CreatedAtAction(nameof(GetById), new { id = roadType.Id }, model);
        }

        /// <summary>
        /// Update a roadType
        /// </summary>
        /// <param name="id">ID of roadType to update</param>
        /// <param name="model">Updated roadType information</param>
        /// <returns>The updated roadType</returns>
        /// <response code="200">RoadType was updated</response>
        /// <response code="400">Invalid roadType data specified</response>
        /// <response code="404">RoadType not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RoadTypeModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoadTypeModel>> Update(int id, RoadTypeModel model)
        {
            var roadType = await _roadTypeRepository.GetByIdAsync(id);
            if (roadType == null)
            {
                return NotFound();
            }

            // TODO: Validate roadType type and parent

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            roadType.Name = model.Name;
            // Slug cannot be updated via API
            roadType.DisplayOrder = model.DisplayOrder;

            await _roadTypeRepository.UpdateAsync(roadType);

            return Ok(model);
        }
    }
}
