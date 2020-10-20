using System;
using System.ComponentModel.DataAnnotations;
using RailWiki.Shared.Models.Users;

namespace RailWiki.Shared.Models.Photos
{
    public class GetAlbumModel : BaseModel
    {
        public int UserId { get; set; }
        public virtual GetUserModel User { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int PhotoCount { get; set; }

        public int? CoverPhotoId { get; set; }
        [MaxLength(255)]
        public string CoverPhotoFileName { get; set; }
        public string CoverPhotoUrl { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
