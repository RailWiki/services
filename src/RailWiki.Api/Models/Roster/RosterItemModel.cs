using System.ComponentModel.DataAnnotations;

namespace RailWiki.Api.Models.Roster
{
    public class RosterItemModel : BaseModel
    {
        public int RoadId { get; set; }
        public virtual RoadModel Road { get; set; }

        [Required, MaxLength(10)]
        public string RoadNumber { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        [MaxLength(255)]
        public string Slug { get; set; }
    }
}
