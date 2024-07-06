using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualBasic;

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

            var members = new object[,]
            {
                { Guid.NewGuid().ToString(), "John", string.Empty, "1", "2024-01-01" },
                { Guid.NewGuid().ToString(), "Jane", string.Empty, "1", "2024-01-01" },
                { Guid.NewGuid().ToString(), "Tony", string.Empty, "2", "2024-01-01" },
                { Guid.NewGuid().ToString(), "Peter", "Some excuse", "3", "2024-01-01"}
            };

            migrationBuilder.InsertData(
                table: "Members",
                columns: ["Id", "Name", "RecurrentExcuse", "Status", "LastStatusUpdate"],
                values: members);


            migrationBuilder.InsertData(
                table: "SchoolAttendances",
                columns: ["Id", "MemberId", "CreatedDate", "IsPresent", "Excuse"],
                values: new object[,]
                {
                    { Guid.NewGuid().ToString(), members[0,0], DateTime.UtcNow.AddMonths(-1), true, string.Empty },
                    { Guid.NewGuid().ToString(), members[1,0], DateTime.UtcNow.AddMonths(-1), true, string.Empty },
                    { Guid.NewGuid().ToString(), members[2,0], DateTime.UtcNow.AddMonths(-1), false, "Doctor's appointment" },
                    { Guid.NewGuid().ToString(), members[0,0], DateTime.UtcNow.AddMonths(-1), true, string.Empty },
                    { Guid.NewGuid().ToString(), members[1,0], DateTime.UtcNow.AddMonths(-1), true, string.Empty },
                    { Guid.NewGuid().ToString(), members[2,0], DateTime.UtcNow.AddMonths(-1), true, string.Empty },
                    { Guid.NewGuid().ToString(), members[0,0], DateTime.UtcNow.AddMonths(-1), true, string.Empty },
                    { Guid.NewGuid().ToString(), members[1,0], DateTime.UtcNow.AddMonths(-1), true, string.Empty },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE [SchoolAttendances]");
            migrationBuilder.Sql("DELETE [Members]");
        }
    }
}
