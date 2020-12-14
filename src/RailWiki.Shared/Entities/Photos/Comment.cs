using System;
using RailWiki.Shared.Entities.Users;

namespace RailWiki.Shared.Entities.Photos
{
    public class Comment : BaseEntity
    {
        /// <summary>
        /// Comment type - Album, Photo
        /// </summary>
        public string EntityType { get; set; }

        public int EntityId { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public string CommentText { get; set; }

        public DateTime? FlaggedOn { get; set; }
        public int? FlagUserId { get; set; }
        public string FlagReason { get; set; }
    }
}
