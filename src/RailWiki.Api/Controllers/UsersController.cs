using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RailWiki.Api.Models;
using RailWiki.Shared.Models.Users;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Users;
using RailWiki.Shared.Models.Users;
using RailWiki.Shared.Security;
using RailWiki.Shared.Services.Users;

namespace RailWiki.Api.Controllers
{
    [Route("users")]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IUserStatsService _userStatsService;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService,
            IUserStatsService userStatsService,
            IRepository<User> userRepository,
            IMapper mapper,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _userStatsService = userStatsService;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<GetUserModel>> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<GetUserModel>(user);
            return Ok(model);
        }

        [HttpGet("{id}/stats")]
        [AllowAnonymous]
        public async Task<ActionResult<UserStatsModel>> GetStatsByUserId(int id)
        {
            var stats = await _userStatsService.GetStatsByUserId(id);
            return Ok(stats);
        }

        [HttpGet("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetUserModel>>> Get(string query = null, int page = 1, int pageSize = 50)
        {
            var users = _userRepository.TableNoTracking
                .Where(x => (string.IsNullOrEmpty(query) || x.FirstName.Contains(query) || x.LastName.Contains(query)))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ProjectTo<GetUserModel>(_mapper.ConfigurationProvider);

            var pagedResponse = new PagedResponse<GetUserModel>(pageSize, page);
            await pagedResponse.PaginateResultsAsync(users);

            AddPaginationResponseHeaders(pagedResponse);

            return Ok(pagedResponse.Data);
        }

        [HttpGet("current")]
        public async Task<ActionResult<GetUserModel>> CurrentUser()
        {
            var user = await _userService.GetUserByIdAsync(User.GetUserId());
            if (user == null)
            {
                return Forbid(); // or 404?
            }

            var userModel = _mapper.Map<GetUserModel>(user);
            return Ok(userModel);
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(CreateUserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var registerRequest = new RegisterUserRequest
                {
                    Email = model.Email,
                    Password = model.Password,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userService.RegisterUserAsync(registerRequest);
                return Ok(); // Don't necessarily want to return the user information here...
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error registering user");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
