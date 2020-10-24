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
using RailWiki.Shared.Models.Roster;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Roster;
using RailWiki.Shared.Helpers;
using RailWiki.Shared.Services.Roster;
using RailWiki.Shared.Security;

namespace RailWiki.Api.Controllers
{
    /// <summary>
    /// Manages locomotives
    /// </summary>
    [Route("locomotives")]
    public class LocomotivesController : BaseApiController
    {
        private readonly IRepository<Locomotive> _locomotiveRepository;
        private readonly IRoadService _roadService;
        private readonly IMapper _mapper;
        private readonly ILogger<LocomotivesController> _logger;

        public LocomotivesController(IRepository<Locomotive> locomotiveRepository,
            IRoadService roadService,
            IMapper mapper,
            ILogger<LocomotivesController> logger)
        {
            _locomotiveRepository = locomotiveRepository;
            _roadService = roadService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of locomotives
        /// </summary>
        /// <param name="roadId">Road ID</param>
        /// <param name="roadNumber">Road number</param>
        /// <param name="modelNumber">Model number</param>
        /// <param name="serialNumber">Serial number</param>
        /// <returns>A list of locomotives</returns>
        /// <response code="200">The list of locomotives</response>
        [HttpGet("")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<LocomotiveModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<LocomotiveModel>>> Get(int? roadId = null, string roadNumber = null, string modelNumber = null, string serialNumber = null, int page = 1, int pageSize = 50)
        {
            var locomotives = _locomotiveRepository.TableNoTracking
                .Include(x => x.Road)
                .Where(x => (!roadId.HasValue || x.RoadId == roadId)
                    && (string.IsNullOrEmpty(roadNumber) || x.ReportingMarks.Contains(roadNumber)) // TODO: Ideally don't want to do a contains here
                    // ... but the road rpt marks are part of the road number, so we have to for now...
                    && (string.IsNullOrEmpty(modelNumber) || x.ModelNumber == modelNumber)
                    && (string.IsNullOrEmpty(serialNumber) || x.SerialNumber == serialNumber))
                .OrderBy(x => x.Road.Name)
                .ThenBy(x => x.RoadNumber)
                .ProjectTo<LocomotiveModel>(_mapper.ConfigurationProvider);

            var pagedResponse = new PagedResponse<LocomotiveModel>(pageSize, page);
            await pagedResponse.PaginateResultsAsync(locomotives);

            AddPaginationResponseHeaders(pagedResponse);

            return Ok(locomotives);
        }

        /// <summary>
        /// Get a locomotive by ID
        /// </summary>
        /// <param name="id">The locomotive ID</param>
        /// <returns>The requested locomotive</returns>
        /// <response code="200">The requested locomotive</response>
        /// <response code="404">Locomotive not found</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LocomotiveModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocomotiveModel>> GetById(int id)
        {
            var locomotive = await _locomotiveRepository.GetByIdAsync(id);
            if (locomotive == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<LocomotiveModel>(locomotive);
            return result;
        }

        /// <summary>
        /// Create a new locomotive
        /// </summary>
        /// <param name="model">The locomotive to create</param>
        /// <returns>Newly created locomotive</returns>
        /// <response code="201">The locomotive was created</response>
        /// <response code="400">Invalid locomotive data specified</response>
        [HttpPost("")]
        [Authorize(Policy = Policies.ApprovedUser)]
        [ProducesResponseType(typeof(LocomotiveModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LocomotiveModel>> Create(LocomotiveModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var road = await _roadService.GetById(model.RoadId);
            if (road == null)
            {
                ModelState.AddModelError(nameof(model.RoadId), "Road is not valid");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Validate road number (numbers only) road + road number combo

            var locomotive = new Locomotive
            {
                RoadId = road.Id,
                RoadNumber = model.RoadNumber,
                ReportingMarks = $"{road.ReportingMarks} {model.RoadNumber}",
                Notes = model.Notes,
                // Slug is generated below
                ModelNumber = model.ModelNumber,
                SerialNumber = model.SerialNumber,
                FrameNumber = model.FrameNumber,
                BuiltAs = model.BuiltAs,
                BuildMonth = model.BuildMonth,
                BuildYear = model.BuildYear
            };

            locomotive.Slug = locomotive.ReportingMarks.Slugify();

            await _locomotiveRepository.CreateAsync(locomotive);

            // Re-fetch the locomotive to make sure the model is fully hydrated
            locomotive = await _locomotiveRepository.GetByIdAsync(locomotive.Id);

            model = _mapper.Map<LocomotiveModel>(locomotive);

            return CreatedAtAction(nameof(GetById), new { id = locomotive.Id }, model);
        }

        /// <summary>
        /// Update a locomotive
        /// </summary>
        /// <param name="id">ID of locomotive to update</param>
        /// <param name="model">Updated locomotive information</param>
        /// <returns>The updated locomotive</returns>
        /// <response code="200">Locomotive was updated</response>
        /// <response code="400">Invalid locomotive data specified</response>
        /// <response code="404">Locomotive not found</response>
        [HttpPut("{id}")]
        [Authorize(Policy = Policies.ApprovedUser)]
        [ProducesResponseType(typeof(LocomotiveModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocomotiveModel>> Update(int id, LocomotiveModel model)
        {
            var locomotive = await _locomotiveRepository.GetByIdAsync(id);
            if (locomotive == null)
            {
                return NotFound();
            }

            // TODO: Validate locomotive type and parent

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Road, road number and slug cannot be updated - create new locomotive record
            locomotive.Notes = model.Notes;
            locomotive.ModelNumber = model.ModelNumber;
            locomotive.SerialNumber = model.SerialNumber;
            locomotive.FrameNumber = model.FrameNumber;
            locomotive.BuiltAs = model.BuiltAs;
            locomotive.BuildMonth = model.BuildMonth;
            locomotive.BuildYear = model.BuildYear;

            await _locomotiveRepository.UpdateAsync(locomotive);

            return Ok(model);
        }
    }
}
