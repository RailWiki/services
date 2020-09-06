using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RailWiki.Api.Models.Users;
using RailWiki.Api.Models.Users;
using RailWiki.Shared.Services.Users;

namespace RailWiki.Api.Controllers
{
    [Route("users")]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService,
            IMapper mapper,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
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
