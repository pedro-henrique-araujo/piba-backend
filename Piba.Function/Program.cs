using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Piba.Data;
using Piba.Repositories;
using Piba.Repositories.Interfaces;
using Piba.Services;
using Piba.Services.Interfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddScoped<MemberService, MemberServiceImp>();
        services.AddScoped<SchoolAttendanceService, SchoolAttendanceServiceImp>();
        services.AddScoped<SaturdayWithoutClassService, SaturdayWithoutClassServiceImp>();
        services.AddScoped<LogService, LogServiceImp>();
        services.AddScoped<EmailService, EmailServiceImp>();

        services.AddScoped<MemberRepository, MemberRepositoryImp>();
        services.AddScoped<SchoolAttendanceRepository, SchoolAttendanceRepositoryImp>();
        services.AddScoped<SaturdayWithoutClassRepository, SaturdayWithoutClassRepositoryImp>();
        services.AddScoped<LogRepository, LogRepositoryImp>();

        services.AddScoped<SmtpClientWrapper, SmtpClientWrapperImp>();
        services.AddScoped<EnvironmentVariables, EnvironmentVariablesImp>();

        services.AddDbContext<PibaDbContext>(options =>
        {
            options.UseSqlServer(Environment.GetEnvironmentVariable("DatabaseConnectionString"));
        });
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();
