namespace LinkMeetShareProject.Models
{
    public class User
    {
        public int UserKey { get; set; }
        public string Email { get; set; }
        public ICollection<MeetingLinkUser> UserEnrollLinks { get; set; }
    }
}
