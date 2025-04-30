namespace LinkMeetShareProject.Models
{
    public class MeetingLink
    {
        public int MeetingLinkKey { get; set; }
        public string Tittle { get; set; }
        public string Link { get; set; }
        public DateTime Time { get; set; }
        public ICollection<MeetingLinkUser> UsersJoinToMeet { get; set; }
    }
}
