using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Models.Roster
{
    public class RollingStockModel : RosterItemModel
    {
        public int RollingStockTypeId { get; set; }
        public virtual RollingStockTypeModel RollingStockType { get; set; }
        public int RollingStockClassId { get; set; }
        public virtual RollingStockClassModel RollingStockClass { get; set; }

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
