using System;

namespace RailWiki.Shared.Models.Photos
{
    public class AlbumResponseModel
    {
        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int PhotoCount { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
