using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    /// Manages rolling stock types
    /// </summary>
    [Route("rolling-stock-types")]
    public class RollingStockTypesController : BaseApiController
    {
        private readonly IRepository<RollingStockType> _rollingStockTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RollingStockTypesController> _logger;

        public RollingStockTypesController(IRepository<RollingStockType> rollingStockTypeRepository,
            IMapper mapper,
            ILogger<RollingStockTypesController> logger)
        {
            _rollingStockTypeRepository = rollingStockTypeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of rollingStockTypes
        /// </summary>
        /// <param name="name">Optional. The rollingStockType name to filter by (contains)</param>
        /// <returns>A list of rollingStockTypes</returns>
        /// <response code="200">The list of rollingStockTypes</response>
        [HttpGet("")]
        [ProducesResponseType(typeof(List<RollingStockTypeModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<RollingStockTypeModel>>> Get(string name = null)
        {
            var rollingStockTypes = await _rollingStockTypeRepository.TableNoTracking
                .Where(x => (string.IsNullOrEmpty(name) || x.Name.Contains(name)))
                .OrderBy(x => x.Name)
                .ProjectTo<RollingStockTypeModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(rollingStockTypes);
        }

        /// <summary>
        /// Get a rollingStockType by ID
        /// </summary>
        /// <param name="id">The rollingStockType ID</param>
        /// <returns>The requested rollingStockType</returns>
        /// <response code="200">The requested rollingStockType</response>
        /// <response code="404">RollingStockType not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(RollingStockTypeModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RollingStockTypeModel>> GetById(int id)
        {
            var rollingStockType = await _rollingStockTypeRepository.GetByIdAsync(id);
            if (rollingStockType == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<RollingStockTypeModel>(rollingStockType);
            return result;
        }

        /// <summary>
        /// Create a new rollingStockType
        /// </summary>
        /// <param name="model">The rollingStockType to create</param>
        /// <returns>Newly created rollingStockType</returns>
        /// <response code="201">The rollingStockType was created</response>
        /// <response code="400">Invalid rollingStockType data specified</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(RollingStockTypeModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RollingStockTypeModel>> Create(RollingStockTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rollingStockType = new RollingStockType
            {
                Name = model.Name,
                AARDesignation = model.AARDesignation,
                Description = model.Description
            };

            await _rollingStockTypeRepository.CreateAsync(rollingStockType);

            model = _mapper.Map<RollingStockTypeModel>(rollingStockType);

            return CreatedAtAction(nameof(GetById), new { id = rollingStockType.Id }, model);
        }

        /// <summary>
        /// Update a rollingStockType
        /// </summary>
        /// <param name="id">ID of rollingStockType to update</param>
        /// <param name="model">Updated rollingStockType information</param>
        /// <returns>The updated rollingStockType</returns>
        /// <response code="200">RollingStockType was updated</response>
        /// <response code="400">Invalid rollingStockType data specified</response>
        /// <response code="404">RollingStockType not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RollingStockTypeModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RollingStockTypeModel>> Update(int id, RollingStockTypeModel model)
        {
            var rollingStockType = await _rollingStockTypeRepository.GetByIdAsync(id);
            if (rollingStockType == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            rollingStockType.Name = model.Name;
            rollingStockType.AARDesignation = model.AARDesignation;
            rollingStockType.Description = model.Description;

            await _rollingStockTypeRepository.UpdateAsync(rollingStockType);

            return Ok(model);
        }
    }
}
