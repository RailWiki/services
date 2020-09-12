using System.ComponentModel.DataAnnotations;

namespace RailWiki.Shared.Entities
{
    public class CrossReference : BaseEntity
    {
        [Required,MaxLength(50)]
        public string EntityType { get; set; }
        public int EntityId { get; set; }

        [Required, MaxLength(50)]
        public string Source { get; set; }

        [Required, MaxLength(50)]
        public string SourceIdentifier { get; set; }        
    }
}
