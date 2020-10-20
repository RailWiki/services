using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RailWiki.Shared.Models.Geography;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Geography;

namespace RailWiki.Api.Controllers
{
    /// <summary>
    /// Manages geographical locations
    /// </summary>
    [Route("locations")]
    public class LocationsController : BaseApiController
    {
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<StateProvince> _stateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(IRepository<Location> locationRepository,
            IRepository<StateProvince> stateRepository,
            IMapper mapper,
            ILogger<LocationsController> logger)
        {
            _locationRepository = locationRepository;
            _stateRepository = stateRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of locations
        /// </summary>
        /// <param name="stateId">Optional. The StateProvinceId to filter by</param>
        /// <param name="name">Optional. The location name to filter by (contains)</param>
        /// <returns>A list of locations</returns>
        /// <response code="200">The list of locations</response>
        [HttpGet("")]
        [ProducesResponseType(typeof(List<LocationModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<LocationModel>>> Get(int? stateId = null, string name = null)
        {
            var locations = await _locationRepository.TableNoTracking
                .Where(x => (!stateId.HasValue || x.StateProvinceId == stateId.Value)
                    && (string.IsNullOrEmpty(name) || x.Name.Contains(name)))
                .OrderBy(x => x.Name)
                .ProjectTo<LocationModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(locations);
        }

        /// <summary>
        /// Get a location by ID
        /// </summary>
        /// <param name="id">The location ID</param>
        /// <returns>The requested location</returns>
        /// <response code="200">The requested location</response>
        /// <response code="404">Location not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LocationModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationModel>> GetById(int id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<LocationModel>(location);
            return result;
        }

        /// <summary>
        /// Create a new location
        /// </summary>
        /// <param name="model">The location to create</param>
        /// <returns>Newly created location</returns>
        /// <response code="201">The location was created</response>
        /// <response code="400">Invalid location data specified</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(LocationModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LocationModel>> Create(LocationModel model)
        {
            var state = await _stateRepository.GetByIdAsync(model.StateProvinceId);
            if (state == null)
            {
                ModelState.AddModelError(nameof(model.StateProvinceId), "Invalid StateProvinceId");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var location = new Location
            {
                Name = model.Name,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                StateProvinceId = state.Id
            };

            await _locationRepository.CreateAsync(location);

            location.StateProvinceId = state.Id;
            model = _mapper.Map<LocationModel>(location);

            return CreatedAtAction(nameof(GetById), new { id = location.Id }, model);
        }

        /// <summary>
        /// Update a location
        /// </summary>
        /// <param name="id">ID of location to update</param>
        /// <param name="model">Updated location information</param>
        /// <returns>The updated location</returns>
        /// <response code="200">Location was updated</response>
        /// <response code="400">Invalid location data specified</response>
        /// <response code="404">Location not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LocationModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationModel>> Update(int id, LocationModel model)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            var stateProvinceChanged = false;
            if (location.StateProvinceId != model.StateProvinceId)
            {
                var state = await _stateRepository.GetByIdAsync(model.StateProvinceId);
                if (state == null)
                {
                    ModelState.AddModelError(nameof(model.StateProvinceId), "Invalid StateProvinceId");
                }
                stateProvinceChanged = true;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            location.Name = model.Name;
            location.Latitude = model.Latitude;
            location.Longitude = model.Longitude;
            if (stateProvinceChanged)
            {
                location.StateProvinceId = model.StateProvinceId;
            }

            await _locationRepository.UpdateAsync(location);

            return Ok(model);
        }
    }
}
