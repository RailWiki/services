using RailWiki.Shared.Models.Roster;

namespace RailWiki.Shared.Models.Photos
{
    public class PhotoRollingStockModel : BaseModel
    {
        public int PhotoId { get; set; }
        public virtual PhotoModel Photo { get; set; }

        public int RollingStockId { get; set; }
        public virtual RollingStockModel RollingStock { get; set; }
    }
}
