using System.ComponentModel.DataAnnotations;

namespace RailWiki.Api.Models.Roster
{
    public class RollingStockTypeModel : BaseModel
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(10)]
        public string AARDesignation { get; set; }

        [MaxLength(255)]

        public string Description { get; set; }
    }
}
