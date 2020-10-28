namespace RailWiki.Shared.Models.Geography
{
    /// <summary>
    /// Represents a [read-only] location where a photo could be tagged at
    /// </summary>
    public class GetLocationModel : BaseModel
    {
        /// <summary>
        /// The "friendly" name of the location. Typically should be "{city}, {state}"
        /// </summary>
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
        /// The location's state or province name
        /// </summary>

        public string StateProvinceName { get; set; }

        /// <summary>
        /// The location's state or province abbreviation
        /// </summary>
        public string StateProvinceAbbreviation { get; set; }

        /// <summary>
        /// The location's country abbreviation
        /// </summary>
        public string CountryAbbreviation { get; set; }
    }
}
