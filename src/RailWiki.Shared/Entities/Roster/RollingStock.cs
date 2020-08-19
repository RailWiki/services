using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailWiki.Shared.Entities.Roster
{
    public class RollingStock : RosterItem
    {
        public int RollingStockTypeId { get; set; }
        [ForeignKey(nameof(RollingStockTypeId))]
        public virtual RollingStockType RollingStockType { get; set; }
        public int RollingStockClassId { get; set; }
        [ForeignKey(nameof(RollingStockClassId))]
        public virtual RollingStockClass RollingStockClass { get; set; }

        [MaxLength(500)]
        public string Details { get; set; }

        [MaxLength(25)]
        public string Plate { get; set; }

        public int MaxGrossWeight { get; set; }
        public int LoadLimit { get; set; }
        public int DryCapacity { get; set; }

        public string ExteriorDimensions { get; set; }
        public string InteriorDimensions { get; set; }
    }
}
