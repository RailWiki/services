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
    /// Manages rolling stock
    /// </summary>
    [Route("rolling-stock")]
    public class RollingStockController : BaseApiController
    {
        private readonly IRepository<RollingStock> _rollingStockRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RollingStockController> _logger;

        public RollingStockController(IRepository<RollingStock> rollingStockRepository,
            IMapper mapper,
            ILogger<RollingStockController> logger)
        {
            _rollingStockRepository = rollingStockRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of rollingStocks
        /// </summary>
        /// <param name="roadId">Road ID</param>
        /// <param name="roadNumber">Road number</param>
        /// <param name="typeId">Type ID</param>
        /// <param name="classId">Classifiction ID</param>
        /// <returns>A list of rolling stock</returns>
        /// <response code="200">The list of rolling stock</response>
        [HttpGet("")]
        [ProducesResponseType(typeof(List<RollingStockModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<RollingStockModel>>> Get(int? roadId = null, string roadNumber = null, int? typeId = null, int? classId = null)
        {
            var rollingStocks = await _rollingStockRepository.TableNoTracking
                .Where(x => (!roadId.HasValue || x.RoadId == roadId)
                    && (string.IsNullOrEmpty(roadNumber) || x.RoadNumber == roadNumber)
                    && (!typeId.HasValue || x.RollingStockTypeId == typeId.Value)
                    && (!classId.HasValue || x.RollingStockClassId == classId.Value))
                .OrderBy(x => x.Road.Name)
                .ThenBy(x => x.RoadNumber)
                .ProjectTo<RollingStockModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(rollingStocks);
        }

        /// <summary>
        /// Get a rollingStock by ID
        /// </summary>
        /// <param name="id">The rollingStock ID</param>
        /// <returns>The requested rollingStock</returns>
        /// <response code="200">The requested rollingStock</response>
        /// <response code="404">RollingStock not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RollingStockModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RollingStockModel>> GetById(int id)
        {
            var rollingStock = await _rollingStockRepository.GetByIdAsync(id);
            if (rollingStock == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<RollingStockModel>(rollingStock);
            return result;
        }

        /// <summary>
        /// Create a new rollingStock
        /// </summary>
        /// <param name="model">The rollingStock to create</param>
        /// <returns>Newly created rollingStock</returns>
        /// <response code="201">The rollingStock was created</response>
        /// <response code="400">Invalid rollingStock data specified</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(RollingStockModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RollingStockModel>> Create(RollingStockModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Validate road, road + road number combo
            // TODO: Validate rs type and rs class

            var rollingStock = new RollingStock
            {
                RoadId = model.RoadId,
                RoadNumber = model.RoadNumber,
                Notes = model.Notes,
                Slug = model.Slug, // TODO: auto generate = road marks + road number
                RollingStockTypeId = model.RollingStockTypeId,
                RollingStockClassId = model.RollingStockClassId,
                Details = model.Details,
                Plate = model.Plate,
                MaxGrossWeight = model.MaxGrossWeight,
                LoadLimit = model.LoadLimit,
                DryCapacity = model.DryCapacity,
                ExteriorDimensions = model.ExteriorDimensions,
                InteriorDimensions = model.InteriorDimensions
            };

            await _rollingStockRepository.CreateAsync(rollingStock);

            model = _mapper.Map<RollingStockModel>(rollingStock);

            return CreatedAtAction(nameof(GetById), new { id = rollingStock.Id }, model);
        }

        /// <summary>
        /// Update a rollingStock
        /// </summary>
        /// <param name="id">ID of rollingStock to update</param>
        /// <param name="model">Updated rollingStock information</param>
        /// <returns>The updated rollingStock</returns>
        /// <response code="200">RollingStock was updated</response>
        /// <response code="400">Invalid rollingStock data specified</response>
        /// <response code="404">RollingStock not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RollingStockModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RollingStockModel>> Update(int id, RollingStockModel model)
        {
            var rollingStock = await _rollingStockRepository.GetByIdAsync(id);
            if (rollingStock == null)
            {
                return NotFound();
            }

            // TODO: Validate rollingStock type and parent

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Road, road number and slug cannot be updated - create new rollingStock record
            rollingStock.Notes = model.Notes;
            rollingStock.RollingStockTypeId = model.RollingStockTypeId;
            rollingStock.RollingStockClassId = model.RollingStockClassId;
            rollingStock.Details = model.Details;
            rollingStock.Plate = model.Plate;
            rollingStock.MaxGrossWeight = model.MaxGrossWeight;
            rollingStock.LoadLimit = model.LoadLimit;
            rollingStock.DryCapacity = model.DryCapacity;
            rollingStock.ExteriorDimensions = model.ExteriorDimensions;
            rollingStock.InteriorDimensions = model.InteriorDimensions;

            await _rollingStockRepository.UpdateAsync(rollingStock);

            return Ok(model);
        }
    }
}
