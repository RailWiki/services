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
using RailWiki.Api.Models.Users;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Users;
using RailWiki.Shared.Services.Users;

namespace RailWiki.Api.Controllers
{
    [Route("users")]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService,
            IRepository<User> userRepository,
            IMapper mapper,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<UserModel>>> Get(string query = null, int page = 1, int pageSize = 50)
        {
            var users = _userRepository.TableNoTracking
                .Where(x => (string.IsNullOrEmpty(query) || x.FirstName.Contains(query) || x.LastName.Contains(query)))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ProjectTo<UserModel>(_mapper.ConfigurationProvider);

            var pagedResponse = new PagedResponse<UserModel>(pageSize, page);
            await pagedResponse.PaginateResultsAsync(users);

            AddPaginationResponseHeaders(pagedResponse);

            return Ok(pagedResponse.Data);
        }

        [HttpGet("current")]
        public async Task<ActionResult<UserModel>> CurrentUser()
        {
            var user = await _userService.GetUserByIdAsync(User.GetUserId());
            if (user == null)
            {
                return Forbid(); // or 404?
            }

            var userModel = _mapper.Map<UserModel>(user);
            return Ok(userModel);
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterUserModel model)
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
