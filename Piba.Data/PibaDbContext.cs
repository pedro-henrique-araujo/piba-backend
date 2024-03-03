using Microsoft.EntityFrameworkCore;
using Piba.Data.Entities;

namespace Piba.Data
{
    public class PibaDbContext : DbContext
    {
        public PibaDbContext(DbContextOptions<PibaDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<SchoolAttendance> SchoolAttendances { get; set; }
        public DbSet<SaturdayWithoutClass> SaturdayWithoutClasses { get; set; }

        public DbSet<Log> Logs { get; set; }
    }
}