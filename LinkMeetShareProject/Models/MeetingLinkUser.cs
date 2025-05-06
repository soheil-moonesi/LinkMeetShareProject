namespace LinkMeetShareProject.Models
{
    public class MeetingLinkUser
    {
        public int? MeetingLinkKey_R { get; set; }
        public MeetingLink? MeetingLink_R { get; set; }
        public int? UserKey_R { get; set; }
        public User? User_R { get; set; }

    }
}
