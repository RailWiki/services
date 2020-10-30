using System;
using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Models.Photos
{
    public class UpdatePhotoModel : BaseModel
    {
        public int AlbumId { get; set; }

        [MaxLength(100)]
        public string Author { get; set; }

        [MaxLength(100)]
        public string LocationName { get; set; }

        public int? LocationId { get; set; }
        
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime? PhotoDate { get; set; }
    }
}
