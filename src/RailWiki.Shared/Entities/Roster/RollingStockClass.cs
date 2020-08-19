using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailWiki.Shared.Entities.Roster
{
    public class RollingStockClass : BaseEntity
    {
        public int RollingStockTypeId { get; set; }
        [ForeignKey(nameof(RollingStockTypeId))]
        public virtual RollingStockType RollingStockType { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(10)]
        public string AARDesignation { get; set; }
    }
}
