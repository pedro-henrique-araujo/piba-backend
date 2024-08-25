using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piba.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableSessionAttendanceItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionAttendanceItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionAttendanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionAttendanceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionAttendanceItems_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionAttendanceItems_SessionAttendances_SessionAttendanceId",
                        column: x => x.SessionAttendanceId,
                        principalTable: "SessionAttendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionAttendanceItems_MemberId",
                table: "SessionAttendanceItems",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionAttendanceItems_SessionAttendanceId",
                table: "SessionAttendanceItems",
                column: "SessionAttendanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionAttendanceItems");
        }
    }
}
