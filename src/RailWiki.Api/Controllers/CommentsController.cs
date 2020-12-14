using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RailWiki.Shared.Models.Photos;
using RailWiki.Shared.Security;
using RailWiki.Shared.Services.Photos;
using RailWiki.Shared.Services.Users;

namespace RailWiki.Api.Controllers
{
    [Route("comments")]
    public class CommentsController : BaseApiController
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ICommentService commentService,
            IUserService userService,
            ILogger<CommentsController> logger)
        {
            _commentService = commentService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{type}/{entityId}")]
        [AllowAnonymous]
        public async Task<ActionResult<GetCommentModel>> Get(string type, int entityId)
        {
            var comments = await _commentService.GetCommentsAsync(type, entityId);

            return Ok(comments);
        }

        [HttpPost("{type}/{entityId}")]
        [Authorize(Policy = Policies.ApprovedUser)]
        public async Task<ActionResult> Create(string type, int entityId, CreateCommentModel model)
        {
            var user = await _userService.GetUserByIdAsync(User.GetUserId());

            model.EntityType = type;
            model.EntityId = entityId;

            var comment = await _commentService.CreateAsync(model, user);

            return Created("", comment);
        }
    }
}
