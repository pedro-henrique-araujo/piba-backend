using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using Piba.Data;
using Piba.Repositories;
using Piba.Repositories.Interfaces;
using Piba.Services;
using Piba.Services.Interfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        services.AddScoped<EmailService, EmailServiceImp>();
        services.AddScoped<EnvironmentVariables, FunctionEnvironmentVariables>();
        services.AddScoped<ExcelService, ExcelServiceImp>();
        services.AddScoped<LogService, LogServiceImp>();
        services.AddScoped<MemberService, MemberServiceImp>();
        services.AddScoped<SaturdayWithoutClassService, SaturdayWithoutClassServiceImp>();
        services.AddScoped<SchoolAttendanceService, SchoolAttendanceServiceImp>();
        services.AddScoped<SmtpClientWrapper, SmtpClientWrapperImp>();
        services.AddScoped<StatusHistoryService, StatusHistoryServiceImp>();


        services.AddScoped<LogRepository, LogRepositoryImp>();
        services.AddScoped<MemberRepository, MemberRepositoryImp>();
        services.AddScoped<SaturdayWithoutClassRepository, SaturdayWithoutClassRepositoryImp>();
        services.AddScoped<SchoolAttendanceRepository, SchoolAttendanceRepositoryImp>();
        services.AddScoped<StatusHistoryItemRepository, StatusHistoryItemRepositoryImp>();
        services.AddScoped<StatusHistoryRepository, StatusHistoryRepositoryImp>();


        services.AddDbContext<PibaDbContext>(options =>
        {
            options.UseSqlServer(Environment.GetEnvironmentVariable("DatabaseConnectionString"));
        });
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();
