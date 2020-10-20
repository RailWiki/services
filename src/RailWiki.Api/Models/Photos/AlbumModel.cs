using System;
using System.ComponentModel.DataAnnotations;
using RailWiki.Api.Models.Users;

namespace RailWiki.Api.Models.Photos
{
    [Obsolete("Use Shared GetAlbumModel or create a specific model for implementation")]
    public class AlbumModel : BaseModel
    {
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int PhotoCount { get; set; }

        public int? CoverPhotoId { get; set; }
        [MaxLength(255)]
        public string CoverPhotoFileName { get; set; }
        public string CoverPhotoUrl { get; set;  }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
