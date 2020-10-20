using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Models.Roster
{
    public class RoadModel : BaseModel
    {
        public int RoadTypeId { get; set; }
        public virtual RoadTypeModel RoadType { get; set; }

        public int? ParentId { get; set; }
        public virtual RoadModel Parent { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Slug { get; set; }

        [Required, MaxLength(10)]
        public string ReportingMarks { get; set; }

        public int LocomotiveCount { get; set; }
        public int RollingStockCount { get; set; }
    }
}
