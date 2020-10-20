using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RailWiki.Shared.Models.Geography;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Geography;

namespace RailWiki.Api.Controllers
{
    [Route("countries/{countryId}/states")]
    public class StatesController : BaseApiController
    {
        private readonly IRepository<StateProvince> _stateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StatesController> _logger;

        public StatesController(IRepository<StateProvince> stateRepository,
            IMapper mapper,
            ILogger<StatesController> logger)
        {
            _stateRepository = stateRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<StateProvinceModel>>> Get(int countryId)
        {
            var countries = await _stateRepository.TableNoTracking
                .Where(x => x.CountryId == countryId)
                .OrderBy(x => x.Name)
                .ProjectTo<StateProvinceModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(countries);
        }
    }
}
