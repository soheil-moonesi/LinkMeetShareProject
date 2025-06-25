    using LiteBus.Messaging.Abstractions;
using LiteBus.Queries.Abstractions;
    using LinkMeetShareProject.Models;

namespace LinkMeetShareProject
{

    public class GetUserQuery : IQuery<ApiUser>
    {
        public int UserId { get; }

        public GetUserQuery(int userId)
        {
            UserId = userId;
        }
    }
}
