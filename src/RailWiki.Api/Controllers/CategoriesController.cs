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
    /// Manages categories
    /// </summary>
    [Route("categories")]
    public class CategoriesController : BaseApiController
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IRepository<Category> categoryRepository,
            IMapper mapper,
            ILogger<CategoriesController> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of categories
        /// </summary>
        /// <param name="roadId">Road ID</param>
        /// <param name="roadNumber">Road number</param>
        /// <param name="modelNumber">Model number</param>
        /// <param name="serialNumber">Serial number</param>
        /// <returns>A list of categories</returns>
        /// <response code="200">The list of categories</response>
        [HttpGet("")]
        [ProducesResponseType(typeof(List<CategoryModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CategoryModel>>> Get()
        {
            var categories = await _categoryRepository.TableNoTracking
                .OrderBy(x => x.Name)
                .ProjectTo<CategoryModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(categories);
        }

        /// <summary>
        /// Get a category by ID
        /// </summary>
        /// <param name="id">The category ID</param>
        /// <returns>The requested category</returns>
        /// <response code="200">The requested category</response>
        /// <response code="404">Category not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryModel>> GetById(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<CategoryModel>(category);
            return result;
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="model">The category to create</param>
        /// <returns>Newly created category</returns>
        /// <response code="201">The category was created</response>
        /// <response code="400">Invalid category data specified</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryModel>> Create(CategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category
            {
                Name = model.Name
            };

            await _categoryRepository.CreateAsync(category);

            model = _mapper.Map<CategoryModel>(category);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, model);
        }

        /// <summary>
        /// Update a category
        /// </summary>
        /// <param name="id">ID of category to update</param>
        /// <param name="model">Updated category information</param>
        /// <returns>The updated category</returns>
        /// <response code="200">Category was updated</response>
        /// <response code="400">Invalid category data specified</response>
        /// <response code="404">Category not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryModel>> Update(int id, CategoryModel model)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            category.Name = model.Name;

            await _categoryRepository.UpdateAsync(category);

            return Ok(model);
        }
    }
}
