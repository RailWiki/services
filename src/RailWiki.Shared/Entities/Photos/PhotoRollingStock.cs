using RailWiki.Shared.Entities.Roster;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailWiki.Shared.Entities.Photos
{
    public class PhotoRollingStock : BaseEntity
    {
        public int PhotoId { get; set; }
        [ForeignKey(nameof(PhotoId))]
        public virtual Photo Photo { get; set; }

        public int RollingStockId { get; set; }
        [ForeignKey(nameof(RollingStockId))]
        public virtual RollingStock RollingStock { get; set; }
    }
}
