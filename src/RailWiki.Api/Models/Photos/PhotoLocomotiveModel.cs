using RailWiki.Api.Models.Roster;

namespace RailWiki.Api.Models.Photos
{
    public class PhotoLocomotiveModel : BaseModel
    {
        public int PhotoId { get; set; }
        public virtual PhotoModel Photo { get; set; }

        public int LocomotiveId { get; set; }
        public virtual LocomotiveModel Locomotive { get; set; }
    }
}
