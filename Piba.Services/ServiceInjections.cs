using Microsoft.Extensions.DependencyInjection;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class ServiceInjections
    {
        public static void Inject(IServiceCollection services)
        {
            services.AddScoped<EmailService, EmailServiceImp>();
            services.AddScoped<EnvironmentVariables, WebApiEnvironmentVariables>();
            services.AddScoped<ExcelService, ExcelServiceImp>();
            services.AddScoped<LogService, LogServiceImp>();
            services.AddScoped<MemberService, MemberServiceImp>();
            services.AddScoped<SaturdayWithoutClassService, SaturdayWithoutClassServiceImp>();
            services.AddScoped<SchoolAttendanceService, SchoolAttendanceServiceImp>();
            services.AddScoped<SmtpClientWrapper, SmtpClientWrapperImp>();
            services.AddScoped<StatusHistoryService, StatusHistoryServiceImp>();
            services.AddScoped<UserService, UserServiceImp>();
            services.AddScoped<GoogleLoginService, GoogleLoginServiceImp>();
            services.AddScoped<GoogleWebSignatureService, GoogleWebSignatureServiceImp>();
            services.AddScoped<JwtService, JwtServiceImp>();
        }
    }
}
