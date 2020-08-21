namespace RailWiki.Api.Models.Entities.Photos
{
    public class PhotoCategoryModel : BaseModel
    {
        public int PhotoId { get; set; }
        public virtual PhotoModel Photo { get; set; }

        public int CategoryId { get; set; }
        public virtual CategoryModel Category { get; set; }
    }
}
