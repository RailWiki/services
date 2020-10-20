using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Models.Photos
{
    /// <summary>
    /// Represents a category that a photo can be tagged with
    /// </summary>
    public class CategoryModel : BaseModel
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
