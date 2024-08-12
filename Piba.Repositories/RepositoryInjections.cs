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
        }
    }
}
