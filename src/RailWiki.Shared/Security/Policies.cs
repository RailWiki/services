namespace RailWiki.Shared.Security
{
    public static class Policies
    {
        /// <summary>
        /// The user must be the owner of the album or a site moderator
        /// </summary>
        public const string AlbumOwnerOrMod = nameof(AlbumOwnerOrMod);

        /// <summary>
        /// The user must be the owner of the photo or a site moderator
        /// </summary>
        public const string PhotoOwnerOrMod = nameof(PhotoOwnerOrMod);
    }
}
