using LiteBus.Messaging.Abstractions;
using Microsoft.AspNetCore.Identity;
using LinkMeetShareProject.Models;
using LinkMeetShareProject;
using ShareLib;
using LiteBus.Queries.Abstractions;
using System.Runtime.CompilerServices;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, ApiUser>
{
    private readonly UserManager<ApiUser> _userManager;

    public GetUserQueryHandler(UserManager<ApiUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApiUser> HandleAsync(GetUserQuery message, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync( message.UserId.ToString());
        if (user == null) return null;

        return new ApiUser
        {
            Id = user.UserName,
            Email = user.Email,
            // Map other properties as needed
        };
    }
}