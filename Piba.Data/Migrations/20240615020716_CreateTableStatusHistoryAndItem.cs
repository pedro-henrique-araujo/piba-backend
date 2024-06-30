using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piba.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableStatusHistoryAndItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusHistoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusHistoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusHistoryItems_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatusHistoryItems_StatusHistories_StatusHistoryId",
                        column: x => x.StatusHistoryId,
                        principalTable: "StatusHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistoryItems_MemberId",
                table: "StatusHistoryItems",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistoryItems_StatusHistoryId",
                table: "StatusHistoryItems",
                column: "StatusHistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusHistoryItems");

            migrationBuilder.DropTable(
                name: "StatusHistories");
        }
    }
}
