using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Models.Roster
{
    public class LocomotiveTypeModel : BaseModel
    {
        [MaxLength(25)]
        public string Family { get; set; }

        [Required, MaxLength(50)]
        public string Manufacturer { get; set; }

        public string Name { get; set; }

        public int LocomotiveCount { get; set; }
    }
}
