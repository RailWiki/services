using RailWiki.Shared.Entities.Roster;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailWiki.Shared.Entities.Photos
{
    public class PhotoLocomotive : BaseEntity
    {
        public int PhotoId { get; set; }
        [ForeignKey(nameof(PhotoId))]
        public virtual Photo Photo { get; set; }

        public int LocomotiveId { get; set; }
        [ForeignKey(nameof(LocomotiveId))]
        public virtual Locomotive Locomotive { get; set; }
    }
}
