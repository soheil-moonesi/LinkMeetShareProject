namespace LinkMeetShareProject.Models
{
    public class MeetingLinkUser
    {
        public int MeetingLinkId { get; set; }
        public MeetingLink MeetingLink { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }
}
