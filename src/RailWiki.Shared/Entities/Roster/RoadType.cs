using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Entities.Roster
{
    public class RoadType : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public virtual List<Road> Roads { get; set; }
    }
}
