using System.ComponentModel.DataAnnotations.Schema;

namespace RailWiki.Shared.Entities.Photos
{
    public class PhotoCategory : BaseEntity
    {
        public int PhotoId { get; set; }
        [ForeignKey(nameof(PhotoId))]
        public virtual Photo Photo { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
    }
}
