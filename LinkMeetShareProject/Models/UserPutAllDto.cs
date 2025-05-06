namespace LinkMeetShareProject.Models
{
    public class UserPutAllDto
    {
        public string EmailDto { get; set; }

        public ICollection<MeetingLinkUserDto> MeetingLinkUserDto { get; set; }
    }
}
