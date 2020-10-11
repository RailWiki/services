using System.Security.Claims;

namespace RailWiki.Shared.Security
{
    public static class UserExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return 0;
            }

            var userIdClaim = principal.FindFirst(x => x.Type == AppClaimTypes.UserId && x.Issuer == AppClaimTypes.AppClaimsIssuer);

            return int.TryParse(userIdClaim?.Value, out var userId) ? userId : 0;
        }
    }
}
