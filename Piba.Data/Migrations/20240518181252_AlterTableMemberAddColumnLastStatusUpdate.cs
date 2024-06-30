using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piba.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableMemberAddColumnLastStatusUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastStatusUpdate",
                table: "Members",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastStatusUpdate",
                table: "Members");
        }
    }
}
