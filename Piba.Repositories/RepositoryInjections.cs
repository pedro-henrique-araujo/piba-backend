using Microsoft.Extensions.DependencyInjection;
using Piba.Repositories.Interfaces;

namespace Piba.Repositories
{
    public class RepositoryInjections
    {
        public static void Inject(IServiceCollection services)
        {
            services.AddScoped<LogRepository, LogRepositoryImp>();
            services.AddScoped<MemberRepository, MemberRepositoryImp>();
            services.AddScoped<SaturdayWithoutClassRepository, SaturdayWithoutClassRepositoryImp>();
            services.AddScoped<SchoolAttendanceRepository, SchoolAttendanceRepositoryImp>();
            services.AddScoped<StatusHistoryItemRepository, StatusHistoryItemRepositoryImp>();
            services.AddScoped<StatusHistoryRepository, StatusHistoryRepositoryImp>();
            services.AddScoped<UserRepository, UserRepositoryImp>();
            services.AddScoped<AuthenticationRepository, AuthenticationRepositoryImp>();
            services.AddScoped<AuthorizationRepository, AuthorizationRepositoryImp>();
            services.AddScoped<SessionAttendanceRepository, SessionAttendanceRepositoryImp>();
            services.AddScoped<SessionAttendanceItemRepository, SessionAttendanceItemRepositoryImp>();
            services.AddScoped<SongRepository, SongRepositoryImp>();
            services.AddScoped<RoleRepository, RoleRepositoryImp>();
            services.AddScoped<LinkRepository, LinkRepositoryImp>();
        }
    }
}
