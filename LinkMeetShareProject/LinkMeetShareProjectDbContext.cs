using LinkMeetShareProject.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LinkMeetShareProject
{
    public class LinkMeetShareProjectDbContext : DbContext
    {
        public LinkMeetShareProjectDbContext(DbContextOptions<LinkMeetShareProjectDbContext> Options) : base(Options)
        {

        } 

        public DbSet<MeetingLinkUser> MeetingLinkUser { get; set; }
        public DbSet<MeetingLink> MeetingLink { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasKey("UserKey");
            modelBuilder.Entity<MeetingLink>().HasKey("MeetingLinkKey");

            modelBuilder.Entity<MeetingLinkUser>()
                .HasKey("MeetingLinkKey_R", "UserKey_R");

            modelBuilder.Entity<MeetingLink>()
                .HasMany(p => p.UsersJoinToMeet).WithOne(p =>p.MeetingLink_R)
                .HasForeignKey(p => p.MeetingLinkKey_R);

            modelBuilder.Entity<User>().HasMany(p => p.UserEnrollLinks)
                .WithOne(p => p.User_R)
                .HasForeignKey(p => p.UserKey_R);

        }
    }
}   
