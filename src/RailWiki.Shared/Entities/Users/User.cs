using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailWiki.Shared.Entities.Users
{
    public class User : BaseEntity
    {
        public string SubjectId { get; set; } // If we use Okta, this will be for the user's subject Id

        [Required, MaxLength(100)]
        public string EmailAddress { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        public DateTime RegisteredOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        [NotMapped]
        public bool IsApproved => ApprovedOn.HasValue;
    }
}
