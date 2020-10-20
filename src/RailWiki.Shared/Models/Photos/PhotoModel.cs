using System;
using System.ComponentModel.DataAnnotations;
using RailWiki.Shared.Models.Geography;
using RailWiki.Shared.Models.Users;

namespace RailWiki.Shared.Models.Photos
{
    [Obsolete("Use GetPhotoModel")]
    public class PhotoModel : BaseModel
    {
        public int AlbumId { get; set; }
        public virtual GetAlbumModel Album { get; set; }

        public int UserId { get; set; }
        public virtual GetUserModel User { get; set; }

        [MaxLength(50)]
        public string Author { get; set; }

        [MaxLength(50)]
        public string LocationName { get; set; }

        public int? LocationId { get; set; }
        public virtual LocationModel Location { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(509)]
        public string Description { get; set; }

        [MaxLength(255), Required]
        public string Filename { get; set; }

        public DateTime? PhotoDate { get; set; }
        public DateTime UploadDate { get; set; }

        public int ViewCount { get; set; }
    }
}
