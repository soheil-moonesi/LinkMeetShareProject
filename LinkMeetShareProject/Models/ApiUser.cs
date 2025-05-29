using Microsoft.AspNetCore.Identity;

namespace LinkMeetShareProject.Models
{
        public class ApiUser : IdentityUser
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
}
