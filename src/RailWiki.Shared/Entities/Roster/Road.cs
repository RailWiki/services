using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailWiki.Shared.Entities.Roster
{
    public class Road : BaseEntity
    {
        public int RoadTypeId { get; set; }
        [ForeignKey(nameof(RoadTypeId))]
        public virtual RoadType RoadType { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual Road Parent { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Slug { get; set; }

        [Required, MaxLength(10)]
        public string ReportingMarks { get; set; }

        public virtual List<RoadAlternateName> AlternateNames { get; set; } = new List<RoadAlternateName>();

        public int LocomotiveCount { get; set; }
        public int RollingStockCount { get; set; }
    }
}
