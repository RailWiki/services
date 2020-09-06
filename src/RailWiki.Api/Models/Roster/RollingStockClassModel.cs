using System.ComponentModel.DataAnnotations;

namespace RailWiki.Api.Models.Roster
{
    public class RollingStockClassModel : BaseModel
    {
        public int RollingStockTypeId { get; set; }
        public virtual RollingStockTypeModel RollingStockType { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(10)]
        public string AARDesignation { get; set; }
    }
}
