using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RailWiki.Shared.Entities.Geography;

namespace RailWiki.Shared.Models.Photos
{

    public class PhotoResponseModel
    {
        public int Id { get; set; }

        public int AlbumId { get; set; }
        public AlbumResponseModel Album { get; set; }

        public int UserId { get; set; }
        //public virtual User User { get; set; }

        [MaxLength(50)]
        public string Author { get; set; }

        [MaxLength(50)]
        public string LocationName { get; set; }

        public int? LocationId { get; set; }
        public virtual Location Location { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(509)]
        public string Description { get; set; }

        [MaxLength(255), Required]
        public string Filename { get; set; }

        public DateTime? PhotoDate { get; set; }
        public DateTime UploadDate { get; set; }

        public int ViewCount { get; set; }

        public Dictionary<string, string> Files { get; set; } = new Dictionary<string, string>();
    }
}
