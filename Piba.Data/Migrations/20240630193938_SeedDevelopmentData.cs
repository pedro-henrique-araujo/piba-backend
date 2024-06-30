using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piba.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDevelopmentData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (Environment.GetEnvironmentVariable("SEED")?.ToLower() != "true") return;

            migrationBuilder.InsertData(
                table: "Members",
                columns: ["Id", "Name", "RecurrentExcuse", "Status", "LastStatusUpdate"],
                values: new object[,] 
                {
                    { Guid.NewGuid().ToString(), "John", string.Empty, "1", "2024-01-01" },
                    { Guid.NewGuid().ToString(), "Jane", string.Empty, "1", "2024-01-01" },
                    { Guid.NewGuid().ToString(), "Tony", string.Empty, "2", "2024-01-01" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
