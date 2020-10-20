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
using RailWiki.Shared.Models.Roster;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Roster;

namespace RailWiki.Api.Controllers
{
    /// <summary>
    /// Manages locomotive types (aka models)
    /// </summary>
    [Route("locomotive-types")]
    public class LocomotiveTypesController : BaseApiController
    {
        private readonly IRepository<LocomotiveType> _locomotiveTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LocomotiveTypesController> _logger;

        public LocomotiveTypesController(IRepository<LocomotiveType> locomotiveTypeRepository,
            IMapper mapper,
            ILogger<LocomotiveTypesController> logger)
        {
            _locomotiveTypeRepository = locomotiveTypeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of locomotiveTypes
        /// </summary>
        /// <param name="manufacturer">Optional. The manufacturer name to filter by (contains)</param>
        /// <param name="name">Optional. The locomotiveType name to filter by (contains)</param>
        /// <returns>A list of locomotiveTypes</returns>
        /// <response code="200">The list of locomotiveTypes</response>
        [HttpGet("")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<LocomotiveTypeModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<LocomotiveTypeModel>>> Get(string manufacturer = null, string name = null)
        {
            var locomotiveTypes = await _locomotiveTypeRepository.TableNoTracking
                .Where(x => (string.IsNullOrEmpty(name) || x.Name.Contains(name))
                    && (string.IsNullOrEmpty(manufacturer) || x.Manufacturer.Contains(manufacturer)))
                .OrderBy(x => x.Manufacturer)
                .ThenBy(x => x.Name)
                .ProjectTo<LocomotiveTypeModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(locomotiveTypes);
        }

        /// <summary>
        /// Get a locomotiveType by ID
        /// </summary>
        /// <param name="id">The locomotiveType ID</param>
        /// <returns>The requested locomotiveType</returns>
        /// <response code="200">The requested locomotiveType</response>
        /// <response code="404">LocomotiveType not found</response>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LocomotiveTypeModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocomotiveTypeModel>> GetById(int id)
        {
            var locomotiveType = await _locomotiveTypeRepository.GetByIdAsync(id);
            if (locomotiveType == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<LocomotiveTypeModel>(locomotiveType);
            return result;
        }

        /// <summary>
        /// Create a new locomotiveType
        /// </summary>
        /// <param name="model">The locomotiveType to create</param>
        /// <returns>Newly created locomotiveType</returns>
        /// <response code="201">The locomotiveType was created</response>
        /// <response code="400">Invalid locomotiveType data specified</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(LocomotiveTypeModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LocomotiveTypeModel>> Create(LocomotiveTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var locomotiveType = new LocomotiveType
            {
                Family = model.Family,
                Manufacturer = model.Manufacturer,
                Name = model.Name
            };

            await _locomotiveTypeRepository.CreateAsync(locomotiveType);

            model = _mapper.Map<LocomotiveTypeModel>(locomotiveType);

            return CreatedAtAction(nameof(GetById), new { id = locomotiveType.Id }, model);
        }

        /// <summary>
        /// Update a locomotiveType
        /// </summary>
        /// <param name="id">ID of locomotiveType to update</param>
        /// <param name="model">Updated locomotiveType information</param>
        /// <returns>The updated locomotiveType</returns>
        /// <response code="200">LocomotiveType was updated</response>
        /// <response code="400">Invalid locomotiveType data specified</response>
        /// <response code="404">LocomotiveType not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LocomotiveTypeModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocomotiveTypeModel>> Update(int id, LocomotiveTypeModel model)
        {
            var locomotiveType = await _locomotiveTypeRepository.GetByIdAsync(id);
            if (locomotiveType == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            locomotiveType.Family = model.Family;
            locomotiveType.Manufacturer = model.Manufacturer;
            locomotiveType.Name = model.Name;

            await _locomotiveTypeRepository.UpdateAsync(locomotiveType);

            return Ok(model);
        }
    }
}
