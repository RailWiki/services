using RailWiki.Api.Models.Entities.Roster;

namespace RailWiki.Api.Models.Entities.Photos
{
    public class PhotoRollingStockModel : BaseModel
    {
        public int PhotoId { get; set; }
        public virtual PhotoModel Photo { get; set; }

        public int RollingStockId { get; set; }
        public virtual RollingStockModel RollingStock { get; set; }
    }
}
