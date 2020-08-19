using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailWiki.Shared.Entities.Geography
{
    /// <summary>
    /// Represents a state or province
    /// </summary>
    public class StateProvince : BaseEntity
    {
        /// <summary>
        /// The related country ID
        /// </summary>
        public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }

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
