﻿// <auto-generated />
using System;
using LinkMeetShareProject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LinkMeetShareProject.Migrations
{
    [DbContext(typeof(LinkMeetShareProjectDbContext))]
    [Migration("20250430175651_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.15");

            modelBuilder.Entity("LinkMeetShareProject.Models.MeetingLink", b =>
                {
                    b.Property<int>("MeetingLinkKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tittle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("MeetingLinkKey");

                    b.ToTable("MeetingLink");
                });

            modelBuilder.Entity("LinkMeetShareProject.Models.MeetingLinkUser", b =>
                {
                    b.Property<int>("MeetingLinkKey_R")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserKey_R")
                        .HasColumnType("INTEGER");

                    b.HasKey("MeetingLinkKey_R", "UserKey_R");

                    b.HasIndex("UserKey_R");

                    b.ToTable("MeetingLinkUser");
                });

            modelBuilder.Entity("LinkMeetShareProject.Models.User", b =>
                {
                    b.Property<int>("UserKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserKey");

                    b.ToTable("User");
                });

            modelBuilder.Entity("LinkMeetShareProject.Models.MeetingLinkUser", b =>
                {
                    b.HasOne("LinkMeetShareProject.Models.MeetingLink", "MeetingLink_R")
                        .WithMany("UsersJoinToMeet")
                        .HasForeignKey("MeetingLinkKey_R")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LinkMeetShareProject.Models.User", "User_R")
                        .WithMany("UserEnrollLinks")
                        .HasForeignKey("UserKey_R")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MeetingLink_R");

                    b.Navigation("User_R");
                });

            modelBuilder.Entity("LinkMeetShareProject.Models.MeetingLink", b =>
                {
                    b.Navigation("UsersJoinToMeet");
                });

            modelBuilder.Entity("LinkMeetShareProject.Models.User", b =>
                {
                    b.Navigation("UserEnrollLinks");
                });
#pragma warning restore 612, 618
        }
    }
}
