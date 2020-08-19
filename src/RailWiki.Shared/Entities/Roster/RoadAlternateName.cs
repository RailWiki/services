using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailWiki.Shared.Entities.Roster
{
    public class RoadAlternateName : BaseEntity
    {
        public int RoadId { get; set; }
        [ForeignKey(nameof(RoadId))]
        public virtual Road Road { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(10)]
        public string ReportingMarks { get; set; }
    }
}
