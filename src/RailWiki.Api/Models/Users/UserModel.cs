using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailWiki.Api.Models.Users
{
    public class UserModel : BaseModel
    {
        // TODO: Probably shouldn't be exposed in APIs
        public string SubjectId { get; set; } // If we use Okta, this will be for the user's subject Id

        [Required, MaxLength(100)]
        public string EmailAddress { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public DateTime RegisteredOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        [NotMapped]
        public bool IsApproved => ApprovedOn.HasValue;
    }
}
