using System;
using System.ComponentModel.DataAnnotations;
using RailWiki.Api.Models.Entities.Users;

namespace RailWiki.Api.Models.Entities.Photos
{
    public class AlbumModel : BaseModel
    {
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int PhotoCount { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
