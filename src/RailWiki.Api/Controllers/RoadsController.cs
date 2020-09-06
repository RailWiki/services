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
using RailWiki.Api.Models;
using RailWiki.Api.Models.Roster;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Roster;

namespace RailWiki.Api.Controllers
{
    /// <summary>
    /// Manages railroad companies ("roads")
    /// </summary>
    [Route("roads")]
    public class RoadsController : BaseApiController
    {
        private readonly IRepository<Road> _roadRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoadsController> _logger;

        public RoadsController(IRepository<Road> roadRepository,
            IMapper mapper,
            ILogger<RoadsController> logger)
        {
            _roadRepository = roadRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of roads
        /// </summary>
        /// <param name="name">Optional. The road name to filter by (contains)</param>
        /// <param name="typeId">Optional. The roadTypeId to filter results by</param>
        /// <param name="page">Current page of results. Defaults to 1.</param>
        /// <param name="pageSize">Number of results to take per request. Defaults to 50.</param>
        /// <returns>A list of roads</returns>
        /// <response code="200">The list of roads</response>
        [HttpGet("")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<RoadModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<RoadModel>>> Get(string name = null, int? typeId = null, int page = 1, int pageSize = 50)
        {
            var roads = _roadRepository.TableNoTracking
                .Where(x => (string.IsNullOrEmpty(name) || x.Name.Contains(name))
                    && (!typeId.HasValue || x.RoadTypeId == typeId.Value))
                .OrderBy(x => x.Name)
                .ProjectTo<RoadModel>(_mapper.ConfigurationProvider);

            // TODO: Not sure if i Like this or not, but we'll see how it works
            var pagedResponse = new PagedResponse<RoadModel>(pageSize, page);
            await pagedResponse.PaginateResultsAsync(roads);

            AddPaginationResponseHeaders(pagedResponse);

            return Ok(pagedResponse.Data);
        }

        /// <summary>
        /// Get a road by ID
        /// </summary>
        /// <param name="id">The road ID</param>
        /// <returns>The requested road</returns>
        /// <response code="200">The requested road</response>
        /// <response code="404">Road not found</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RoadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoadModel>> GetById(int id)
        {
            var road = await _roadRepository.GetByIdAsync(id);
            if (road == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<RoadModel>(road);
            return result;
        }

        /// <summary>
        /// Create a new road
        /// </summary>
        /// <param name="model">The road to create</param>
        /// <returns>Newly created road</returns>
        /// <response code="201">The road was created</response>
        /// <response code="400">Invalid road data specified</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(RoadModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoadModel>> Create(RoadModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var road = new Road
            {
                RoadTypeId = model.RoadTypeId, // TODO: validate
                ParentId = model.ParentId, // TODO: validate
                Name = model.Name,
                Slug = model.Slug, // TODO: auto generate
                ReportingMarks = model.ReportingMarks
            };

            await _roadRepository.CreateAsync(road);

            model = _mapper.Map<RoadModel>(road);

            return CreatedAtAction(nameof(GetById), new { id = road.Id }, model);
        }

        /// <summary>
        /// Update a road
        /// </summary>
        /// <param name="id">ID of road to update</param>
        /// <param name="model">Updated road information</param>
        /// <returns>The updated road</returns>
        /// <response code="200">Road was updated</response>
        /// <response code="400">Invalid road data specified</response>
        /// <response code="404">Road not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RoadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoadModel>> Update(int id, RoadModel model)
        {
            var road = await _roadRepository.GetByIdAsync(id);
            if (road == null)
            {
                return NotFound();
            }

            // TODO: Validate road type and parent

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            road.RoadTypeId = model.RoadTypeId;
            road.ParentId = model.ParentId;
            road.Name = model.Name;
            // Slug cannot be updated via API
            road.ReportingMarks = model.ReportingMarks;

            await _roadRepository.UpdateAsync(road);

            return Ok(model);
        }
    }
}
