using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Models.Geography
{
    /// <summary>
    /// Represents a state or province
    /// </summary>
    public class StateProvinceModel : BaseModel
    {
        /// <summary>
        /// The related country ID
        /// </summary>
        public int CountryId { get; set; }

        public virtual CountryModel Country { get; set; }

        /// <summary>
        /// Name of the state or province
        /// </summary>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Two or three letter abbreviation
        /// </summary>
        [Required, MaxLength(5)]
        public string Abbreviation { get; set; }
    }
}
