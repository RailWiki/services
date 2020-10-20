using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Models.Roster
{
    public class LocomotiveModel : RosterItemModel
    {
        [MaxLength(25)]
        public string ModelNumber { get; set; }
        [MaxLength(50)]
        public string SerialNumber { get; set; }
        [MaxLength(50)]
        public string FrameNumber { get; set; }

        [MaxLength(50)]
        public string BuiltAs { get; set; }

        public int? BuildMonth { get; set; }
        public int? BuildYear { get; set; }
    }
}
