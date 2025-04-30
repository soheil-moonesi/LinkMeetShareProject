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
           // base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MeetingLinkUser>()
                .HasKey("MeetingLinkId", "UserId");

            modelBuilder.Entity<MeetingLink>()
                .HasMany(p => p.Users).WithOne(p => p.MeetingLink)
                .HasForeignKey(p => p.MeetingLinkId);

            modelBuilder.Entity<User>().HasMany(p => p.Links)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

        }
    }
}   
