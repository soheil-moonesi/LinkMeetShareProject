using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkMeetShareProject.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeetingLink",
                columns: table => new
                {
                    MeetingLinkKey = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tittle = table.Column<string>(type: "TEXT", nullable: false),
                    Link = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingLink", x => x.MeetingLinkKey);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserKey = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserKey);
                });

            migrationBuilder.CreateTable(
                name: "MeetingLinkUser",
                columns: table => new
                {
                    MeetingLinkKey_R = table.Column<int>(type: "INTEGER", nullable: false),
                    UserKey_R = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingLinkUser", x => new { x.MeetingLinkKey_R, x.UserKey_R });
                    table.ForeignKey(
                        name: "FK_MeetingLinkUser_MeetingLink_MeetingLinkKey_R",
                        column: x => x.MeetingLinkKey_R,
                        principalTable: "MeetingLink",
                        principalColumn: "MeetingLinkKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingLinkUser_User_UserKey_R",
                        column: x => x.UserKey_R,
                        principalTable: "User",
                        principalColumn: "UserKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingLinkUser_UserKey_R",
                table: "MeetingLinkUser",
                column: "UserKey_R");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetingLinkUser");

            migrationBuilder.DropTable(
                name: "MeetingLink");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
