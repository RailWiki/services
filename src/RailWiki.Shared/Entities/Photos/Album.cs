using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RailWiki.Shared.Entities.Geography;
using RailWiki.Shared.Entities.Users;

namespace RailWiki.Shared.Entities.Photos
{
    public class Album : BaseEntity
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int PhotoCount { get; set; }

        public int? CoverPhotoId { get; set; }
        [MaxLength(255)]
        public string CoverPhotoFileName { get; set;  }

        public int? LocationId { get; set; }
        [ForeignKey(nameof(LocationId))]
        public virtual Location Location { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
