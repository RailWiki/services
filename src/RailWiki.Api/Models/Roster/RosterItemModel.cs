using System.ComponentModel.DataAnnotations;

namespace RailWiki.Api.Models.Roster
{
    public class RosterItemModel : BaseModel
    {
        public int RoadId { get; set; }
        public virtual RoadModel Road { get; set; }

        [Required, MaxLength(10)]
        public string RoadNumber { get; set; }

        /// <summary>
        /// The full reporting marks of the item (ROAD + NUMBER)
        /// </summary>
        [MaxLength(15)]
        public string ReportingMarks { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        [MaxLength(255)]
        public string Slug { get; set; }
    }
}
