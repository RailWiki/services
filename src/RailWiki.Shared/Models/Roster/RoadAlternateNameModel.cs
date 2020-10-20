using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Models.Roster
{
    public class RoadAlternateNameModel : BaseModel
    {
        public int RoadId { get; set; }
        public virtual RoadModel Road { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(10)]
        public string ReportingMarks { get; set; }
    }
}
