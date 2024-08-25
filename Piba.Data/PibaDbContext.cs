using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Piba.Data.Entities;

namespace Piba.Data
{
    public class PibaDbContext : IdentityDbContext
    {
        public PibaDbContext(DbContextOptions<PibaDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<SessionAttendance> SessionAttendances { get; set; }
        public DbSet<SessionAttendanceItem> SessionAttendanceItems { get; set; }
        public DbSet<SchoolAttendance> SchoolAttendances { get; set; }
        public DbSet<SaturdayWithoutClass> SaturdayWithoutClasses { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }
        public DbSet<StatusHistoryItem> StatusHistoryItems { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}