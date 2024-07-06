using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piba.Data.Migrations
{
    /// <inheritdoc />
    public partial class SchoolAtendancesMemberIdForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SchoolAttendances_MemberId",
                table: "SchoolAttendances",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolAttendances_Members_MemberId",
                table: "SchoolAttendances",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolAttendances_Members_MemberId",
                table: "SchoolAttendances");

            migrationBuilder.DropIndex(
                name: "IX_SchoolAttendances_MemberId",
                table: "SchoolAttendances");
        }
    }
}
