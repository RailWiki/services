using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Models.Geography
{
    /// <summary>
    /// Represents a location where a photo could be tagged at
    /// </summary>
    public class LocationModel : BaseModel
    {
        /// <summary>
        /// The "friendly" name of the location. Typically should be "{city}, {state}"
        /// </summary>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Latitude of the location
        /// </summary>
        public float? Latitude { get; set; }
        /// <summary>
        /// Longitude of the location
        /// </summary>
        public float? Longitude { get; set; }

        /// <summary>
        /// The location's state or province ID
        /// </summary>
        public int? StateProvinceId { get; set; }

        /// <summary>
        /// Readonly. To set the StateProvince, set
        /// </summary>
        public StateProvinceModel StateProvince { get; set; }
    }
}
