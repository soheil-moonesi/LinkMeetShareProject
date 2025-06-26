    using LiteBus.Messaging.Abstractions;
using LiteBus.Queries.Abstractions;
    using LinkMeetShareProject.Models;

namespace LinkMeetShareProject
{

    public class GetUserQuery : IQuery<ApiUser>
    {
        public string emailPerson { get; }

        public GetUserQuery(string userId)
        {
            emailPerson = "test@example.com";

        }
    }
}
