using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Entities.Geography
{
    /// <summary>
    /// Represents a Country
    /// </summary>
    public class Country : BaseEntity
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

        public virtual ICollection<StateProvince> StateProvinces { get; set; } = new List<StateProvince>();
    }
}
