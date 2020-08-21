using System.ComponentModel.DataAnnotations;

namespace RailWiki.Api.Models.Entities.Geography
{
    /// <summary>
    /// Represents a Country
    /// </summary>
    public class CountryModel : BaseModel
    {
        /// <summary>
        /// Country name
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// The country's ISO code / abbreviation
        /// </summary>
        [Required, MaxLength(5)]
        public string CountryCode { get; set; }
    }
}
