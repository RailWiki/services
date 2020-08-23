using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RailWiki.Api.Models.Entities.Roster
{
    public class RoadTypeModel : BaseModel
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        public string Slug { get; set; }

        public int DisplayOrder { get; set; }

        public virtual List<RoadModel> Roads { get; set; }
    }
}
