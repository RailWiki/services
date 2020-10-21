using System;

namespace RailWiki.Shared.Models.Users
{
    public class GetUserModel : BaseModel
    {
        public string EmailAddress { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Slug { get; set; }

        public DateTime RegisteredOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool IsApproved => ApprovedOn.HasValue;
    }
}
