using System.Runtime.InteropServices;
using LinkMeetShareProject.Models;
using Riok.Mapperly.Abstractions;


namespace LinkMeetShareProject
{   
    [Mapper]
    public partial class UserMapper
    {
        [MapProperty(nameof(UserAddDto.EmailDto), nameof(User.Email))] // Maps EmailDto→Email
        public partial User UserAddDtoToUser(UserAddDto user);
    }
}
