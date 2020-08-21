using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RailWiki.Api.Models.Entities.Geography;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Geography;

namespace RailWiki.Api.Controllers
{
    [Route("countries")]
    public class CountriesController : BaseApiController
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(IRepository<Country> countryRepository,
            IMapper mapper,
            ILogger<CountriesController> logger)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<CountryModel>>> Get()
        {
            var countries = await _countryRepository.TableNoTracking
                .OrderBy(x => x.Name)
                .ProjectTo<CountryModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(countries);
        }
    }
}
