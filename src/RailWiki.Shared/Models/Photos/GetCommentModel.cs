using System;
using RailWiki.Shared.Models.Users;

namespace RailWiki.Shared.Models.Photos
{
    public class GetCommentModel : BaseModel
    {
        /// <summary>
        /// Comment type - Album, Photo
        /// </summary>
        public string EntityType { get; set; }

        public int EntityId { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        
        public virtual GetUserModel User { get; set; }

        public string CommentText { get; set; }

        public DateTime? FlaggedOn { get; set; }
        public int? FlagUserId { get; set; }
        public string FlagReason { get; set; }
    }

    public class CreateCommentModel
    {
        public string EntityType { get; set; }

        public int EntityId { get; set; }

        public string CommentText { get; set; }
    }
}
