using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RailWiki.Shared.Services.Users;

namespace RailWiki.Shared.Security.Requirements
{
    public class ApprovedUserRequirement : IAuthorizationRequirement
    {
        
    }

    public class ApprovedUserAuthorizationHandler : AuthorizationHandler<ApprovedUserRequirement>
    {
        private readonly IUserService _userService;

        public ApprovedUserAuthorizationHandler(IUserService userService)
        {
            _userService = userService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApprovedUserRequirement requirement)
        {
            var user = await _userService.GetUserByIdAsync(context.User.GetUserId());
            if (user?.IsApproved ?? false)
            {
                context.Succeed(requirement);
            }
        }
    }
}
