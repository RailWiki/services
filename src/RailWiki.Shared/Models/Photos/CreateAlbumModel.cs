using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Models.Photos
{
    public class CreateAlbumModel
    {
        public int UserId { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
