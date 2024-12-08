using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piba.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableCanteAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CanteAvailabilites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteAvailabilites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanteAvailabilites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CanteAvailabilites_UserId",
                table: "CanteAvailabilites",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CanteAvailabilites");
        }
    }
}
