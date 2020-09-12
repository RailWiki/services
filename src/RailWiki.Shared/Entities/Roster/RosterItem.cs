using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailWiki.Shared.Entities.Roster
{
    public class RosterItem : BaseEntity
    {
        public int RoadId { get; set; }
        [ForeignKey(nameof(RoadId))]
        public virtual Road Road { get; set; }

        [Required, MaxLength(10)]
        public string RoadNumber { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        [MaxLength(255)]
        public string Slug { get; set; }
    }
}
