using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RailWiki.Shared.Entities.Photos;

namespace RailWiki.Shared.Security
{
    public class OwnerOrModRequirement : IAuthorizationRequirement
    {
    }

    public class AlbumAuthorizationHandler : AuthorizationHandler<OwnerOrModRequirement, Album>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerOrModRequirement requirement, Album resource)
        {
            if (context.User?.GetUserId() == resource.UserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class PhotoAuthorizationHandler : AuthorizationHandler<OwnerOrModRequirement, Photo>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerOrModRequirement requirement, Photo resource)
        {
            if (context.User?.GetUserId() == resource.UserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
