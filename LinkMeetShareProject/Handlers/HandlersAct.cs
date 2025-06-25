using LinkMeetShareProject.Models;
using LiteBus.Queries.Abstractions;

namespace LinkMeetShareProject.Handlers
{
    public class HandlersAct
    {

        public class GetUserQueryHandler : IQueryHandler<GetUserQuery, User>
        {
            public Task<User> HandleAsync(GetUserQuery query, CancellationToken cancellationToken = default)
            {
                // Logic to retrieve a user
                return Task.FromResult(new User());
            }
        }



    }
}
