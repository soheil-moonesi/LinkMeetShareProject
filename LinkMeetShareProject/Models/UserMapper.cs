using System.Runtime.InteropServices;
using LinkMeetShareProject.Dto;
using LinkMeetShareProject.Models;
using Riok.Mapperly.Abstractions;


namespace LinkMeetShareProject
{
    [Mapper]
    public partial class UserMapper
    {
        [MapProperty(nameof(UserAddDto.EmailDto), nameof(User.Email))] // Maps EmailDto→Email
        public partial User UserAddDtoToUser(UserAddDto user);

        [MapProperty(nameof(UserPutAllDto.EmailDto), nameof(User.Email))] //
        [MapProperty(nameof(UserPutAllDto.MeetingLinkUserDto), nameof(User.UserEnrollLinks))] // 
        public partial User UserPutAllDtoToUser(UserPutAllDto user);

        [MapProperty(nameof(MeetingLinkUserDto.MeetingLinkKey_R), nameof(MeetingLinkUser.MeetingLinkKey_R))] //
        [MapProperty(nameof(MeetingLinkUserDto.UserKey_R), nameof(MeetingLinkUser.UserKey_R))] // 
        public partial MeetingLinkUser  MeetingPutAllDtoToMeetingLinkUser(MeetingLinkUserDto value);
    }


}
