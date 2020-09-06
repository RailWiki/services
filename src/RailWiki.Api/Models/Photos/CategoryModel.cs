using System.ComponentModel.DataAnnotations;

namespace RailWiki.Api.Models.Photos
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
