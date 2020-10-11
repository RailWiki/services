namespace RailWiki.Shared.Security
{
    public static class AppClaimTypes
    {
        // Set the claims issuer so we can validate the claims we issue
        public const string AppClaimsIssuer = "https://railwikiapi";

        public const string UserId = "UserId";
    }
}
