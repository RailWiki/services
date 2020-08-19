using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Entities.Photos
{
    /// <summary>
    /// Represents a category that a photo can be tagged with
    /// </summary>
    public class Category : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
