using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Piba.Data;
using Piba.Repositories;
using Piba.Repositories.Interfaces;
using Piba.Services;
using Piba.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PibaDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Piba");
    options.UseSqlServer(connectionString);
});


builder.Services.AddScoped<EmailService, EmailServiceImp>();
builder.Services.AddScoped<EnvironmentVariables, WebApiEnvironmentVariables>();
builder.Services.AddScoped<ExcelService, ExcelServiceImp>();
builder.Services.AddScoped<LogService, LogServiceImp>();
builder.Services.AddScoped<MemberService, MemberServiceImp>();
builder.Services.AddScoped<SaturdayWithoutClassService, SaturdayWithoutClassServiceImp>();
builder.Services.AddScoped<SchoolAttendanceService, SchoolAttendanceServiceImp>();
builder.Services.AddScoped<SmtpClientWrapper, SmtpClientWrapperImp>();
builder.Services.AddScoped<StatusHistoryService, StatusHistoryServiceImp>();

builder.Services.AddScoped<LogRepository, LogRepositoryImp>();
builder.Services.AddScoped<MemberRepository, MemberRepositoryImp>();
builder.Services.AddScoped<SaturdayWithoutClassRepository, SaturdayWithoutClassRepositoryImp>();
builder.Services.AddScoped<SchoolAttendanceRepository, SchoolAttendanceRepositoryImp>();
builder.Services.AddScoped<StatusHistoryItemRepository, StatusHistoryItemRepositoryImp>();
builder.Services.AddScoped<StatusHistoryRepository, StatusHistoryRepositoryImp>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<PibaDbContext>();
    dbContext.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.Run();
