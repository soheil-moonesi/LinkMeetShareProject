namespace LinkMeetShareProject.Models
{
    public class UserPutAllDto
    {
        public string EmailDto { get; set; }

        public ICollection<MeetingLinkUser> MeetingLinkUserDto { get; set; }
    }
}
