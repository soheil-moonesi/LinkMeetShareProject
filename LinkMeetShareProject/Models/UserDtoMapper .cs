using System.Runtime.InteropServices;
using LinkMeetShareProject.Dto;
using LinkMeetShareProject.Models;
using Riok.Mapperly.Abstractions;


namespace LinkMeetShareProject
{
    [Mapper]
    public partial class UserDtoMapper
    {

        [MapProperty(nameof(ApiUserDto.FirstName), nameof(ApiUser.FirstName))] // Maps EmailDto→Email
        [MapProperty(nameof(ApiUserDto.LastName), nameof(ApiUser.LastName))]
        [MapProperty(nameof(ApiUserDto.email), nameof(ApiUser.Email))] // Maps EmailDto→Email
        [MapProperty(nameof(ApiUserDto.email), nameof(ApiUser.UserName))] // Maps EmailDto→Email
        public partial ApiUser dtoToUser(ApiUserDto user);

    }
}
