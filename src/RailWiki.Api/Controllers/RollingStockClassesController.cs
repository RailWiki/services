using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RailWiki.Shared.Models.Roster;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Roster;

namespace RailWiki.Api.Controllers
{
    /// <summary>
    /// Manages rolling stock classes
    /// </summary>
    [Route("rolling-stock-classes")]
    public class RollingStockClasssController : BaseApiController
    {
        private readonly IRepository<RollingStockClass> _rollingStockClassRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RollingStockClasssController> _logger;

        public RollingStockClasssController(IRepository<RollingStockClass> rollingStockClassRepository,
            IMapper mapper,
            ILogger<RollingStockClasssController> logger)
        {
            _rollingStockClassRepository = rollingStockClassRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of rollingStockClasss
        /// </summary>
        /// <param name="name">Optional. The rollingStockClass name to filter by (contains)</param>
        /// <returns>A list of rollingStockClasss</returns>
        /// <response code="200">The list of rollingStockClasss</response>
        [HttpGet("")]
        [ProducesResponseType(typeof(List<RollingStockClassModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<RollingStockClassModel>>> Get(string name = null)
        {
            var rollingStockClasss = await _rollingStockClassRepository.TableNoTracking
                .Where(x => (string.IsNullOrEmpty(name) || x.Name.Contains(name)))
                .OrderBy(x => x.Name)
                .ProjectTo<RollingStockClassModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(rollingStockClasss);
        }

        /// <summary>
        /// Get a rollingStockClass by ID
        /// </summary>
        /// <param name="id">The rollingStockClass ID</param>
        /// <returns>The requested rollingStockClass</returns>
        /// <response code="200">The requested rollingStockClass</response>
        /// <response code="404">RollingStockClass not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(RollingStockClassModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RollingStockClassModel>> GetById(int id)
        {
            var rollingStockClass = await _rollingStockClassRepository.GetByIdAsync(id);
            if (rollingStockClass == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<RollingStockClassModel>(rollingStockClass);
            return result;
        }

        /// <summary>
        /// Create a new rollingStockClass
        /// </summary>
        /// <param name="model">The rollingStockClass to create</param>
        /// <returns>Newly created rollingStockClass</returns>
        /// <response code="201">The rollingStockClass was created</response>
        /// <response code="400">Invalid rollingStockClass data specified</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(RollingStockClassModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RollingStockClassModel>> Create(RollingStockClassModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rollingStockClass = new RollingStockClass
            {
                Name = model.Name,
                AARDesignation = model.AARDesignation
            };

            await _rollingStockClassRepository.CreateAsync(rollingStockClass);

            model = _mapper.Map<RollingStockClassModel>(rollingStockClass);

            return CreatedAtAction(nameof(GetById), new { id = rollingStockClass.Id }, model);
        }

        /// <summary>
        /// Update a rollingStockClass
        /// </summary>
        /// <param name="id">ID of rollingStockClass to update</param>
        /// <param name="model">Updated rollingStockClass information</param>
        /// <returns>The updated rollingStockClass</returns>
        /// <response code="200">RollingStockClass was updated</response>
        /// <response code="400">Invalid rollingStockClass data specified</response>
        /// <response code="404">RollingStockClass not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RollingStockClassModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RollingStockClassModel>> Update(int id, RollingStockClassModel model)
        {
            var rollingStockClass = await _rollingStockClassRepository.GetByIdAsync(id);
            if (rollingStockClass == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            rollingStockClass.Name = model.Name;
            rollingStockClass.AARDesignation = model.AARDesignation;

            await _rollingStockClassRepository.UpdateAsync(rollingStockClass);

            return Ok(model);
        }
    }
}
