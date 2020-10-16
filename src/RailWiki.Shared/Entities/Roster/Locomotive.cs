using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailWiki.Shared.Entities.Roster
{
    public class Locomotive : RosterItem
    {
        public int? TypeId { get; set; }
        [ForeignKey(nameof(TypeId))]
        public virtual LocomotiveType Model { get; set; }

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
